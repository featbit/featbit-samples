// .\FeatBit.DataExport.exe -e "32f6be54-6796-44dd-bc82-f4824b0da13c" -t "2023-12-12 07:42:13.062" -c "Host=127.0.0.1;Protocol=http;Port=8123;Database=featbit" -g "WriteKey=NUc5S2JlSGRqc3ExOVN1eGhLQ29TdjJMVW1SamRHWm4=;Host=https://api.segment.io" -s 500 -i 100 -b 30 -a "Endpoint=sb://featbit.servicebus.windows.net/;SharedAccessKeyName=FeatBit;SharedAccessKey=nk6sEZuTmJN2/sSCTGHzy9KlHJpdKU05K+AEhP1AOYQ="

using FeatBit.DataExport;
using FeatBit.DataExport.ClickHouse;
using FeatBit.DataExport.Destination.AzureEventHub;
using FeatBit.DataExport.Destination.Segment;
using System.Text.Json;

var parameters = new ParamOptions()
{
    PageSize = 500,
    QueryInterval = 100
};
if (Util.ParseParameters(args, parameters))
{
    Console.WriteLine($"Env Id: {parameters.EnvId}; Timestamp: {parameters.TimeStamp}; Page Size: {parameters.PageSize}");
}
else
{
    Console.WriteLine($"Invalid Parameters");
    return;
}


AzureEventHubWriter azureEventHubWriter = new AzureEventHubWriter(parameters.AzureEventHubConnectionString);
await azureEventHubWriter.Write();
await azureEventHubWriter.Reader();

return;

SegmentWriter segmentService = CreateSegmentService(parameters.SegmentConnectionString);


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

        if(segmentService != null)
        {
            (bool isSuccess, parameters.TimeStamp, FlagValueEvent failedEvent) = 
                await segmentService.WriteFlagValueEventsBatchAsync(flagValueEvents, parameters.TimeStamp);
            if(!isSuccess)
            {
                Console.WriteLine($"Write FlagValue Events Failed! Timestamp: {parameters.TimeStamp}; JsonContent: {JsonSerializer.Serialize(failedEvent)}");
                break;
            }   
        }

        await Task.Delay(parameters.QueryInterval);
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