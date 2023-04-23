using Microsoft.AspNetCore.Mvc;
using CurrencyApi.Currency.CurrencyService;
using CurrencyApi.Currency.CurrencyDtos;
using CurrencyApi.Currency.ErrorHandling;

namespace CurrencyApi.Currency;

/// <summary>
/// Controller for currency exchange rates
/// </summary>
[ApiController]
[TypeFilter(typeof(CurrencyErrorHandler))]
[Route("exchange")]
public class CurrencyController : Controller
{
    private const int MaxQuotations = 255;
    private readonly IRatesService _ratesService;
    
    public CurrencyController(IRatesService ratesService) => _ratesService = ratesService;

    
    /// <summary>
    /// Return average exchange rate for given currency code and today's date
    /// </summary>
    /// <param name="currencyCode">3 letter currency code</param>
    /// <returns>Average exchange rate</returns>
    /// <response code="404">Data not found</response>
    /// <response code="500">Fetch failed</response>
    [HttpGet]
    [Produces("application/json", Type=typeof(AverageRateDto))]
    [Route("average/{currencyCode}")]
    public async Task<IActionResult> GetAverageExchangeRate(string currencyCode)
    {
        return await GetAverageExchangeRate(currencyCode, DateTime.Today);
    }

    
    /// <summary>
    /// Return average exchange rate for given currency code and date
    /// </summary>
    /// <param name="currencyCode">3 letter currency code</param>
    /// <param name="dateTime">Date to check (YYYY-MM-DD)</param>
    /// <returns>Average exchange rate</returns>
    /// <response code="404">Data not found</response>
    /// <response code="500">Fetch failed</response>
    [HttpGet]
    [Produces("application/json", Type=typeof(AverageRateDto))]
    [Route("average/{currencyCode}/{dateTime:datetime=YYYY-MM-DD}")]
    public async Task<IActionResult> GetAverageExchangeRate(string currencyCode, DateTime dateTime)
    {
        var date = DateOnly.FromDateTime(dateTime);
        currencyCode = currencyCode.ToUpper();
        
        var rate = await _ratesService.GetAverageExchangeRate(currencyCode, date);
        return Ok(new AverageRateDto(currencyCode, date, rate));
    }

    
    /// <summary>
    /// Return min and max average exchange rate for given currency code and last quotations
    /// </summary>
    /// <param name="currencyCode">3 letter currency code</param>
    /// <param name="quotations">Number of last quotations to check</param>
    /// <returns>Min and max average exchange rate</returns>
    /// <response code="400">Invalid quotations count</response>
    /// <response code="404">Data not found</response>
    /// <response code="500">Fetch failed</response>
    [HttpGet]
    [Produces("application/json", Type=typeof(ExtremeRatesDto))]
    [Route("extremes/{currencyCode}/{quotations:int?}")]
    public async Task<IActionResult> GetExtremeAverageExchangeRate(string currencyCode, int quotations = MaxQuotations)
    {
        currencyCode = currencyCode.ToUpper();
        
        if (quotations <= 0)
        {
            return BadRequest("Quotations must be greater than 0");
        }
        if (quotations > MaxQuotations){
            return BadRequest($"Quotations must be less or equal {MaxQuotations}");
        }
        
        var data = await _ratesService.GetMinAndMaxAverageExchangeRate(currencyCode, quotations);
        return Ok(new ExtremeRatesDto(currencyCode, quotations, data.min, data.max));
    }

    
    /// <summary>
    /// Return max difference between buy and ask for given currency code and last quotations
    /// </summary>
    /// <param name="currencyCode">3 letter currency code</param>
    /// <param name="quotations">Number of last quotations to check</param>
    /// <returns>Max difference between buy and ask</returns>
    /// <response code="400">Invalid quotations count</response>
    /// <response code="404">Data not found</response>
    /// <response code="500">Fetch failed</response>
    [HttpGet]
    [Produces("application/json", Type=typeof(RateDifferenceDto))]
    [Route("maxBuyAskDifference/{currencyCode}/{quotations:int?}")]
    public async Task<IActionResult> GetMaxDifferenceBetweenBuyAndAsk(string currencyCode, int quotations = MaxQuotations)
    {
        currencyCode = currencyCode.ToUpper();
        
        if (quotations <= 0)
        {
            return BadRequest("Quotations must be greater than 0");
        }
        if (quotations > MaxQuotations)
        {
            return BadRequest($"Quotations must be less or equal {MaxQuotations}");
        }
        
        var data = await _ratesService.GetMaxDifferenceBetweenBuyAndAsk(currencyCode, quotations);
        return Ok(new RateDifferenceDto(currencyCode, quotations, data));
    }
}