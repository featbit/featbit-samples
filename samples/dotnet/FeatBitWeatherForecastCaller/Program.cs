// See https://aka.ms/new-console-template for more information

Console.WriteLine("Console APP for generating DataDog APM simulation data");

await WriteDataDogAPMSimulationAsync();

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