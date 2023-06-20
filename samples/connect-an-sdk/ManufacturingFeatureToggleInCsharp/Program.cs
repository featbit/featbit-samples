
using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using FeatBit.Sdk.Server.Options;

// setup sdk options
var options = new FbOptionsBuilder("9m4Rle1LuUW-c4jlOxWqnQfR9fs96zF0y4GkCxWN9lNg")
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
    Console.WriteLine($"Choose Demo: '1' for progressive rollout, '2' for feature control");
    var cmd = Console.ReadLine();

    if (cmd == "1")
        (new Functions()).ProgressiveRollout("first-feature-flag", client);
    else if (cmd == "2")
        (new Functions()).ShowSpecificCustomers(client);
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
        Console.WriteLine($"first-feature-flag for customer A: {jsonVariation}");
    }
}