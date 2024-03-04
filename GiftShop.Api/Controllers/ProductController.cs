using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftShop.Api.Controllers;

[Authorize]
[ApiController]
[Route("/api/v1/product")]
public class ProductController : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        return Ok();
    }
}
