using AutoMapper;
using GiftShop.Api.Models;
using GiftShop.Application.Interfaces;
using GiftShop.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftShop.Api.Controllers.CMS
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/brand")]
    public class BrandController(
        ILogger<BrandController> _logger, 
        IMapper _mapper, 
        IBrandService _brandService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetBrands([FromQuery] string name, string code)
        {
            return Json(await _brandService.GetBrands(name, code));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandByID([FromQuery] int id)
        {
            return Json(await _brandService.GetBrandByID(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand([FromBody]BrandModel brandModel)
        {
            var brand = _mapper.Map<Brand>(brandModel);
            return Json(await _brandService.CreateBrand(brand));
        }
    }
}
