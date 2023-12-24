using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Text;
using System.Text.Json;


namespace FeatBit.DataExport.Destination.AzureEventHub
{
    public class AzureEventHubWriter
    {
        private readonly EventHubProducerClient _producerClient;
        private readonly string _eventhubConnectionString;
        private readonly string _eventHubPlan;
        private readonly string _eventHubName;
        public AzureEventHubWriter(string azureEventHubConnectionString, string eventHubPlan, string eventHubName)
        {
            _eventhubConnectionString = azureEventHubConnectionString;
            _eventHubPlan = eventHubPlan;
            _eventHubName = eventHubName;
            _producerClient = new EventHubProducerClient(
                azureEventHubConnectionString,
                eventHubName);
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

            Console.WriteLine("===============================================");
            Console.WriteLine("===============================================");
            Console.WriteLine($"Sending {flagValueEvents.Count} events To Azure Event Hub {_eventHubName}...");


            int batchIndex = 0, totalEvent = flagValueEvents.Count;
            while (true)
            {
                int restEvent = totalEvent - batchIndex * batchSize;
                if(restEvent <= 0)
                    break;
                int takeEvent = restEvent > batchSize ? batchSize : restEvent;

                using EventDataBatch eventBatch = await _producerClient.CreateBatchAsync();

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
                    eventBatch.Dispose();
                }
                batchIndex++;
            }

            Console.WriteLine($"{flagValueEvents.Count} events sents to Azure Event Hub...");
            Console.WriteLine("===============================================");
            Console.WriteLine("===============================================");

            return (true, lastSentTimeStamp);

        }

        public async Task<(bool ifAllSent, string lastSentTimeStamp)> WriteCustomEventEventsBatchAsync(
            List<CustomEvent> customEvents)
        {
            string lastSentTimeStamp = "";
            int batchSize = 100;
            switch (_eventHubPlan)
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

            Console.WriteLine("===============================================");
            Console.WriteLine("===============================================");
            Console.WriteLine($"Sending {customEvents.Count} events To Azure Event Hub {_eventHubName}...");


            int batchIndex = 0, totalEvent = customEvents.Count;
            while (true)
            {
                int restEvent = totalEvent - batchIndex * batchSize;
                if (restEvent <= 0)
                    break;
                int takeEvent = restEvent > batchSize ? batchSize : restEvent;

                using EventDataBatch eventBatch = await _producerClient.CreateBatchAsync();

                IEnumerable<CustomEvent> batchEvents = customEvents.Skip(batchIndex * batchSize).Take(takeEvent);
                foreach (var evt in batchEvents)
                {
                    if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt)))))
                    {
                        var firstEvt = batchEvents.First();
                        throw new Exception($"Failed to Try Add CustomEvent to Batch. It's maybe too large for the batch. First Event Id {firstEvt.Id} @ {firstEvt.Timestamp}; {JsonSerializer.Serialize(firstEvt)}");
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
                    eventBatch.Dispose();
                }
                batchIndex++;
            }

            Console.WriteLine($"{customEvents.Count} events sents to Azure Event Hub...");
            Console.WriteLine("===============================================");
            Console.WriteLine("===============================================");

            return (true, lastSentTimeStamp);

        }

        public async Task Dispose()
        {
            await _producerClient.DisposeAsync();
        }
    }
}
