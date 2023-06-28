using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace EventSimulatorWithCPPLib
{
    public static class FTWrapper
    {
        const string ftSDKPath = "C:\\Code\\featbit\\debug\\vcpkg\\installed\\x64-windows\\bin\\ft-sdk.dll";
        [DllImport(ftSDKPath)]
        public static extern void Install(string jsonConfig);

        [DllImport(ftSDKPath)]
        public static extern void InitRUMConfig(string jsonConfig);

        [DllImport(ftSDKPath)]
        public static extern void InitLogConfig(string jsonConfig);

        [DllImport(ftSDKPath)]
        public static extern void InitTraceConfig(string jsonConfig);


        [DllImport(ftSDKPath)]
        public static extern void BindUserData(string jsonConfig);

        public static void IdentifyUser()
        {
            FTWrapper.BindUserData("{}");
        }

        [DllImport(ftSDKPath)]
        public static extern void UnbindUserdata();

        [DllImport(ftSDKPath)]
        public static extern void AddAction(string actionName, string actionType, long duration, long startTime);

        [DllImport(ftSDKPath)]
        public static extern void StartAction(string actionName, string actionType);

        [DllImport(ftSDKPath)]
        public static extern void StartView(string actionName);

        [DllImport(ftSDKPath)]
        public static extern void StopView();

        [DllImport(ftSDKPath)]
        public static extern void AddError(string log, string message, string errorType, string state);

        [DllImport(ftSDKPath)]
        public static extern void AddLongTask(string log, long duration);

        [DllImport(ftSDKPath)]
        public static extern void StartResource(string resourceId);

        [DllImport(ftSDKPath)]
        public static extern void StopResource(string resourceId);

        [DllImport(ftSDKPath)]
        public static extern void AddResource(string resourceId, string resourceParams, string netStauts);

        [DllImport(ftSDKPath)]
        public static extern void AddLog(string log, string message);

        [DllImport(ftSDKPath)]
        public static extern IntPtr GetTraceHeader(string resourceId, string url);

        [DllImport(ftSDKPath)]
        public static extern IntPtr GetTraceHeaderWithUrl(string url);

        [DllImport(ftSDKPath)]
        public static extern void DeInit();
    }

    public class SportsService
    {
        private readonly SportRepository _sportRepo;

        public SportsService(SportRepository sportRepo) 
        {
            _sportRepo = sportRepo;
        }

        public async Task<Sport> GetSportsByCityAsync(string cityId, int pageIndex, int pageSize)
        {
            return new Sport();
        }
    }




    /// <summary>
    /// https://leaware.com/asp-net-core-many-contexts-many-databases/
    /// </summary>
    public class SportRepository
    {
        private readonly SelfHostDatabaseDbContext _oldDbContext;
        private readonly CloudDatabaseDbContext _newDbContext;

        private readonly FeatBitService _fbService;

        public SportRepository(DbContexts dbContexts, FeatBitService fbService)
        {
            _oldDbContext = dbContexts["self-hosted-database"] as SelfHostDatabaseDbContext;
            _newDbContext = dbContexts["cloud-database"] as CloudDatabaseDbContext;

            _fbService = fbService;
        }

        public async Task<List<Sport>> GetSportsByCityAsync(int cityId, int pageIndex, int pageSize)
        {
            var tasks = new List<Task<List<Sport>>>();

            // 当读取Sport相关业务的旧数据库开关返回 true 时，则添加读取任务到执行任务
            if (_fbService.BoolVariation("read-sport-from-olddb"))
            {
                tasks.Add(GetSportsByCityQueryAsync(_oldDbContext, cityId, pageIndex, pageSize));
            }
            // 当读取Sport相关业务的新数据库开关返回 true 时，则添加读取任务到执行任务
            if (_fbService.BoolVariation("read-sport-from-newdb"))
            {
                tasks.Add(GetSportsByCityQueryAsync(_newDbContext, cityId, pageIndex, pageSize));
            }

            // 同时执行两个读操作（为了避免新增数据读取增加请求时间），并将结果进行对比，并将结果返回
            // 如果结果不一致，则返回旧数据库读取结果，并进行记录
            return await _fbService.RunAndCompareDbTasksAsync(
                                            tasks,
                                            timeOutDelayForNewDB: 3000, // 设定新数据库的最长等待时间，避免不良体感
                                            (timeoutInfo) => { }, // 当新数据库调用超时，发信息至观测云
                                            (unMatchInfo) => { }, // 当返回结果不一致时，发信息至观测云
                                            (exception) => { } // 当出现异常时，发信息至观测云
                                        );
        }

        public async Task<List<Sport>> GetSportsByCityQueryAsync(DbContext dbContext, int cityId, int pageIndex, int pageSize)
        {
            var query = from s in dbContext.Sports
                        join cs in dbContext.CitySports on s.Id equals cs.Sportid
                        where cs.CityId == cityId
                        select s;
            return await Task.Run(() => query.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }
    }

    public class GuanceSDK
    {
        public void RecordLongTask(object obj) { }
        public void RecordError(object obj) { }
        public void RecordException(object obj) { }
    }

    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CitySport
    {
        public int CityId { get; set; }
        public int Sportid { get; set; }
    }

    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DbContexts
    {
        private readonly Dictionary<string, DbContext> _dbContexts = new Dictionary<string, DbContext> ();

        public DbContext this[string index] => _dbContexts[index];
    }

    public class DbContext
    {
        public List<Sport> Sports { get; set; }
        public List<City> Cities { get; set; }
        public List<CitySport> CitySports { get; set; }
    }

    public class SelfHostDatabaseDbContext: DbContext
    {

    }

    public class CloudDatabaseDbContext: DbContext
    {

    }


    public class FeatBitService
    {
        public async Task<T> RunAndCompareDbTasksAsync<T>(List<Task<T>> dbTasks, int timeOutDelayForNewDB = 3000, 
            Action<string> longTaskAction = null, Action<string> unmatchResultAction = null, Action<Exception> exceptionAction = null)
        {
            //var timeoutTask = Task.Delay(timeOutDelayInMs).ContinueWith(_ => default(T));
            //var taskResults = await Task.WhenAll(dbTasks.Select(task => Task.WhenAny(task, timeoutTask)));

            var taskResults = await Task.WhenAll(dbTasks);

            return CompareAndReturnFinalResult(taskResults.ToList());
        }

        public bool BoolVariation(string flagKey, string defaultValue = "")
        {
            return true;
        }

        public bool CompareDbTasksReturnValue<T>(T values)
        {
            return true;
        }

        public T CompareAndReturnFinalResult<T>(List<T> values, Action action = null)
        {
            if (action != null)
                action();
            return values[0];
        }

        public async Task<T> CompareAndReturnFinalResultAsync<T>(List<T> values, Task<Action> actionAsync)
        {
            await actionAsync.WaitAsync(new TimeSpan(1000));
            return values[0];
        }
    }
}
