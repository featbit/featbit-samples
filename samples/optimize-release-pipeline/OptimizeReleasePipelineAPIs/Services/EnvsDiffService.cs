using Newtonsoft.Json;
using OptimizeReleasePipelineAPIs.Models;
using OptimizeReleasePipelineAPIs.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OptimizeReleasePipelineAPIs.Services
{
    public class EnvsDiffService : IEnvsDiffService
    {
        private readonly IConfiguration _configuration;
        public EnvsDiffService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<EnvDiffReportModel> FindKeyAndVariationDiffAsync(EnvDiffParam param)
        {
            var lastEnvFeatureFlags = await GetEnvFeatureFlagsAsync(param.LastEnvironmentId, param.AccessToken);
            var currentEnvFeatureFlags = await GetEnvFeatureFlagsAsync(param.CurrentEnvironmentId, param.AccessToken);

            var report = new EnvDiffReportModel() { Differences = [] };

            foreach (var lastFlag in lastEnvFeatureFlags)
            {
                var currentFlag = currentEnvFeatureFlags.FirstOrDefault(f => f.Key == lastFlag.Key);
                if (currentFlag == null)
                {
                    report.Differences.Add($"Flag '{lastFlag.Key}' is missing in the current environment.");
                }
                else
                {
                    var ave = AreVariationsEqual(lastFlag.Variations, currentFlag.Variations);
                    if (ave.Item1 == false)
                        report.Differences.Add($"Flag '{lastFlag.Key}': {ave.Item2}");
                }
            }

            return report;
        }

        private Tuple<bool, string> AreVariationsEqual(ICollection<Variation> variations1, ICollection<Variation> variations2)
        {
            if (variations1.Count != variations2.Count)
            {
                return new Tuple<bool, string>(false, $"Last Env has {variations1.Count} variations, Current Env has {variations2.Count} variations");
            }

            // Create a HashSet to store the values of variations in variations2
            var variations2Values = new HashSet<string>(variations2.Select(v => v.Value));

            // Check if all variations in variations1 exist in variations2
            foreach (var variation1 in variations1)
            {
                if (!variations2Values.Contains(variation1.Value))
                {
                    return new Tuple<bool, string>(false, $"variation {variation1.Value} doesn't exist in current environment");
                }
            }
            return new Tuple<bool, string>(true, "");
        }

        private async Task<List<FeatureFlagSimple>> GetEnvFeatureFlagsAsync(string envId, string accessToken)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"{_configuration.GetValue<string>("FeatBitAPIsUrl")}/v1/envs/{envId}/feature-flags");
                request.Headers.Add("Authorization", accessToken);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var contentStr = await response.Content.ReadAsStringAsync();
                var content = JsonConvert.DeserializeObject<ApiResponse<PagedResult<FeatureFlagSimple>>>(contentStr);
                return content.Data.Items.ToList();
            }
        }
    }
}
