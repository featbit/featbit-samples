using FeatBit.Sdk.Server.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add FeatBit service
builder.Services.AddFeatBit(options =>
{
    options.EventUri = new Uri("https://featbit-tio-eu-eval.azurewebsites.net");
    options.StreamingUri = new Uri("wss://featbit-tio-eu-eval.azurewebsites.net");
    options.EnvSecret = "######lhl3wj0Ek0OXorUNT20cYQ3zSJhPUPg0mRr59x9t_NSg";
    options.StartWaitTime = TimeSpan.FromSeconds(3);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
