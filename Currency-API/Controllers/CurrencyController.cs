﻿using Microsoft.AspNetCore.Mvc;
using CurrencyApi.RatesApi;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyApi.Controllers;

[ApiController]
[Route("exchange")]
public class CurrencyController : Controller
{
    private IRatesApi _ratesApi; 
    private IMemoryCache _cache;
    
    private const int MaxQuotations = 255;
    
    public CurrencyController(IMemoryCache cache, IRatesApi ratesApi) => (_cache, _ratesApi) = (cache, ratesApi);

    [HttpGet]
    [Route("average/{currencyCode}/{dateTime:datetime=yyyy-MM-dd}")]
    public async Task<IActionResult> GetAverageExchangeRate(string currencyCode, DateTime dateTime)
    {
        var date = DateOnly.FromDateTime(dateTime);
        currencyCode = currencyCode.ToUpper();
        double exchangeRate;
        try {
            exchangeRate = await _ratesApi.GetAverageExchangeRate(currencyCode, date);
        } catch (DataNotFoundException) {
            return NotFound($"Data not found for {currencyCode} on {date}");
        } catch (FetchFailedException) {
            return StatusCode(500, "Failed to fetch data from NBP API");
        }
        var result = new { currencyCode, date, exchangeRate };
        return Ok(result);
    }
    
    [HttpGet]
    [Route("extremes/{currencyCode}/{quotations:int}")]
    public async Task<IActionResult> GetMaxAverageExchangeRate(string currencyCode, int quotations)
    {
        if(quotations <= 0)
            return BadRequest("Quotations must be greater than 0");
        if (quotations > MaxQuotations)
            return BadRequest($"Quotations must be less than {MaxQuotations}");
        DateAndValue minExchangeRate, maxExchangeRate;
        try {
            (minExchangeRate, maxExchangeRate) = await _ratesApi.GetMinAndMaxAverageExchangeRate(currencyCode, quotations);
        }
        catch (DataNotFoundException) {
            return NotFound($"Data not found for {currencyCode} on {quotations} quotations");
        }
        catch (FetchFailedException) {
            return StatusCode(500, "Failed to fetch data from NBP API");
        }

        var result = new { currencyCode, quotations, minExchangeRate, maxExchangeRate };
        return Ok(result);
    }
    
}