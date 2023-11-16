using API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<HelloWorldEndpoint>();
builder.Services.AddSingleton<HelloNameEndpoint>();
builder.Services.AddSingleton<WeatherEndpoint>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/hello-world", (HelloWorldEndpoint endpoint) => Task.FromResult(endpoint.Get()));
app.MapGet("/hello/world", (HelloWorldEndpoint endpoint) => Task.FromResult(endpoint.Get()));
app.MapGet("/hello/{name}", (HelloNameEndpoint endpoint, string name) => Task.FromResult(endpoint.Get(name)));
app.MapGet("/weather", (WeatherEndpoint endpoint) => Task.FromResult(endpoint.Get()));

app.Run();
