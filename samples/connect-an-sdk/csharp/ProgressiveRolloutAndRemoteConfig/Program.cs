
using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using FeatBit.Sdk.Server.Options;
using Microsoft.Extensions.Logging;

// setup sdk options
var options = new FbOptionsBuilder("gvkuIffZRkWoXWM-VumvsAJGumzSi8qUeo7MWeDXG0jQ")
    .Event(new Uri("http://localhost:5100"))
    .Steaming(new Uri("ws://localhost:5100"))
    .Build();

// creates a new client instance that connects to FeatBit with the custom option.
var client = new FbClient(options);
if (!client.Initialized)
{
Console.WriteLine(
    "FbClient failed to initialize. All Variation calls will use fallback value."
);
}
else
{
Console.WriteLine("FbClient successfully initialized!");
}


do
{
    Console.WriteLine($"Choose Demo: '1' for progressive rollout, '2' for feature control, '3' for algo release");
    var cmd = Console.ReadLine();

    if (cmd == "1")
        (new Functions()).ProgressiveRollout("percentage-rollout", client);
    else if (cmd == "2")
        (new Functions()).ShowSpecificCustomers(client);
    else if (cmd == "3")
        (new Functions()).ShowFixNet(client);

    
    Console.WriteLine($"Type q to quit, others to continue");

    cmd = Console.ReadLine();
    if (cmd != "q")
        continue;
    else
        break;

} while (true);

// close the client to ensure that all insights are sent out before the app exits
await client.CloseAsync();

public class Functions
{
    public void ProgressiveRollout(string flagKey, FbClient fbClient)
    {
        int trueUsers = 0, falseUsers = 0, totalUsers = 1000;
        for (int i = 0; i < totalUsers; i++)
        {
            var user = FbUser.Builder($"tester-id-{i}").Name($"tester-{i}").Build();

            var boolVariation = fbClient.BoolVariation(flagKey, user, defaultValue: false);
            //Console.WriteLine($"flag '{flagKey}' returns {boolVariation} for user {user.Key}");

            if (boolVariation == true)
                trueUsers++;
            else
                falseUsers++;
        }

        Console.WriteLine($"Total Users: {totalUsers}; True Users: {trueUsers}; False Users: {falseUsers}");

    }

    public void ShowSpecificCustomers(FbClient fbClient)
    {
        var user = FbUser.Builder($"fake-user-in-customer-a")
            .Name($"fake-user-in-customer-a")
            .Custom("customer", "Customer A")
            .Custom("customerDepartment", "Zhoushan Port").Build();

        var boolVariation = fbClient.BoolVariation("first-feature-flag", user, defaultValue: false);
        Console.WriteLine($"first-feature-flag for customer A: {boolVariation}");


        var jsonVariation = fbClient.StringVariation("feature-config", user, defaultValue: "{}");
        Console.WriteLine($"feature-config for customer A: {jsonVariation}");
    }

    /// <summary>
    /// In financial or bank companies, invest money to get more money is such important. 
    /// We usally improve algorithme consequentively to try and try and try.
    /// A new algorithm can be related to several other resources, they're interdependant.
    /// So how can we divides each module indenpendantly and release them smooth and safety
    /// 
    /// 1. 需要开关能力，将业务、部门与数据源之间松耦合掉。即我的部署和上线应该可以独立测试。注：尤其是一次部署涉及到多个独立的业务小逻辑时
    /// 2. 在我所需要的资源全部就位后，我可以随时的在生产环境中进行内部测试 - 无论发布前在测试环境、UAT怎么测，都是不足的
    /// 3. 基本上
    /// </summary>
    /// <param name="fbClient"></param>
    public void ShowFixNet(FbClient fbClient)
    {
        var user = FbUser.Builder($"qa-001")
            .Name($"王品清")
            .Custom("level", "Senior")
            .Custom("department", "QA").Build();

        var variation = fbClient.StringVariation("fixnet", user, defaultValue: "Close");
        if(variation == "Close")
        {
            Console.WriteLine("混合投资算法 - 线新回归算法，正在运行中...");
        }
        if (variation == "Beta")
        {
            Console.WriteLine("混合投资算法 - Fixnet Beta 版，正在运行中...");
        }
        if (variation == "v1.0.0")
        {
            Console.WriteLine("混合投资算法 - Fixnet V1.0.0 版，正在运行中...");
        }
    }
}



//using FeatBit.Sdk.Server;
//using FeatBit.Sdk.Server.Model;
//using FeatBit.Sdk.Server.Options;

//// setup sdk options
//var options = new FbOptionsBuilder("LfqPR4yAh0-xIuIRV1z9JABU-pa29kOkq73rwBzqNMRQ")
//    .Event(new Uri("http://60.204.203.18:8081"))
//    .Steaming(new Uri("ws://60.204.203.18:8081"))
//    .Build();

//// creates a new client instance that connects to FeatBit with the custom option.
//var client = new FbClient(options);
//if (!client.Initialized)
//{
//    Console.WriteLine(
//        "FbClient failed to initialize. All Variation calls will use fallback value."
//    );
//}
//else
//{
//    Console.WriteLine("FbClient successfully initialized!");
//}

//// flag to be evaluated
//const string flagKey = "game-runner";

//// create a user
//var user = FbUser.Builder("tester-id").Name("tester").Build();

//// evaluate a boolean flag for a given user
//var boolVariation = client.BoolVariation(flagKey, user, defaultValue: false);
//Console.WriteLine($"flag '{flagKey}' returns {boolVariation} for user {user.Key}");

//// evaluate a boolean flag for a given user with evaluation detail
//var boolVariationDetail = client.BoolVariationDetail(flagKey, user, defaultValue: false);
//Console.WriteLine(
//    $"flag '{flagKey}' returns {boolVariationDetail.Value} for user {user.Key}. " +
//    $"Reason Kind: {boolVariationDetail.Kind}, Reason Description: {boolVariationDetail.Reason}"
//);

//// close the client to ensure that all insights are sent out before the app exits
//await client.CloseAsync();
