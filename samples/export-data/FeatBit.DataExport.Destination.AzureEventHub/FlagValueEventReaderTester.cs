using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Blobs;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;


namespace FeatBit.DataExport.Destination.AzureEventHub
{
    public class FlagValueEventReaderTester
    {
        private readonly string _eventhubConnectionString;
        private readonly EventProcessorClient _processor;

        //https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/eventhub/Azure.Messaging.EventHubs.Processor/samples/Sample04_ProcessingEvents.md
        private const int _eventsBeforeCheckpoint = 25;
        private ConcurrentDictionary<string, int> _partitionEventCount = new ConcurrentDictionary<string, int>();

        public FlagValueEventReaderTester(string azureEventHubConnectionString)
        {
            _eventhubConnectionString = azureEventHubConnectionString;


            BlobContainerClient storageClient = new (
                "DefaultEndpointsProtocol=https;AccountName=featbit;AccountKey=VbnyNGCVuqsWHSDrOtbcWO2N2waCXdsgW0EH3giadrEhlfExiLQpRr5PdLYZORQ2jzv/IKQ2ZxaY+AStNK/krQ==;EndpointSuffix=core.windows.net", "flagvalueevents");
            _processor = new (storageClient,
                                EventHubConsumerClient.DefaultConsumerGroupName,
                                _eventhubConnectionString,
                                "flagvaluecapture");
        }

        public async Task ReadAsync()
        {
            _processor.ProcessEventAsync += ProcessEventHandler;
            _processor.ProcessErrorAsync += ProcessErrorHandler;

            await _processor.StartProcessingAsync();

            await Task.Delay(TimeSpan.FromSeconds(15));

            await StopProcessingAsync();
        }

        public async Task StopProcessingAsync()
        {
            await _processor.StopProcessingAsync();
        }

        public Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            var evt = JsonSerializer.Deserialize<FlagValueEvent>(Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
            Console.WriteLine($"Received event: {evt.Id} {evt.EventName} {evt.Username} {evt.Timestamp}");

            //await eventArgs.UpdateCheckpointAsync();

            //string partition = eventArgs.Partition.PartitionId;
            //int eventsSinceLastCheckpoint = _partitionEventCount.AddOrUpdate(
            //    key: partition,
            //    addValue: 1,
            //    updateValueFactory: (_, currentCount) => currentCount + 1);
            //if (eventsSinceLastCheckpoint >= _eventsBeforeCheckpoint)
            //{
            //    await eventArgs.UpdateCheckpointAsync();
            //    _partitionEventCount[partition] = 0;
            //}

            return Task.CompletedTask;
        }

        public Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine($"\tPartition '{eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
