using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlazorAppDemo.FeatureFlags
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class FeatureGateAttribute : ActionFilterAttribute
    {
        public string FlagKey;
        public string DefaultValue;
        public string[] PassedValues;
        public FeatureGateAttribute(string flagKey, string defaultValue, string[] passedValues)
        {
            FlagKey = flagKey;
            DefaultValue = defaultValue;
            PassedValues = passedValues;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IFbClient fm = context.HttpContext.RequestServices.GetRequiredService<IFbClient>();
            var userName = context.HttpContext.User.Identity?.Name;
            var fbUser = FbUser.Builder(userName ??= Guid.NewGuid().ToString()).Build();
            var variation = fm.StringVariation(FlagKey, fbUser, DefaultValue);
            if (PassedValues.Contains(variation))
            {
                await next.Invoke().ConfigureAwait(true);
            }
            else
            {
                context.Result = new NotFoundResult();
            }
        }
    }
}
