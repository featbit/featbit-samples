// See https://aka.ms/new-console-template for more information
using ClickHouse.Client.ADO;
using ClickHouse.Client.Utility;
using Dapper;
using Segment.Analytics;
using Segment.Serialization;


using var connection = new ClickHouseConnection("Host=127.0.0.1;Protocol=http;Port=8123;Database=featbit");
// ExecuteScalarAsync is an async extension which creates command and executes it
//var version = await connection.ExecuteScalarAsync("SELECT version()");
//var version = await connection.ExecuteScalarAsync("SELECT * FROM events WHERE  timestamp > '2023-12-12 07:42:13.062'");
//Console.WriteLine(version);


string sql = "SELECT uuid, env_id, event as event_name FROM events WHERE  timestamp > '2023-12-12 07:42:13.062'";
var functions = await connection.QueryAsync<EventObject>(sql);
Console.WriteLine(string.Join('\n', functions));

//var segmentWriteKey = "qvqLU5RiPcu1syq5IGSBGGQkacFhGcXQ";
//var configuration = new Configuration(segmentWriteKey,
//            flushAt: 10,
//            flushInterval: 12);
//var analytics = new Analytics(configuration);

//for (int i = 51; i < 65; i++)
//{
//    analytics.Identify("tester-id-" + i, new JsonObject { ["username"] = "tester-id-" + i });
//    analytics.Track("Feature Flag Evaluation", new JsonObject
//    {
//        ["flagId"] = "32f6be54-6796-44dd-bc82-f4824b0da13c-growthbook-test-001",
//        ["variationid"] = "4ebdf4f6-ed31-41bd-884e-1af37dc7c033",
//        ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss +0000"), // "2023-12-12 04:38:38.754 +0000"
//        ["properties"] = new JsonObject { ["envId"] = "32f6be54-6796-44dd-bc82-f4824b0da13c" },
//        ["userId"] = "tester-id-" + i,
//        ["username"] = "tester-id-" + i,
//        ["event"] = "Feature Flag Evaluation"
//    });
//    Console.WriteLine("Sent message " + i);
//}

Console.WriteLine("Read for finish");
Console.ReadLine();

public class HttpRequestToSegment
{
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://jsonplaceholder.typicode.com"),
    };
}

public class EventObject
{
    public Object uuid { get; set; }
    public string env_id { get; set; }
    public string event_name { get; set; }
}
