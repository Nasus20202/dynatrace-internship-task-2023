using CurrencyApi.Currency.CurrencyService.Exceptions;
using CurrencyApi.Currency.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CurrencyApi.Currency.ErrorHandling;

public class CurrencyErrorHandler : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is DataNotFoundException)
        {
            context.Result = new NotFoundObjectResult(new ErrorContext(context.Exception.Message, StatusCodes.Status404NotFound));
            context.ExceptionHandled = true;
        } 
        else if (context.Exception is FetchFailedException)
        {
            const int statusCode = 500;
            context.Result = new ObjectResult(new ErrorContext(context.Exception.Message, statusCode))
            {
                StatusCode = statusCode
            };
            context.ExceptionHandled = true;
        }
        else if (context.Exception is InvalidQuotationsCountException)
        {
            context.Result = new BadRequestObjectResult(new ErrorContext(context.Exception.Message, StatusCodes.Status400BadRequest));
            context.ExceptionHandled = true;
        }
    }
}