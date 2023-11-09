// See https://aka.ms/new-console-template for more information
using FeatBit.Sdk.Server.Model;
using FeatBit.Sdk.Server.Options;
using FeatBit.Sdk.Server;

Console.WriteLine("Hello, World!");

//await WriteDataDogAPMSimulationAsync();

//const string flagKey = "abnumerictest";

//await ABTestConversionTestAsync(flagKey, "ab-test-conversion-test-hit");
//await ABTestNumericTestAsync(flagKey, "ab-test-numeric-test-2");

const string flagKey = "abctest";
await ABTestConversionTestAsync(flagKey, "evt-cnv", 3);
//await ABTestNumericTestAsync(flagKey, "evt-dig");


async Task ABTestConversionTestAsync(string flagKey, string metricName, int variations = 2)
{
    FbClient client = NewFbClient();

    while (true)
    {
        var roundRandom = (new Random()).Next(10, 30);
        int v2Round = 0, c = 0, t1 = 0, t2 = 0;
        for (int i = 0; i < roundRandom; i++)
        {
            // create a user
            var userRandomGuid = Guid.NewGuid();
            var userKey = $"abnumerictest-user-{userRandomGuid}";
            var userName = $"abnumerictest-name-{userRandomGuid}";
            var user = FbUser.Builder(userKey).Name(userName).Build();

            if (variations == 2)
            {
                // evaluate a boolean flag for a given user with evaluation detail
                var variation = client.BoolVariation(flagKey, user);

                if (variation == false)
                {
                    var hitRandom = (new Random()).Next(0, 50);
                    if (hitRandom % 3 == 0 || hitRandom % 5 == 0)
                    {
                        client.Track(user, metricName);
                    }
                }
                else
                {
                    v2Round++;
                    var hitRandom = (new Random()).Next(0, 100);
                    if (hitRandom % 3 == 0 || hitRandom % 4 == 0 || hitRandom % 5 == 0 || hitRandom % 6 == 0)
                    {
                        client.Track(user, metricName);
                    }
                }
            }
            else if(variations == 3)
            {
                var variation = client.StringVariation(flagKey, user, "c");

                if (variation == "c")
                {
                    c++;
                    var hitRandom = (new Random()).Next(0, 50);
                    if (hitRandom % 3 == 0 || hitRandom % 5 == 0)
                    {
                        client.Track(user, metricName);
                    }
                }
                else if (variation == "t1")
                {
                    t1++;
                    var hitRandom = (new Random()).Next(0, 100);
                    if (hitRandom % 3 == 0 || hitRandom % 4 == 0 || hitRandom % 5 == 0)
                    {
                        client.Track(user, metricName);
                    }
                }
                else if (variation == "t2")
                {
                    t2++;
                    var hitRandom = (new Random()).Next(0, 100);
                    if (hitRandom % 3 == 0 || hitRandom % 4 == 0 || hitRandom % 5 == 0 || hitRandom % 6 == 0)
                    {
                        client.Track(user, metricName);
                    }
                }
            }
        }

        if (variations == 2)
            Console.WriteLine($"Experiment/Controlled: {v2Round}/{roundRandom-v2Round}");
        else if (variations == 3)
            Console.WriteLine($"C/T1/T2: {c}/{t1}/{t2}");
        await Task.Delay(1000);
    }

    // close the client to ensure that all insights are sent out before the app exits
    // await client.CloseAsync();
}

async Task ABTestNumericTestAsync(string flagKey, string metricName)
{
    FbClient client = NewFbClient();

    while (true)
    {
        var roundRandom = (new Random()).Next(7, 20);
        int v2Round = 0;
        for (int i = 0; i < roundRandom; i++)
        {
            // create a user
            var userRandomGuid = Guid.NewGuid();
            var userKey = $"abnumerictest-user-{userRandomGuid}";
            var userName = $"abnumerictest-name-{userRandomGuid}";
            var user = FbUser.Builder(userKey).Name(userName).Build();

            // evaluate a boolean flag for a given user with evaluation detail
            var versionToUse = client.BoolVariation(flagKey, user);

            if (versionToUse == false)
            {
                var hitRandom = (new Random()).Next(1, 3);
                for (int j = 0; j < hitRandom; j++)
                {
                    var errorRandom = (new Random()).Next(3, 40);
                    client.Track(user, metricName, errorRandom);
                }
            }
            else
            {
                v2Round++;
                var hitRandom = (new Random()).Next(1, 3);
                for (int j = 0; j < hitRandom; j++)
                {
                    var errorRandom = (new Random()).Next(2, 30);
                    client.Track(user, metricName, errorRandom);
                }
            }
        }

        Console.WriteLine($"Experiment/Controlled: {v2Round}/{roundRandom - v2Round}");

        await Task.Delay(1000);
    }

    // close the client to ensure that all insights are sent out before the app exits
    // await client.CloseAsync();
}


async Task WriteDataDogAPMSimulationAsync()
{
    var apiRoute = "https://localhost:7180";

    List<string> apiNodes = new List<string>() {
    "api/AirQuality/so2",
    "api/AirQuality/no2",
    "api/AirQuality/pm10",
    "api/AirQuality/pm2p5",
    "api/RainForecast",
    "WeatherForecast",
};

    while (true)
    {
        int totalCall = 0;
        int successCall = 0;
        int errorCall = 0;
        foreach (var apiNode in apiNodes)
        {
            HttpClient client = new HttpClient();
            int numberOfCall = (new Random()).Next(3, 10);
            for (int i = 0; i < numberOfCall; i++)
            {
                totalCall++;
                HttpResponseMessage response = await client.GetAsync($"{apiRoute}/{apiNode}");
                if (response.IsSuccessStatusCode)
                    successCall++;
                else
                    errorCall++;
            }
        }
        Console.WriteLine($"Total Call: {totalCall}; Error Call: {errorCall}");

        await Task.Delay((new Random()).Next(1000, 3000));
    }
}

static FbClient NewFbClient()
{
    // setup sdk options
    var options = new FbOptionsBuilder("ZtVbXh-Z8EeiUYzrMRXXbAFpMBrbbPSUyvVZ2pHH_8rA")
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