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
    options.EventUri = new Uri("http://localhost:5100");
    options.StreamingUri = new Uri("ws://localhost:5100");
    options.EnvSecret = "gvkuIffZRkWoXWM-VumvsAJGumzSi8qUeo7MWeDXG0jQ";
    options.StartWaitTime = TimeSpan.FromSeconds(3);
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

app.Run();
