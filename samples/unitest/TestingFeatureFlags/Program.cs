using FeatBit.Sdk.Server.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TestingFeatureFlags.Models;
using TestingFeatureFlags.Repositories;
using TestingFeatureFlags.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<SqlDataDbContext>(opt => opt.UseInMemoryDatabase("SqlDataDb"));
builder.Services.AddDbContext<NoSqlDataDbContext>(opt => opt.UseInMemoryDatabase("NoSqlDataDb"));
builder.Services.AddTransient<IOneRepository, OneRepository>();
builder.Services.AddTransient<IOneNoSqlRepository, OneNoSqlRepository>();
builder.Services.AddTransient<IDataService, DataService>();
builder.Services.AddFeatBit(options =>
{
    options.EnvSecret = "Og71vBFnOEqAasPD360oMw5ZjgdjXPU0qDo5LAVn4GzA";
    options.StreamingUri = new Uri("wss://app-eval.featbit.co");
    options.EventUri = new Uri("https://app-eval.featbit.co");
    options.StartWaitTime = TimeSpan.FromSeconds(3);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
