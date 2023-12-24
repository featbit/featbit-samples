// -e "32f6be54-6796-44dd-bc82-f4824b0da13c" -t "2023-12-12 07:41:51.288" -c "Host=127.0.0.1;Protocol=http;Port=8123;Database=featbit" -g "WriteKey=NUc5S2JlS***FAKE***1eGhLQ29TdjJMVW1SamRHWm4=;Host=https://api.segment.io" -s 1000 -i 100 -b 5 --eventtype "CustomEvent" --pipeline "CustomEventToSegment"

// -e "32f6be54-6796-44dd-bc82-f4824b0da13c" -t "2023-12-12 07:42:13.288" -c "Host=127.0.0.1;Protocol=http;Port=8123;Database=featbit" -g "WriteKey=NUc5S2JlS***FAKE***1eGhLQ29TdjJMVW1SamRHWm4=;Host=https://api.segment.io" -s 1000 -i 100 -b 5 --eventtype "CustomEvent" --pipeline "FlagValueToSegment"

// -e "32f6be54-6796-44dd-bc82-f4824b0da13c" -t "2023-12-12 07:41:51.288" -c "Host=127.0.0.1;Protocol=http;Port=8123;Database=featbit" -s 1000 -i 100 -b 5 -a "Endpoint=sb://featbitexportdata.servicebus.windows.net/;SharedAccessKeyName=flagvalueevents;SharedAccessKey=Q0sXMCyF***FAKE***Yz/kUnGu1wAbjeRnB+AEhJIzHhU=" --azevthubplan "Basic" --azevthubname "flagvaluecapture" --eventtype "CustomEvent" --pipeline "CustomEventToAzureEventHub"

// -e "32f6be54-6796-44dd-bc82-f4824b0da13c" -t "2023-12-12 07:42:13.288" -c "Host=127.0.0.1;Protocol=http;Port=8123;Database=featbit" -s 1000 -i 100 -b 5 -a "Endpoint=sb://featbitexportdata.servicebus.windows.net/;SharedAccessKeyName=flagvalueevents;SharedAccessKey=Q0sXMCyF***FAKE***Yz/kUnGu1wAbjeRnB+AEhJIzHhU=" --azevthubplan "Basic" --azevthubname "flagvaluecapture" --eventtype "CustomEvent" --pipeline "FlagValueToAzureEventHub"


using FeatBit.DataExport;
using FeatBit.DataExport.Destination.AzureEventHub;
using FeatBit.DataExport.Destination.Segment;
using FeatBit.DataExport.Pipelines.AzureEventHub;
using FeatBit.DataExport.Pipelines.Segment;

var parameters = new ParamOptions();
if (Util.ParseParameters(args, parameters))
{
    switch(parameters.Pipeline)
    {
        case "FlagValueToSegment":
            SegmentWriter segmentService = CreateSegmentService(parameters.SegmentConnectionString);
            await FlatValueEventsToSegmentPipeline.ExportAsync(parameters, segmentService);
            break;
        case "CustomEventToSegment":
            segmentService = CreateSegmentService(parameters.SegmentConnectionString);
            await CustomEventsToSegmentPipeline.ExportAsync(parameters, segmentService);
            break;
        case "FlagValueToAzureEventHub":
            // test propose only
            //FlagValueEventReaderTester azEvtHubTester = new(parameters.AzureEventHubConnectionString);
            //await azEvtHubTester.ReadAsync();
            //return;

            AzureEventHubWriter azEvtHubService = CreateAzureEventHubService(parameters.AzureEventHubConnectionString, parameters.AzureEventHubPlan, parameters.AzureEventHubName);
            await FlatValueEventsToAzureEventHubPipeline.ExportAsync(parameters, azEvtHubService);
            break;
        case "CustomEventToAzureEventHub":
            //Test propose only, Please keep the code
            //CustomEventReaderTester azEvtHubTester = new(parameters.AzureEventHubConnectionString);
            //await azEvtHubTester.ReadAsync();
            //return;

            azEvtHubService = CreateAzureEventHubService(parameters.AzureEventHubConnectionString, parameters.AzureEventHubPlan, parameters.AzureEventHubName);
            await CustomEventsToAzureEventHubPipeline.ExportAsync(parameters, azEvtHubService);
            break;
        default:
            Console.WriteLine($"Invalid Pipeline");
            break;
    }
}
else
{
    Console.WriteLine($"Invalid Parameters");
}

Console.WriteLine($"Press any key to exit...");
Console.ReadKey();
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