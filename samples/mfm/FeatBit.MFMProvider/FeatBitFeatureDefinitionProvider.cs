using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatBit.MFMProvider
{
    public class FeatBitFeatureDefinitionProvider : IFeatureDefinitionProvider
    {
        private readonly Dictionary<string, FeatureDefinition> _features;

        public FeatBitFeatureDefinitionProvider()
        {
            var enabledDic = new Dictionary<string, string>
            {
                {"Value", "100"}
            };
            var enabledConfigBuilder = new ConfigurationBuilder();
            enabledConfigBuilder.AddInMemoryCollection(enabledDic);


            var disEnbledDic = new Dictionary<string, string>
            {
                {"Value", "0"}
            };
            var disEnabledConfigBuilder = new ConfigurationBuilder();
            disEnabledConfigBuilder.AddInMemoryCollection(disEnbledDic);

            _features = new Dictionary<string, FeatureDefinition>
            {
                { 
                    "TrueFeatureFlag", 
                    new FeatureDefinition {
                        Name = "TrueFeatureFlag",
                        EnabledFor = new List<FeatureFilterConfiguration>()
                        {
                            new FeatureFilterConfiguration()
                            {
                                 Name = "Microsoft.Percentage",
                                 Parameters = enabledConfigBuilder.Build()
                            }
                        },
                        RequirementType = RequirementType.Any
                    }
                },
                { 
                    "FalseFeatureFlag", 
                    new FeatureDefinition { 
                        Name = "FalseFeatureFlag",
                        EnabledFor = new List<FeatureFilterConfiguration>()
                        {
                            new FeatureFilterConfiguration()
                            {
                                 Name = "Microsoft.Percentage",
                                 Parameters = disEnabledConfigBuilder.Build()
                            }
                        },
                        RequirementType = RequirementType.Any
                    } 
                }
            };
        }
        public async IAsyncEnumerable<FeatureDefinition> GetAllFeatureDefinitionsAsync()
        {
            foreach (var feature in _features.Values)
            {
                yield return feature;
                await Task.Yield(); // This line ensures the method yields control back to the caller, simulating asynchronous operation.
            }
        }

        public async Task<FeatureDefinition> GetFeatureDefinitionAsync(string featureName)
        {
            _features.TryGetValue(featureName, out var feature);
            return await Task.FromResult(feature);
        }
    }
}