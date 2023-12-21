using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FeatBit.DataExport.Destination.Segment
{
    public class SegmentWriter
    {
        private readonly HttpClient _httpClient;

        public SegmentWriter(string SegmentConnectionString)
        {
            (string baseAddress, string writeKey) = RetriveWriteKeyAndHost(SegmentConnectionString);

            _httpClient = new()
            {
                BaseAddress = new Uri(baseAddress),
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", writeKey);
        }

        private (string baseAddress, string writeKey) RetriveWriteKeyAndHost(string SegmentConnectionString)
        {
            string baseAddress = "", writeKey = "";
            var args = SegmentConnectionString.Split(';');
            args.ToList().ForEach(arg =>
            {
                var kIndex = arg.IndexOf('=');
                var kv = new string[] { arg.Substring(0, kIndex), arg.Substring(kIndex + 1) };
                if (kv[0].ToLower() == "writekey")
                {
                    writeKey = kv[1];
                }
                else if (kv[0].ToLower() == "host")
                {
                    baseAddress = kv[1];
                }
            });
            return (baseAddress, writeKey);
        }

        public async Task<(bool ifAllSent, string lastTimeStamp, FlagValueEvent failedEvent)> WriteFlagValueEventsBatchAsync(
            List<FlagValueEvent> flagValueEvents, string lastTimeStamp)
        {
            Console.WriteLine($"Sending To Segment...");
            FlagValueEvent failedEvent = null;
            foreach (var evt in flagValueEvents)
            {
                if (await WriteFlagValueEventAsync(evt))
                {
                    lastTimeStamp = evt.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                else
                {
                    failedEvent = evt;
                    throw new Exception($"Write FlagValue Event Failed! Timestamp: {evt.Timestamp}; UUId: {evt.Id}; JsonContent: {JsonSerializer.Serialize(evt)}");
                }
            }
            Console.WriteLine($"Sent Events To Segment.com! Item Count: {flagValueEvents.Count}; Last TimeStamp: {lastTimeStamp}");
            return (true, lastTimeStamp, failedEvent);
        }

        public async Task<bool> WriteFlagValueEventAsync(
            FlagValueEvent evt)
        {
            if(await IdentifyAsync(evt))
            {
                return await TrackAsync(evt);
            }
            return false;
        }

        private async Task<bool> IdentifyAsync(FlagValueEvent evt)
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(evt), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _httpClient.PostAsync("/v1/identify", jsonContent);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            //Console.WriteLine($"{jsonResponse}\n");
            return true;
        }

        private async Task<bool> TrackAsync(FlagValueEvent evt)
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(new TrackModel(evt)), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _httpClient.PostAsync("/v1/track", jsonContent);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            //Console.WriteLine($"{jsonResponse}\n");
            return true;
        }

    }
}
