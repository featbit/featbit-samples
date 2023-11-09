// See https://aka.ms/new-console-template for more information
using Statsig.Client;
using Statsig;

Console.WriteLine("Hello, World!");


while (true)
{
    var guid = Guid.NewGuid();
    await StatsigClient.Initialize(
        "client-uPcNfneXb7TnMQTknXylcz9vupCAkdnYOqSIbLv4KUB",
        new StatsigUser { UserID = $"{guid}", Email = $"{guid}@statsig.com" }
    );

    if (StatsigClient.CheckGate("feature-gate-001"))
    {
        // Gate is on, show new home page
        Console.WriteLine("feature-gate-001 passed");
    }
    else
    {
        // Gate is off, show old home page
        Console.WriteLine("feature-gate-001 failed");
    }
}


Console.ReadLine();