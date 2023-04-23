using Microsoft.AspNetCore.Mvc;
using CurrencyApi.Currency.CurrencyService;
using CurrencyApi.Currency.CurrencyService.Exceptions;

namespace CurrencyApi.Currency;

[ApiController]
[Route("exchange")]
public class CurrencyController : Controller
{
    private readonly IRatesService _ratesService;
    private const int MaxQuotations = 255;
    
    public CurrencyController(IRatesService ratesService) => _ratesService = ratesService;

    [HttpGet]
    [Route("average/{currencyCode}/{dateTime:datetime=yyyy-MM-dd}")]
    public async Task<IActionResult> GetAverageExchangeRate(string currencyCode, DateTime dateTime)
    {
        var date = DateOnly.FromDateTime(dateTime);
        currencyCode = currencyCode.ToUpper();
        double exchangeRate;
        try {
            exchangeRate = await _ratesService.GetAverageExchangeRate(currencyCode, date);
        } catch (DataNotFoundException) {
            return NotFound($"Data not found for {currencyCode} on {date}");
        } catch (FetchFailedException) {
            return StatusCode(500, "Failed to fetch data from NBP API");
        }
        var result = new { currencyCode, date, exchangeRate };
        return Ok(result);
    }
    
    [HttpGet]
    [Route("average/{currencyCode}")]
    public async Task<IActionResult> GetAverageExchangeRate(string currencyCode)
    {
        return await GetAverageExchangeRate(currencyCode, DateTime.Today);
    }
    
    [HttpGet]
    [Route("extremes/{currencyCode}/{quotations:int?}")]
    public async Task<IActionResult> GetMaxAverageExchangeRate(string currencyCode, int quotations = MaxQuotations)
    {
        currencyCode = currencyCode.ToUpper();
        if(quotations <= 0)
            return BadRequest("Quotations must be greater than 0");
        if (quotations > MaxQuotations)
            return BadRequest($"Quotations must be less or equal {MaxQuotations}");
        DateAndValue minExchangeRate, maxExchangeRate;
        try {
            (minExchangeRate, maxExchangeRate) = await _ratesService.GetMinAndMaxAverageExchangeRate(currencyCode, quotations);
        }
        catch (DataNotFoundException)
        {
            return NotFound($"Data not found for {currencyCode} in {quotations} quotations");
        }
        catch (FetchFailedException) {
            return StatusCode(500);
        }

        var result = new { currencyCode, quotations, minExchangeRate, maxExchangeRate };
        return Ok(result);
    }
    
    [HttpGet]
    [Route("maxBuyAskDifference/{currencyCode}/{quotations:int?}")]
    public async Task<IActionResult> GetMaxDifferenceBetweenBuyAndAsk(string currencyCode, int quotations = MaxQuotations)
    {
        currencyCode = currencyCode.ToUpper();
        if(quotations <= 0)
            return BadRequest("Quotations must be greater than 0");
        if (quotations > MaxQuotations)
            return BadRequest($"Quotations must be less or equal {MaxQuotations}");
        DateAndValue maxDifference;
        try {
            maxDifference = await _ratesService.GetMaxDifferenceBetweenBuyAndAsk(currencyCode, quotations);
        }
        catch (DataNotFoundException) {
            return NotFound($"Data not found for {currencyCode} in {quotations} quotations");
        }
        catch (FetchFailedException) {
            return StatusCode(500, "Failed to fetch data from NBP API");
        }

        var result = new { currencyCode, quotations, maxDifference };
        return Ok(result);
    }

}