using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;

namespace BlazorAppDemo.FeatureFlags
{
    public static class FeatBitExtension
    {
        public static bool FeatureReleased(this IFbClient fbClient, string flagKey, string userId = null)
        {
            FbUser fbUser = FbUser.Builder(userId ?? Guid.NewGuid().ToString()).Build();
            return fbClient.BoolVariation(flagKey, fbUser, false);
        }

        public static bool PageAllowAccess(this IFbClient fbClient, string path, string userId = null)
        {
            FbUser fbUser = FbUser.Builder(userId ?? Guid.NewGuid().ToString()).Build();
            return fbClient.BoolVariation("route-navigation", fbUser, false);
        }
    }
}
