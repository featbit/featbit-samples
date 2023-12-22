using FeatBit.DataExport;
using FeatBit.DataExport.ClickHouse;
using FeatBit.DataExport.Destination.AzureEventHub;
using FeatBit.DataExport.Destination.Segment;

var parameters = new ParamOptions()
{
    PageSize = 500,
    QueryInterval = 100
};
if (Util.ParseParameters(args, parameters))
{
    Console.WriteLine($"Env Id: " +
                      $"{parameters.EnvId}; " +
                      $"Timestamp: {parameters.TimeStamp}; " +
                      $"Page Size: {parameters.PageSize}");
}
else
{
    Console.WriteLine($"Invalid Parameters");
    return;
}


SegmentWriter segmentService = CreateSegmentService(parameters.SegmentConnectionString);
AzureEventHubWriter azEvtHubService = CreateAzureEventHubService(parameters.AzureEventHubConnectionString, parameters.AzureEventHubPlan);


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

        string timeStamp = "";

        if (azEvtHubService != null)
        {
            (bool isSuccess, timeStamp) = await azEvtHubService.WriteFlagValueEventsBatchAsync(flagValueEvents);

            // test propose only
            await (new ReaderTester(parameters.AzureEventHubConnectionString, parameters.AzureEventHubPlan)).Reader();
        }

        if (segmentService != null)
        {
            (bool isSuccess, timeStamp) = await segmentService.WriteFlagValueEventsBatchAsync(flagValueEvents);
        }

        await Task.Delay(parameters.QueryInterval);

        parameters.TimeStamp = timeStamp;
    }
    else if(flagValueEvents.Count == 0)
    {
        Console.WriteLine($"No new items found;");
        await Task.Delay(parameters.BigInterval * 1000);
    }
}


SegmentWriter CreateSegmentService(string conn)
{
    if (conn != null && conn.Length > 20 && 
        conn.ToLower().Contains("writekey") &&
        conn.ToLower().Contains("host"))
    {
        return new SegmentWriter(conn);
    }
    return null;
}

AzureEventHubWriter CreateAzureEventHubService(string conn, string plan)
{
    if (!string.IsNullOrWhiteSpace(conn) && !string.IsNullOrWhiteSpace(plan))
    {
            return new AzureEventHubWriter(conn, plan);
    }
    return null;
}