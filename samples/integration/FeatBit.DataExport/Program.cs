// -e "32f6be54-6796-44dd-bc82-f4824b0da13c" -t "2023-12-12 07:41:51.288" -c "Host=127.0.0.1;Protocol=http;Port=8123;Database=featbit" -g "WriteKey=NUc5S2JlSGRqc3ExOVN1eGhLQ29TdjJMVW1SamRHWm4=;Host=https://api.segment.io" -s 1000 -i 100 -b 5 -a "Endpoint=sb://featbitexportdata.servicebus.windows.net/;SharedAccessKeyName=flagvalueevents;SharedAccessKey=Q0sXMCyF3TqUcYLUYz/kUnGu1wAbjeRnB+AEhJIzHhU=" --azevthubplan "Basic" --azevthubname "flagvaluecapture" --eventtype "CustomEvent"

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
        await ExportCustomEventEventsAsync(parameters, azEvtHubService, segmentService);
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
    //FlagValueEventReaderTester azEvtHubTester = new (parameters.AzureEventHubConnectionString);
    //await azEvtHubTester.ReadAsync();
    //return;

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

async Task ExportCustomEventEventsAsync(ParamOptions parameters, AzureEventHubWriter azEvtHubService, SegmentWriter segmentService)
{
    //Test propose only, Please keep the code
    //CustomEventReaderTester azEvtHubTester = new(parameters.AzureEventHubConnectionString);
    //await azEvtHubTester.ReadAsync();
    //return;

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

            string timeStamp = "";

            if (azEvtHubService != null)
            {
                (bool isSuccess, timeStamp) = await azEvtHubService.WriteCustomEventEventsBatchAsync(customEventEvents);
            }

            await Task.Delay(30000);

            if (segmentService != null)
            {
                (bool isSuccess, timeStamp) = await segmentService.WriteCustomEventEventsBatchAsync(customEventEvents);
            }

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

    // test propose only
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