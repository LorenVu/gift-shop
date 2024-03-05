using GiftShop.Application.Interfaces;
using GiftShop.Infastructure.DTOs;
using GiftShop.Infastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftShop.Api.Controllers.Cms;

[AllowAnonymous]
[ApiController]
[Route("/api/v1/product")]
public class ProductController(
        ILogger<ProductController> _logger,
        IProductService _productService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] ProductModel model)
    {
        return Ok(await _productService.GetProducts(model));
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductDTO productDTO)
    {
        return Ok(await _productService.CreateProduct(productDTO));
    }
}
