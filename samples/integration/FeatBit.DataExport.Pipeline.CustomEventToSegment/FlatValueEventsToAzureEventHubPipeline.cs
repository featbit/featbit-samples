using FeatBit.DataExport.ClickHouse;
using FeatBit.DataExport.Destination.AzureEventHub;

namespace FeatBit.DataExport.Pipelines.Segment
{
    public class FlatValueEventsToAzureEventHubPipeline
    {
        public async static Task ExportAsync(ParamOptions parameters, AzureEventHubWriter segmentService)
        {
            long totalSentEvent = 0;
            while (true)
            {
                Console.WriteLine($"Retriving FlagValue top " +
                                  $"{parameters.PageSize} events after " +
                                  $"timestamp {parameters.TimeStamp} from " +
                                  $"environment '{parameters.EnvId}'");

                var flagValueEvents = await ClickHouseReader.RetrieveFlagValueEventsAsync(parameters);
                if (flagValueEvents != null && flagValueEvents.Count > 0)
                {
                    Console.WriteLine($"Retrived Item Count: {flagValueEvents.Count}");

                    (bool isSuccess, string timeStamp) = await segmentService.WriteFlagValueEventsBatchAsync(flagValueEvents);

                    await Task.Delay(parameters.QueryInterval);

                    parameters.TimeStamp = timeStamp;
                    totalSentEvent += flagValueEvents.Count;
                }
                else if (flagValueEvents.Count == 0)
                {
                    Console.WriteLine($"No new items found;");
                    await Task.Delay(parameters.BigInterval * 1000);
                }
                Console.WriteLine($"Total sent event count: {totalSentEvent}");
            }
        }
    }
}
