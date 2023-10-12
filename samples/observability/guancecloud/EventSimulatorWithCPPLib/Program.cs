// See https://aka.ms/new-console-template for more information
using EventSimulatorWithCPPLib;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

string dataKitUrl = "http://localhost:9529";
string appId = "test_web_app";

int sessionNumber = 1024;
sessionNumber = 24;

int ii = 0;
do
{

    using var client = new HttpClient();

    //if(ii < 1500)
    //{
    //    var result = await client.PostAsync($"http://localhost:5260/api/Sports/GetSportsByCityWithoutBug", null);
    //    Task.Delay((new Random()).Next(20, 200)).Wait();
    //}
    //else
    //{
        var result = await client.PostAsync($"http://localhost:5260/api/Sports/GetSportsByCity", null);
        Task.Delay((new Random()).Next(50, 600)).Wait();
    //}


    //Console.WriteLine($"{ii.ToString()}");

    //ii++;
    //if (ii > 2000)
        break;
}
while (true);

return;

for (int i = 0; i < sessionNumber; i++)
{

    FTWrapper.Install("{\"serverUrl\":\"" + dataKitUrl + "\",\"envType\":\"production\",\"serviceName\":\"Your Services\",\"globalContext\":{\"appName\":\"Feature Flags Demo\"}}");
    FTWrapper.InitRUMConfig("{\"appId\":\"" + appId + "\",\"sampleRate\":0.8,\"globalContext\":{\"rum_custom\":\"rum custom value\"}}");
    FTWrapper.InitLogConfig("{\"sampleRate\":0.9,\"enableCustomLog\":true,\"enableLinkRumData\":true,\"globalContext\":{\"log_custom\":\"log custom value\"}}");
    FTWrapper.InitTraceConfig("{\"sampleRate\":0.9,\"traceType\":\"ddtrace\",\"enableLinkRumData\":true}");

    string userId = "user-id-" + Guid.NewGuid().ToString(), userName = "user-id-" + Guid.NewGuid().ToString();
    FTWrapper.BindUserData("{\"userId\": \"" + userId + "\",\"userName\": \"" + userName + "\",\"userEmail\":\"\",\"extra\": {\"custom_data\": \"custom data\"}}");

    FTWrapper.StartView($"Test View {Guid.NewGuid().ToString()}");

    FTWrapper.AddLog("test log", "test message");

    FTWrapper.StartAction("click", "test");

    FTWrapper.AddLongTask("long task test", 100002);

    FTWrapper.AddError("error", "error msg", "", "native_crash");

    string resourceId = Guid.NewGuid().ToString();

    var apiURL = "https://apis.sportamat.com/sports/get?cityId=201582&pageIndex=3&pageSize=15";

    

    #region commented code
    FTWrapper.StartResource(resourceId);
  
    CallResource(
        "https://preprod-docs.cloudcare.cn/real-user-monitoring/cpp/app-access/#_17",
        new ResourceParams
        {
            Url = "https://preprod-docs.cloudcare.cn/real-user-monitoring/cpp/app-access/#_17",
            ResourceStatus = 200
        },
        new NetStatus
        {
            RequestHeader = $"key1=value1,key2=value2",
            ResponseHeader = "key1=value1, key2=value2",
            DnsStartTime = 0,
            DnsEndTime = 1,
            TcpStartTime = 0,
            TcpEndTime = 1,
            SslStartTime = 0,
            SslEndTime = 1,
            Ttfb = 1
        },
        Guid.NewGuid().ToString());

    CallResource(
        "https://docs.guance.com/real-user-monitoring/csharp/app-access/#_13",
        new ResourceParams
        {
            Url = "https://docs.guance.com/real-user-monitoring/csharp/app-access/#_13",
            ResourceStatus = 200
        },
        new NetStatus
        {
            RequestHeader = $"key1=value1,key2=value2",
            ResponseHeader = "key1=value1, key2=value2",
            DnsStartTime = 0,
            DnsEndTime = 1,
            TcpStartTime = 0,
            TcpEndTime = 1,
            SslStartTime = 0,
            SslEndTime = 1,
            Ttfb = 1
        },
        Guid.NewGuid().ToString());


    FTWrapper.StopResource(resourceId);

    IntPtr headData = FTWrapper.GetTraceHeader(resourceId, apiURL);
    string helloJsonString = Marshal.PtrToStringAnsi(headData);
    Console.WriteLine(helloJsonString);

    FTWrapper.AddResource(
        resourceId,
        JsonSerializer.Serialize(new ResourceParams
        {
            Url = apiURL,
            ResourceStatus = 200
        }),
        JsonSerializer.Serialize(new NetStatus
        {
            RequestHeader = $"key1=value1,key2=value2",
            ResponseHeader = "key1=value1, key2=value2",
            DnsStartTime = 0,
            DnsEndTime = 1,
            TcpStartTime = 0,
            TcpEndTime = 1,
            SslStartTime = 0,
            SslEndTime = 1,
            Ttfb = 1
        }));
    #endregion

    FTWrapper.StopView();

    //FTWrapper.UnbindUserdata();

    FTWrapper.DeInit();

    Task.Delay(300).Wait();
}


void CallResource(string apiURL, ResourceParams resourceParams, NetStatus netStatus, string resourceId)
{
    //string resourceId = Guid.NewGuid().ToString();

    FTWrapper.StartResource(resourceId);

    FTWrapper.StopResource(resourceId);

    FTWrapper.GetTraceHeader(resourceId, apiURL);

    FTWrapper.AddResource(
        resourceId,
        JsonSerializer.Serialize(resourceParams),
        JsonSerializer.Serialize(netStatus));
}




Console.ReadLine();

internal class ResourceParams
{
    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Url { get; set; }

    [JsonPropertyName("requestHeader")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string RequestHeader { get; set; }

    [JsonPropertyName("responseHeader")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ResponseHeader { get; set; }

    [JsonPropertyName("responseConnection")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ResponseConnection { get; set; }

    [JsonPropertyName("responseContentType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ResponseContentType { get; set; }

    [JsonPropertyName("responseContentEncoding")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ResponseContentEncoding { get; set; }

    [JsonPropertyName("resourceMethod")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ResourceMethod { get; set; }

    [JsonPropertyName("responseBody")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ResponseBody { get; set; }

    [JsonPropertyName("resourceStatus")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ResourceStatus { get; set; }


}

internal class NetStatus
{
    public NetStatus() { }


    [JsonPropertyName("requestHeader")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string RequestHeader { get; set; }

    [JsonPropertyName("responseHeader")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ResponseHeader { get; set; }

    [JsonPropertyName("dnsTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long DnsTime { get { return DnsEndTime - DnsStartTime; } }

    [JsonPropertyName("fetchStartTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string FetchStartTime { get; set; }

    [JsonPropertyName("tcpTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long TcpTime { get { return TcpEndTime - TcpStartTime; } }

    [JsonPropertyName("responseTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long ResponseTime { get { return ResponseEndTime - ResponseStartTime; } }

    [JsonPropertyName("sslTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long SslTime { get { return SslEndTime - SslStartTime; } }

    [JsonPropertyName("firstByteTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string FirstByteTime { get; set; }

    [JsonPropertyName("ttfb")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Ttfb { get; set; }

    [JsonPropertyName("tcpStartTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long TcpStartTime { get; set; }

    [JsonPropertyName("tcpEndTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long TcpEndTime { get; set; }

    [JsonPropertyName("dnsStartTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long DnsStartTime { get; set; }

    [JsonPropertyName("dnsEndTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long DnsEndTime { get; set; }

    [JsonPropertyName("responseStartTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long ResponseStartTime { get; set; }

    [JsonPropertyName("responseEndTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long ResponseEndTime { get; set; }

    [JsonPropertyName("sslStartTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long SslStartTime { get; set; }

    [JsonPropertyName("sslEndTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long SslEndTime { get; set; }
}