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
    public class ReaderTester
    {
        private readonly EventHubProducerClient _producerClient;
        private readonly string _eventhubConnectionString;
        private readonly string _eventHubPlan;
        public ReaderTester(string azureEventHubConnectionString, string eventHubPlan)
        {
            _eventhubConnectionString = azureEventHubConnectionString;
            _eventHubPlan = eventHubPlan;
            _producerClient = new EventHubProducerClient(
                azureEventHubConnectionString,
                "flagvaluecapture");
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
