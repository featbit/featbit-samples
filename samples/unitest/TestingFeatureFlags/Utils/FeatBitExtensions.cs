using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using System.Reflection.Metadata.Ecma335;

namespace TestingFeatureFlags.Utils
{
    public static class FeatBitExtensions
    {
        public static string FlagValue(this IFbClient fb, string featureFlagKey, string defaultValue)
        {
            return fb.StringVariation(featureFlagKey, FbUser.Builder($"{featureFlagKey}-singleton").Build(), defaultValue);
        }

        public static bool FlagValue(this IFbClient fb, string featureFlagKey, bool defaultValue)
        {
            return fb.BoolVariation(featureFlagKey, FbUser.Builder($"{featureFlagKey}-singleton").Build(), defaultValue);
        }
    }

    public class FeatBitDbMigration
    {
        public async Task RunParallelAndCompare(Task oldTask, Task newTask, Func<bool> compareFunc, CancellationToken token)
        {
            var result = await Task.WhenAll(oldTask, newTask).WaitAsync(TimeSpan.FromSeconds(2), token);
        } 
    }
}
