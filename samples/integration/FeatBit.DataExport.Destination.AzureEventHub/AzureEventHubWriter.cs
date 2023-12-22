using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Azure.Messaging.EventHubs.Processor;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Storage.Blobs;
using System.Text.Json;


namespace FeatBit.DataExport.Destination.AzureEventHub
{
    public class AzureEventHubWriter
    {
        private readonly EventHubProducerClient _producerClient;
        private readonly string _eventhubConnectionString;
        private readonly string _eventHubPlan;
        public AzureEventHubWriter(string azureEventHubConnectionString, string eventHubPlan)
        {
            _eventhubConnectionString = azureEventHubConnectionString;
            _eventHubPlan = eventHubPlan;
            _producerClient = new EventHubProducerClient(
                azureEventHubConnectionString,
                "flagvaluecapture");
        }


        public async Task<(bool ifAllSent, string lastSentTimeStamp)> WriteFlagValueEventsBatchAsync(
            List<FlagValueEvent> flagValueEvents)
        {
            string lastSentTimeStamp = "";
            int batchSize = 100;
            switch(_eventHubPlan)
            {
                case "Basic":
                    batchSize = 100;
                    break;
                case "Standard":
                    batchSize = 500;
                    break;
                case "Premium":
                    batchSize = 500;
                    break;
                case "Dedicated":
                    batchSize = 500;
                    break;
                default:
                    batchSize = 100;
                    break;
            }

            using EventDataBatch eventBatch = await _producerClient.CreateBatchAsync();

            int batchIndex = 0, totalEvent = flagValueEvents.Count;
            while (true)
            {
                int restEvent = totalEvent - batchIndex * batchSize;
                if(restEvent <= 0)
                    break;
                int takeEvent = restEvent > batchSize ? batchSize : restEvent;
                IEnumerable<FlagValueEvent> batchEvents = flagValueEvents.Skip(batchIndex * batchSize).Take(takeEvent);
                foreach (var evt in batchEvents)
                {
                    if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt)))))
                    {
                        var firstEvt = batchEvents.First();
                        throw new Exception($"Failed to Try Add FlagValueEvent to Batch. It's maybe too large for the batch. First Event Id {firstEvt.Id} @ {firstEvt.Timestamp}; {JsonSerializer.Serialize(firstEvt)}");
                    }
                }
                try
                {
                    await _producerClient.SendAsync(eventBatch);
                    lastSentTimeStamp = batchEvents.Last().Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                catch (Exception ex)
                {
                    var firstEvt = batchEvents.First();
                    throw new Exception($"Failed to Send Batch. First Event Id {firstEvt.Id} @ {firstEvt.Timestamp}; {JsonSerializer.Serialize(firstEvt)}", ex);
                }
                finally
                {
                    await _producerClient.DisposeAsync();
                }
                batchIndex++;
            }

            return (true, lastSentTimeStamp);

        }

        public async Task Reader()
        {
            // Create a blob container client that the event processor will use 
            BlobContainerClient storageClient = new BlobContainerClient(
                "DefaultEndpointsProtocol=https;AccountName=featbit;AccountKey=VbnyNGCVuqsWHSDrOtbcWO2N2waCXdsgW0EH3giadrEhlfExiLQpRr5PdLYZORQ2jzv/IKQ2ZxaY+AStNK/krQ==;EndpointSuffix=core.windows.net", "flagvalueevents");

            var processor = new EventProcessorClient(
                storageClient,
                EventHubConsumerClient.DefaultConsumerGroupName,
                _eventhubConnectionString,
                "flagvaluecapture");

            processor.ProcessEventAsync += ProcessEventHandler;
            processor.ProcessErrorAsync += ProcessErrorHandler;

            await processor.StartProcessingAsync();

            await Task.Delay(TimeSpan.FromSeconds(30));

            // Stop the processing
            await processor.StopProcessingAsync();
        }

        public Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            // Write the body of the event to the console window
            Console.WriteLine("\tReceived event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
            Console.ReadLine();
            return Task.CompletedTask;
        }

        public Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            // Write details about the error to the console window
            Console.WriteLine($"\tPartition '{eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
            Console.WriteLine(eventArgs.Exception.Message);
            Console.ReadLine();
            return Task.CompletedTask;
        }
    }
}
