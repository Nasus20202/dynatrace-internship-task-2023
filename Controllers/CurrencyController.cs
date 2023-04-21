using Microsoft.AspNetCore.Mvc;

namespace CurrencyApi.Controllers;

[ApiController]
[Route("exchange")]
public class CurrencyController : Controller
{
    [HttpGet]
    [Route("hello")]
    public IActionResult Index()
    {
        return Ok("Hello World");
    }
}