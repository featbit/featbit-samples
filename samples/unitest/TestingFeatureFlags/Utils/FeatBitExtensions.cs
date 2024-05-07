using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace TestingFeatureFlags.Utils
{
    public static class FeatBitExtensions
    {
        public static bool FeatureEnabled(this IFbClient fb, string featureFlagKey, bool defaultValue = false)
        {
            return fb.BoolVariation(featureFlagKey, FbUser.Builder($"{featureFlagKey}-singleton").Build(), defaultValue);
        }
        
        public static FeatBitMigrationEnum MigrationState(this IFbClient fb, string featureFlagKey, string defaultValue = "")
        {
            var migStr = fb.StringVariation(featureFlagKey, FbUser.Builder($"{featureFlagKey}-singleton").Build(), defaultValue);
            return migStr switch
            {
                "ReadFromOldDbOnly" => FeatBitMigrationEnum.ReadFromOldDbOnly,
                "ReadFromOldAndNewDb" => FeatBitMigrationEnum.ReadFromOldAndNewDb,
                "ReadFromNewDbOnly" => FeatBitMigrationEnum.ReadFromNewDbOnly,
                _ => FeatBitMigrationEnum.ReadFromOldDbOnly
            };
        }
    }

    public class FbDbMigration<T>
    {
        public async static Task<T> MigrateAsync(Func<Task<T>> a1, Func<Task<T>> a2, IFbClient fbClient, string ffKey, Action<T, T> compare, int timeOut = 10000)
        {
            var migrationState = fbClient.MigrationState(ffKey);
            if (migrationState == FeatBitMigrationEnum.ReadFromOldDbOnly)
                return await Task.Run<T>(a1);
            else if (migrationState == FeatBitMigrationEnum.ReadFromNewDbOnly)
                return await Task.Run<T>(a2);
            else
            {
                using var cts = new CancellationTokenSource();
                var t1 = Task.Run<T>(a1);

                var t2 = Task.Run<T>(a2);
                t2.ContinueWith((state) =>
                {
                    cts.Cancel();
                });
                var tTimeout = Task<T>.Delay(timeOut, cts.Token).ContinueWith<T>((state) =>
                {
                    return default;
                });

                Task.Delay(100).Wait();
                
                var parallelTasks = await Task.WhenAll<T>(t1, Task.WhenAny<T>(t2, tTimeout).Result);

                compare(parallelTasks[0], parallelTasks[1]);

                return parallelTasks[0];
            }
        }

        public async static Task<T> RunParallelTasksAsync(Task<T> t1, Task<T> t2, FeatBitMigrationEnum migrationState, Action<T, T> compare, int timeOut = 800)
        {

            if (migrationState == FeatBitMigrationEnum.ReadFromOldDbOnly)
                return await t1;
            else if (migrationState == FeatBitMigrationEnum.ReadFromNewDbOnly)
                return await t2;
            else
            {
                var parallelTasks = Task.WhenAll<T>(t1, t2);
                return await Task.WhenAny(parallelTasks, Task.Delay(timeOut)).ContinueWith(t =>
                {
                    compare(parallelTasks.Result[0], parallelTasks.Result[1]);
                    return parallelTasks.Result[0];
                });
            }
        }
    }

    public enum FeatBitMigrationEnum
    {
        ReadFromOldDbOnly,
        ReadFromNewDbOnly,
        ReadFromOldAndNewDb
    }
}
