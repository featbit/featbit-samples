using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FeatBit.MFMProvider
{
    public class FeatBitFeatureDefinitionProvider : IFeatureDefinitionProvider
    {
        private readonly Dictionary<string, FeatureDefinition> _features;

        public FeatBitFeatureDefinitionProvider()
        {

            _features = new Dictionary<string, FeatureDefinition>
            {
                {
                    "TrueFeatureFlag",
                    TrueFeatureFlag()
                },
                {
                    "FalseFeatureFlag",
                    FalseFeatureFlag()
                },
                {
                    "PercentagePassFeatureFlag",
                    PercentagePassFeatureFlag()
                },
                {
                    "TimeWindowFeatureFlag",
                    TimeWindowFeatureFlag()
                },
                {
                    "TargetingFeatureFlag",
                    TargetingFeatureFlag()
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

        private FeatureDefinition TrueFeatureFlag()
        {
            var enabledDic = new Dictionary<string, string>
            {
                {"Value", "100"}
            };
            var enabledConfigBuilder = new ConfigurationBuilder();
            enabledConfigBuilder.AddInMemoryCollection(enabledDic);
            return new FeatureDefinition
            {
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
            };
        }

        private FeatureDefinition FalseFeatureFlag()
        {
            var disEnbledDic = new Dictionary<string, string>
            {
                {"Value", "0"}
            };
            var disEnabledConfigBuilder = new ConfigurationBuilder();
            disEnabledConfigBuilder.AddInMemoryCollection(disEnbledDic);
            return new FeatureDefinition
            {
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
            };
        }

        private FeatureDefinition PercentagePassFeatureFlag()
        {
            var percentageDic = new Dictionary<string, string>
            {
                {"Value", "50"}
            };
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(percentageDic);
            return new FeatureDefinition
            {
                Name = "PercentagePassFeatureFlag",
                EnabledFor = new List<FeatureFilterConfiguration>()
                        {
                            new FeatureFilterConfiguration()
                            {
                                 Name = "Microsoft.Percentage",
                                 Parameters = configBuilder.Build()
                            }
                        },
                RequirementType = RequirementType.All
            };
        }

        private FeatureDefinition TimeWindowFeatureFlag()
        {
            var dic = new Dictionary<string, string>
            {
                {"Start", "2024-05-23T21:00:00"},
                {"End", "2024-05-24T21:00:00"},
                {"Recurrence", "{\"Pattern\": {" +
                                               "\"Type\": \"Daily\"," +
                                               "\"Interval\": 1" +
                                             "}," +
                                "\"Range\": {" +
                                             "\"Type\": \"NoEnd\"" +
                                           "}" +
                                "}"
                }
            };
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(dic);
            return new FeatureDefinition
            {
                Name = "TimeWindowFeatureFlag",
                EnabledFor = new List<FeatureFilterConfiguration>()
                        {
                            new FeatureFilterConfiguration()
                            {
                                 Name = "Microsoft.TimeWindow",
                                 Parameters = configBuilder.Build()
                            }
                        },
                RequirementType = RequirementType.All
            };
        }

        private FeatureDefinition TargetingFeatureFlag()
        {
            var json2 = "{\"Audience\":{\"Users\":[\"Jeff\",\"Alicia\"],\"Groups\":[{\"Name\":\"Ring0\",\"RolloutPercentage\":100},{\"Name\":\"Ring1\",\"RolloutPercentage\":50}],\"DefaultRolloutPercentage\":20,\"Exclusion\":{\"Users\":[\"Ross\"],\"Groups\":[\"Ring2\"]}}}";
            var configuration = new ConfigurationBuilder().AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(json2))).Build();
            return new FeatureDefinition
            {
            
                Name = "TargetingFeatureFlag",
                EnabledFor = new List<FeatureFilterConfiguration>()
                        {
                            //config
                            new FeatureFilterConfiguration()
                            {
                                 Name = "Microsoft.Targeting",
                                 Parameters = configuration
                            }
                        },
                RequirementType = RequirementType.All
            };


        }
    }
}