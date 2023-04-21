using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using CurrencyApi.RatesApi;

namespace CurrencyApi.Controllers;

[ApiController]
[Route("exchange")]
public class CurrencyController : Controller
{
    private readonly IMemoryCache _cache;
    private IRatesApi _ratesApi; 
    
    public CurrencyController(IMemoryCache cache, IRatesApi ratesApi) => (_cache, _ratesApi) = (cache, ratesApi);

    [HttpGet]
    [Route("average/{currencyCode}/{date:datetime=yyyy-MM-dd}")]
    public async Task<IActionResult> Index(string currencyCode, DateTime date)
    {
        var response = await _ratesApi.GetAverageExchangeRate(currencyCode, date);
        return Ok(response);
    }
}