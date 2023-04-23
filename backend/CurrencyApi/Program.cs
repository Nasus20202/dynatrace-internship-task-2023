using System.Reflection;
using CurrencyApi.Currency.CurrencyService;
using CurrencyApi.Currency.CurrencyService.JsonFetcher;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" }); // Map DateOnly to string in Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // Generate Swagger documentation from XML comments
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

builder.Services.AddMemoryCache(); // turn on caching

// Dependency injection
builder.Services.AddTransient<IRatesService, NbpApiService>();
builder.Services.AddTransient<ICachedJsonFetcher, CachedJsonFetcher>();
// Add global HttpClient
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();