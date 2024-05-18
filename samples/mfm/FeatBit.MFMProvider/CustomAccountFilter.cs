using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FeatBit.MFMProvider
{
    public interface IAccountContext
    {
        string AccountId { get; set; }
        string[] Groups { get; set; }
    }


    [FilterAlias("CustomAccount")]
    public class CustomAccountFilter : IContextualFeatureFilter<IAccountContext>
    {
        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext featureFilterContext, IAccountContext appContext)
        {
            throw new NotImplementedException();
        }
    }
}
