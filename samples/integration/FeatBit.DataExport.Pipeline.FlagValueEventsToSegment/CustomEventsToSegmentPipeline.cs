using FeatBit.DataExport.ClickHouse;
using FeatBit.DataExport.Destination.Segment;

namespace FeatBit.DataExport.Pipelines.Segment
{
    public class CustomEventsToSegmentPipeline
    {
        public async static Task ExportAsync(ParamOptions parameters, SegmentWriter segmentService)
        {
            long totalSentEvent = 0;
            while (true)
            {
                Console.WriteLine($"Retriving CustomEvent top " +
                                  $"{parameters.PageSize} events after " +
                                  $"timestamp {parameters.TimeStamp} from " +
                                  $"environment '{parameters.EnvId}'");

                var customEventEvents = await ClickHouseReader.RetrieveCustomEventEventsAsync(parameters);
                if (customEventEvents != null && customEventEvents.Count > 0)
                {
                    Console.WriteLine($"Retrived Item Count: {customEventEvents.Count}");

                    (bool isSuccess, string timeStamp) = await segmentService.WriteCustomEventEventsBatchAsync(customEventEvents);

                    await Task.Delay(parameters.QueryInterval);

                    parameters.TimeStamp = timeStamp;
                    totalSentEvent += customEventEvents.Count;
                }
                else if (customEventEvents.Count == 0)
                {
                    Console.WriteLine($"No new items found;");
                    await Task.Delay(parameters.BigInterval * 1000);
                }
                Console.WriteLine($"Total sent event count: {totalSentEvent}");
            }
        }
    }
}
