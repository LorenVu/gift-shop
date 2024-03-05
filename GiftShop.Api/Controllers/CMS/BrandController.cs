using GiftShop.Application.Interfaces;
using GiftShop.Infastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftShop.Api.Controllers.Cms;

[ApiController]
[AllowAnonymous]
[Route("api/v1/brand")]
public class BrandController(
    ILogger<BrandController> _logger, 
    IBrandService _brandService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetBrands([FromQuery] string name = "", string code = "")
    {
        return Ok(await _brandService.GetBrands(name, code));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBrandByID([FromRoute] Guid id)
    {
        return Ok(await _brandService.GetBrandByID(id));
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrand([FromBody]BrandModel brandModel)
    {
        return Ok(await _brandService.CreateBrand(brandModel));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBrand([FromRoute] Guid id)
    {
        return Ok(await _brandService.DeleteBrand(id));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBrand([FromRoute] Guid id, [FromBody] BrandModel brandModel)
    {
        return Ok(await _brandService.UpdateBrand(id, brandModel));
    }
}
