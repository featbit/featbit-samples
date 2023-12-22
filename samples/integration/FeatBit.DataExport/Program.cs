using FeatBit.DataExport;
using FeatBit.DataExport.ClickHouse;
using FeatBit.DataExport.Destination.AzureEventHub;
using FeatBit.DataExport.Destination.Segment;

var parameters = new ParamOptions();
if (Util.ParseParameters(args, parameters))
{
    SegmentWriter segmentService = CreateSegmentService(parameters.SegmentConnectionString);
    AzureEventHubWriter azEvtHubService = CreateAzureEventHubService(parameters.AzureEventHubConnectionString, parameters.AzureEventHubPlan, parameters.AzureEventHubName);

    if (parameters.EventType.ToLower() == "flagvalue")
    {
        await ExportFlagValueEventsAsync(parameters, azEvtHubService, segmentService);
    }
    else if (parameters.EventType.ToLower() == "customevent")
    {
    }
}
else
{
    Console.WriteLine($"Invalid Parameters");
}


Console.WriteLine($"Press any key to exit...");
Console.ReadKey();



async Task ExportFlagValueEventsAsync(ParamOptions parameters, AzureEventHubWriter azEvtHubService, SegmentWriter segmentService)
{
    // test propose only
    ReaderTester azEvtHubTester = new (parameters.AzureEventHubConnectionString, parameters.AzureEventHubPlan);
    await azEvtHubTester.ReadAsync();

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

            string timeStamp = "";

            if (azEvtHubService != null)
            {
                (bool isSuccess, timeStamp) = await azEvtHubService.WriteFlagValueEventsBatchAsync(flagValueEvents);
            }

            if (segmentService != null)
            {
                (bool isSuccess, timeStamp) = await segmentService.WriteFlagValueEventsBatchAsync(flagValueEvents);
            }

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

AzureEventHubWriter CreateAzureEventHubService(string conn, string plan, string evtHubName)
{
    if (!string.IsNullOrWhiteSpace(conn) && !string.IsNullOrWhiteSpace(plan) && !string.IsNullOrWhiteSpace(evtHubName))
    {
        return new AzureEventHubWriter(conn, plan, evtHubName);
    }
    return null;
}