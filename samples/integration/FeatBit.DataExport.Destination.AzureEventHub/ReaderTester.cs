using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Blobs;
using System.Text;
using System.Text.Json;


namespace FeatBit.DataExport.Destination.AzureEventHub
{
    public class ReaderTester
    {
        private readonly EventHubProducerClient _producerClient;
        private readonly string _eventhubConnectionString;
        private readonly string _eventHubPlan;
        private readonly EventProcessorClient _processor;
        public ReaderTester(string azureEventHubConnectionString, string eventHubPlan)
        {
            _eventhubConnectionString = azureEventHubConnectionString;
            _eventHubPlan = eventHubPlan;
            _producerClient = new EventHubProducerClient(
                azureEventHubConnectionString,
                "flagvaluecapture");


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

            //await Task.Delay(TimeSpan.FromSeconds(15));
        }

        public async Task StopProcessingAsync()
        {
            await _processor.StopProcessingAsync();
        }

        public Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            var evt = JsonSerializer.Deserialize<FlagValueEvent>(Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
            //Console.WriteLine($"Received event: {evt.Id} {evt.Username} {evt.Timestamp}");
            return Task.CompletedTask;
        }

        public Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            // Write details about the error to the console window
            Console.WriteLine($"\tPartition '{eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
