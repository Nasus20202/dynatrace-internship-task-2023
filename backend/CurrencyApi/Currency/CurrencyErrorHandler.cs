using CurrencyApi.Currency.CurrencyService.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CurrencyApi.Currency;

public class CurrencyErrorHandler : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is DataNotFoundException)
        {
            context.Result = new NotFoundObjectResult(context.Exception.Message);
            context.ExceptionHandled = true;
        } 
        else if (context.Exception is FetchFailedException)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            context.ExceptionHandled = true;
        }
    }
}