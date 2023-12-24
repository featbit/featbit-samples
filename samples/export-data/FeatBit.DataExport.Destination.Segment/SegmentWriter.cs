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

        public async Task<(bool ifAllSent, string lastSentTimeStamp)> WriteCustomEventEventsBatchAsync(
            List<CustomEvent> customEventEvents)
        {
            string lastSentTimeStamp = "";
            Console.WriteLine($"Sending To Segment...");
            foreach (var evt in customEventEvents)
            {
                await IdentifyAsync(evt);
                await TrackAsync(evt);
                lastSentTimeStamp = evt.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            Console.WriteLine($"Sent Events To Segment.com! Item Count: {customEventEvents.Count}; Last TimeStamp: {lastSentTimeStamp}");
            return (true, lastSentTimeStamp);
        }

        public async Task<(bool ifAllSent, string lastSentTimeStamp)> WriteFlagValueEventsBatchAsync(
            List<FlagValueEvent> flagValueEvents)
        {
            string lastSentTimeStamp = "";
            Console.WriteLine($"Sending To Segment...");
            foreach (var evt in flagValueEvents)
            {
                await IdentifyAsync(evt);
                await TrackAsync(evt);
                lastSentTimeStamp = evt.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            Console.WriteLine($"Sent Events To Segment.com! Item Count: {flagValueEvents.Count}; Last TimeStamp: {lastSentTimeStamp}");
            return (true, lastSentTimeStamp);
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

        private async Task<bool> IdentifyAsync(CustomEvent evt)
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
            using StringContent jsonContent = new(JsonSerializer.Serialize(new FlagValueTrackModel(evt)), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _httpClient.PostAsync("/v1/track", jsonContent);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            //Console.WriteLine($"{jsonResponse}\n");
            return true;
        }

        private async Task<bool> TrackAsync(CustomEvent evt)
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(new CustomEventTrackModel(evt)), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _httpClient.PostAsync("/v1/track", jsonContent);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return true;
        }
    }
}
