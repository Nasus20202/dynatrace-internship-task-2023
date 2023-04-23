using Microsoft.AspNetCore.Mvc;
using CurrencyApi.Currency.CurrencyService;
using CurrencyApi.Currency.CurrencyService.Exceptions;
using CurrencyApi.Currency.CurrencyDto;

namespace CurrencyApi.Currency;

[ApiController]
[TypeFilter(typeof(CurrencyErrorHandler))]
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
        var rate = await _ratesService.GetAverageExchangeRate(currencyCode, date);
        return Ok(new AverageRateDto(currencyCode, date, rate));
    }
    
    [HttpGet]
    [Route("average/{currencyCode}")]
    public async Task<IActionResult> GetAverageExchangeRate(string currencyCode)
    {
        return await GetAverageExchangeRate(currencyCode, DateTime.Today);
    }
    
    [HttpGet]
    [Route("extremes/{currencyCode}/{quotations:int?}")]
    public async Task<IActionResult> GetExtremeAverageExchangeRate(string currencyCode, int quotations = MaxQuotations)
    {
        currencyCode = currencyCode.ToUpper();
        if(quotations <= 0)
            return BadRequest("Quotations must be greater than 0");
        if (quotations > MaxQuotations)
            return BadRequest($"Quotations must be less or equal {MaxQuotations}");
        var data = await _ratesService.GetMinAndMaxAverageExchangeRate(currencyCode, quotations);
        return Ok(new ExtremeRatesDto(currencyCode, quotations, data.min, data.max));
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
        var data = await _ratesService.GetMaxDifferenceBetweenBuyAndAsk(currencyCode, quotations);
        return Ok(new RateDifferenceDto(currencyCode, quotations, data));
    }

}