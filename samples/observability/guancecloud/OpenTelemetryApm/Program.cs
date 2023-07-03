using FeatBit.Sdk.Server.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetryApm;
using OpenTelemetryApm.Options;
using StackExchange.Redis;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OldDbContext>();
builder.Services.AddDbContext<NewAzureSqlDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NewAzureSqlDbConnection")));

var logger = LoggerFactory.Create(builder =>
{
    builder.AddOpenTelemetry(options =>
    {
        options.AddConsoleExporter();
        // add opentelemetry otel log exporter - not stable yet
        // https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/src/OpenTelemetry.Exporter.OpenTelemetryProtocol#enable-log-exporter

    });
}).CreateLogger("Program");

builder.Services.Configure<AzureOptions>(
    builder.Configuration.GetSection(AzureOptions.Position));

string redisCacheConnectionString = builder.Configuration.GetSection("Azure:RedisCache:ConnectionString")?.Value ?? "";
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisCacheConnectionString;
    options.InstanceName = builder.Configuration.GetSection("Azure:RedisCache:OpenTelemetryAPMInstance")?.Value ?? "";
});


builder.Services.AddSingleton<Instrumentation>();

string openTelemetryServiceName = "OpenTelemetryAPM";
ConnectionMultiplexer redisConnectionMultiplexer = ConnectionMultiplexer.Connect(redisCacheConnectionString);
builder.Services.AddOpenTelemetry()
    .ConfigureResource(builder => builder
        .AddService(serviceName: openTelemetryServiceName))
    .WithTracing(builder => builder
        .AddSource(Instrumentation.ActivitySourceName)
        .AddAspNetCoreInstrumentation()
        .AddSqlClientInstrumentation()
        .AddRedisInstrumentation(redisConnectionMultiplexer)
        .AddGrpcClientInstrumentation()
        .AddHttpClientInstrumentation()
        .AddConsoleExporter()
        .AddOtlpExporter())
    .WithMetrics(builder => builder
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter()
        .AddOtlpExporter());

builder.Services.AddFeatBit(options =>
{
    options.EnvSecret = "z4nZw2HYDkCGnB09R12TnAKIvcqEmwcUK-2mWFtGURaQ";
    options.StartWaitTime = TimeSpan.FromSeconds(3);
    options.StreamingUri = new Uri("wss://featbit-tio-eu-eval.azurewebsites.net");
    options.EventUri = new Uri("https://featbit-tio-eu-eval.azurewebsites.net");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => $"Hello World! OpenTelemetry Trace: {Activity.Current?.Id}");

app.Run();


public static class DiagnosticsConfig
{
    public const string ServiceName = "FeatBitGuanceService";
    public static ActivitySource ActivitySource = new ActivitySource(ServiceName);
}