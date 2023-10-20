// See https://aka.ms/new-console-template for more information
using FeatBit.Sdk.Server.Model;
using FeatBit.Sdk.Server.Options;
using FeatBit.Sdk.Server;

Console.WriteLine("Hello, World!");

//await WriteDataDogAPMSimulationAsync();

const string flagKey = "abnumerictest";

//await ABTestConversionTestAsync(flagKey, "ab-test-conversion-test-hit");
await ABTestNumericTestAsync(flagKey, "ab-test-numeric-test-hit");


async Task ABTestConversionTestAsync(string flagKey, string metricName)
{
    FbClient client = NewFbClient();

    while (true)
    {
        var roundRandom = (new Random()).Next(2, 10);
        int v2Round = 0;
        for(int i = 0; i < roundRandom; i++)
        {
            // create a user
            var userRandomGuid = Guid.NewGuid();
            var userKey = $"abnumerictest-user-{userRandomGuid}";
            var userName = $"abnumerictest-name-{userRandomGuid}";
            var user = FbUser.Builder(userKey).Name(userName).Build();

            // evaluate a boolean flag for a given user with evaluation detail
            var versionToUse = client.StringVariation(flagKey, user, defaultValue: "off");

            if (versionToUse == "v1")
            {
                var hitRandom = (new Random()).Next(0, 50);
                if (hitRandom % 3 == 0 || hitRandom % 5 == 0)
                {
                    client.Track(user, metricName);
                }
            }
            else if (versionToUse == "v2")
            {
                v2Round++;
                var hitRandom = (new Random()).Next(0, 100);
                if (hitRandom % 3 == 0 || hitRandom % 5 == 0 || hitRandom % 4 == 0 || hitRandom % 6 == 0)
                {
                    client.Track(user, metricName);
                }
            }
        }

        Console.WriteLine($"{v2Round}/{roundRandom} are v2");
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
            var versionToUse = client.StringVariation(flagKey, user, defaultValue: "off");

            if (versionToUse == "v1")
            {
                var hitRandom = (new Random()).Next(1, 3);
                for (int j = 0; j < hitRandom; j++)
                {
                    var errorRandom = (new Random()).Next(20, 50);
                    client.Track(user, metricName, errorRandom);
                }
            }
            else if (versionToUse == "v2")
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

        Console.WriteLine($"{v2Round}/{roundRandom} are v2");

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
    var options = new FbOptionsBuilder("gvkuIffZRkWoXWM-VumvsAJGumzSi8qUeo7MWeDXG0jQ")
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