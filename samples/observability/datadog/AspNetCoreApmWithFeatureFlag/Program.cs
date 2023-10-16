using Datadog.Trace;
using Datadog.Trace.Configuration;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

//// Create a settings object using the existing
//// environment variables and config sources
//var settings = TracerSettings.FromDefaultSources();

//// Override a value
//settings.GlobalTags.Add("env", "demo");

//// Replace the tracer configuration
//Tracer.Configure(settings);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
