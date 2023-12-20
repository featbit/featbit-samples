using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FeatBit.DataExport.Destination.Segment
{
    public class SegmentWriter
    {
        public static async Task<bool> WriteFlagValueEventAsync(
            FlagValueEvent evt, 
            string segmentWriteKey = "NUc5S2JlSGRqc3ExOVN1eGhLQ29TdjJMVW1SamRHWm4=",
            string segmentBaseUrl = "https://api.segment.io")
        {
            sharedClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", segmentWriteKey);

            if(await IdentifyAsync(evt))
            {
                return await TrackAsync(evt);
            }
            return false;
        }

        private static async Task<bool> IdentifyAsync(FlagValueEvent evt)
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(evt), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await sharedClient.PostAsync("/v1/identify", jsonContent);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
            return true;
        }

        private static async Task<bool> TrackAsync(FlagValueEvent evt)
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(new TrackModel(evt)), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await sharedClient.PostAsync("/v1/track", jsonContent);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
            return true;
        }

        private static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("https://api.segment.io"),
        };
    }
}
