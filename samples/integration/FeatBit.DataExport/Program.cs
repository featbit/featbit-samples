// .\FeatBit.DataExport.exe -e "32f6be54-6796-44dd-bc82-f4824b0da13c" -t "2023-12-12 07:42:13.062" -c "Host=127.0.0.1;Protocol=http;Port=8123;Database=featbit"

using FeatBit.DataExport;
using FeatBit.DataExport.ClickHouse;
using FeatBit.DataExport.Destination.Segment;
using System.Text.Json;

var parameters = new ParamOptions()
{
    PageSize = 15,
    ConnectionString = "Host=127.0.0.1;Protocol=http;Port=8123;Database=featbit",
    EnvId = "32f6be54-6796-44dd-bc82-f4824b0da13c",
    TimeStamp = "2023-12-12 07:42:13.062"
};
//if( Util.ParseParameters(args, parameters))
//{
//    Console.WriteLine($"Env Id: {parameters.EnvId}; Timestamp: {parameters.TimeStamp}; Page Size: {parameters.PageSize}");
//}
//else
//{
//    Console.WriteLine($"Invalid Parameters");
//    return;
//}

while (true)
{
    Console.WriteLine($"Retriving FlagValue top {parameters.PageSize} events after timestamp {parameters.TimeStamp} from environment '{parameters.EnvId}'");
    var flagValueEvents = await ClickHouseReader.RetrieveFlagValueEventsAsync(parameters);
    if (flagValueEvents != null && flagValueEvents.Count > 0)
    {
        foreach(var evt in flagValueEvents)
        {
            if(await SegmentWriter.WriteFlagValueEventAsync(evt))
            {
                parameters.TimeStamp = evt.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                continue;
            }
            else
            {
                throw new Exception($"Write FlagValue Event Failed! Timestamp: {evt.Timestamp}; UUId: {evt.Id}; JsonContent: {JsonSerializer.Serialize(evt)}");
            }
        }
        Console.WriteLine($"Retriving Completed! Retrived Item Count: {flagValueEvents.Count}");
    }
    else if(flagValueEvents.Count == 0)
    {
        Console.WriteLine("No New FlagValue Events, wait 30 seconds");
        Task.Delay(30000).Wait();
    }
}



