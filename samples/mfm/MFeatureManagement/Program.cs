using FeatBit.MFMProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddFeatureManagement(builder.Configuration.GetSection("FeatureManagement"));
builder.Services.AddSingleton<IFeatureDefinitionProvider, FeatBitFeatureDefinitionProvider>()
        .AddFeatureManagement()
        .AddFeatureFilter<FeatBitFilter>()
        .AddFeatureFilter<CustomFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
