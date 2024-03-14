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

            var report = new EnvDiffReportModel() { Warnings = [], Errors = [] };

            foreach (var lastFlag in lastEnvFeatureFlags)
            {
                var currentFlag = currentEnvFeatureFlags.FirstOrDefault(f => f.Key == lastFlag.Key);
                if (currentFlag == null)
                {
                    report.Warnings.Add($"Flag '{lastFlag.Key}' is missing in the current environment.");
                }
                else if (!AreVariationsEqual(lastFlag.Variations, currentFlag.Variations))
                {
                    report.Errors.Add($"Variations for flag '{lastFlag.Key}' are different.");
                }
            }

            return report;
        }

        private bool AreVariationsEqual(ICollection<Variation> variations1, ICollection<Variation> variations2)
        {
            if (variations1.Count != variations2.Count)
            {
                return false;
            }

            // Create a HashSet to store the values of variations in variations2
            var variations2Values = new HashSet<string>(variations2.Select(v => v.Value));

            // Check if all variations in variations1 exist in variations2
            foreach (var variation1 in variations1)
            {
                if (!variations2Values.Contains(variation1.Value))
                {
                    return false;
                }
            }
            return true;
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
