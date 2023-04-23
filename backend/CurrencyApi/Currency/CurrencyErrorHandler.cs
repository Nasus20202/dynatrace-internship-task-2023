using CurrencyApi.Currency.CurrencyService.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CurrencyApi.Currency;

public class CurrencyErrorHandler : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (type == typeof(DataNotFoundException))
        {
            context.Result = new NotFoundObjectResult(context.Exception.Message);
            context.ExceptionHandled = true;
        } 
        else if (type == typeof(FetchFailedException))
        {
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            context.ExceptionHandled = true;
        }
    }
}