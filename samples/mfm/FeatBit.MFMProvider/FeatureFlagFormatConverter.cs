using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace FeatBit.MFMProvider
{
    internal class FeatureFlagFormatConverter
    {
        public FeatureDefinition Convert()
        {
            // We consider here only "Built-In Feature Filters"
            // Patterns:
            // isenabled: try EnabledFor = new List<FeatureFilterConfiguration>() 
            // isdisabled: try EnabledFor = new List<FeatureFilterConfiguration>() 

            var featureDefinition = new FeatureDefinition();

            featureDefinition.RequirementType = RequirementType.All;
            featureDefinition.Name = "FeatureFD001";
            featureDefinition.EnabledFor = new List<FeatureFilterConfiguration>()
            {
                new FeatureFilterConfiguration(){
                    Name = "",
                    Parameters = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
                        {
                            { "Start", "09:00" },
                            { "End", "17:00" }
                        }).Build()
                }
            };

            return null;
        }
    }
}
