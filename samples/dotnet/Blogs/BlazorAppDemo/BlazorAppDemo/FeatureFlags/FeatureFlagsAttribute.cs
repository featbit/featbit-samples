using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace BlazorAppDemo.FeatureFlags
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.All, AllowMultiple = true)]
    public class FeatureFlagsAttribute : ActionFilterAttribute, IAsyncPageFilter
    {
        public string[] Keys;
        public FeatureFlagsAttribute(params string[] keys)
        {
            Keys = keys;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IFbClient fm = context.HttpContext.RequestServices.GetRequiredService<IFbClient>();
            var user = context.HttpContext.User.Identity?.Name;
            var fbUser = FbUser.Builder(user ??= Guid.NewGuid().ToString()).Build();
            var enabled = fm.BoolVariation("weather-page", fbUser, false);
            if (enabled)
            {
                await next.Invoke().ConfigureAwait(true);
            }
            else
            {
                context.Result = new NotFoundResult();
            }
        }

        public Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            throw new NotImplementedException();
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            throw new NotImplementedException();
        }
    }
}
