// See https://aka.ms/new-console-template for more information
using FeatBit.Sdk.Server.Model;
using FeatBit.Sdk.Server.Options;
using FeatBit.Sdk.Server;
using System.Net;

Console.WriteLine("Experimentation Test with FeatBit WeatherForecast");

FbClient client = NewFbClient();
const string flagKey = "abctest-flag";

Task converstionTask = Task.Run(async () =>
{
    await ABTestConversionTestAsync(flagKey, "tryitonline-click", client);
});

Task numericTask = Task.Run(async () =>
{
    await ABTestNumericTestAsync(flagKey, "temp-diff", client);
});

Task.WaitAll(converstionTask, numericTask);


async Task ABTestConversionTestAsync(string flagKey, string metricName, FbClient client)
{
    while (true)
    {
        var roundRandom = (new Random()).Next(10, 30);
        int c = 0, t1 = 0, t2 = 0;
        for (int i = 0; i < roundRandom; i++)
        {
            // create a user
            var userRandomGuid = Guid.NewGuid();
            var userKey = $"abnumerictest-user-{userRandomGuid}";
            var userName = $"abnumerictest-name-{userRandomGuid}";
            var user = FbUser.Builder(userKey).Name(userName).Build();

            var variation = client.StringVariation(flagKey, user, "c");

            if (variation == "controlled")
            {
                c++;
                var hitRandom = (new Random()).Next(0, 50);
                if (hitRandom % 3 == 0 || hitRandom % 5 == 0)
                {
                    client.Track(user, metricName);
                }
            }
            else if (variation == "treatment1")
            {
                t1++;
                var hitRandom = (new Random()).Next(0, 100);
                if (hitRandom % 3 == 0 || hitRandom % 4 == 0 || hitRandom % 5 == 0)
                {
                    client.Track(user, metricName);
                }
            }
            else if (variation == "treatment2")
            {
                t2++;
                var hitRandom = (new Random()).Next(0, 100);
                if (hitRandom % 3 == 0 || hitRandom % 4 == 0 || hitRandom % 5 == 0 || hitRandom % 6 == 0)
                {
                    client.Track(user, metricName);
                }
            }
        }

        Console.WriteLine($"Converstion C/T1/T2: {c}/{t1}/{t2}");
        await Task.Delay(1000);
    }
}

async Task ABTestNumericTestAsync(string flagKey, string metricName, FbClient client)
{
    while (true)
    {
        var roundRandom = (new Random()).Next(7, 20);
        int c = 0, t1 = 0, t2 = 0;
        for (int i = 0; i < roundRandom; i++)
        {
            // create a user
            var userRandomGuid = Guid.NewGuid();
            var userKey = $"abnumerictest-user-{userRandomGuid}";
            var userName = $"abnumerictest-name-{userRandomGuid}";
            var user = FbUser.Builder(userKey).Name(userName).Build();

            // evaluate a boolean flag for a given user with evaluation detail
            var versionToUse = client.StringVariation(flagKey, user, "controlled");

            if (versionToUse == "controlled")
            {
                c++;
                var hitRandom = (new Random()).Next(1, 3);
                for (int j = 0; j < hitRandom; j++)
                {
                    var errorRandom = (new Random()).Next(3, 40);
                    client.Track(user, metricName, errorRandom);
                }
            }
            else if (versionToUse == "treatment1")
            {
                t1++;
                var hitRandom = (new Random()).Next(1, 3);
                for (int j = 0; j < hitRandom; j++)
                {
                    var errorRandom = (new Random()).Next(2, 30);
                    client.Track(user, metricName, errorRandom);
                }
            }
            else if (versionToUse == "treatment2")
            {
                t2++;
                var hitRandom = (new Random()).Next(1, 3);
                for (int j = 0; j < hitRandom; j++)
                {
                    var errorRandom = (new Random()).Next(1, 20);
                    client.Track(user, metricName, errorRandom);
                }
            }
        }

        Console.WriteLine($"Numeric C/T1/T2: {c}/{t1}/{t2}");
        await Task.Delay(1000);
    }

    // close the client to ensure that all insights are sent out before the app exits
    // await client.CloseAsync();
}

static FbClient NewFbClient()
{
    // setup sdk options
    var options = new FbOptionsBuilder("AkWmSfu-2E2pv-XpUulQywPgtR38sEYUuiVu0QddaWlQ")
        .Event(new Uri("http://localhost:5100"))
        .Streaming(new Uri("ws://localhost:5100"))
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

    return client;
}