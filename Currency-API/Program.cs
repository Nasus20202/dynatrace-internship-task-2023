using CurrencyApi.RatesApi;

var builder = WebApplication.CreateBuilder(args);

// CORS
var allowAllPolicy = "_allowAllPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowAllPolicy,
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IRatesApi, NbpApi>(); // Dependency injection for currency rates api
builder.Services.AddHttpClient(); // Dependency injection for http client

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(allowAllPolicy);
app.UseHttpsRedirection();

app.MapControllers();

app.Run();