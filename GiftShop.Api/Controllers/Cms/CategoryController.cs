using GiftShop.Application.Interfaces;
using GiftShop.Application.Services;
using GiftShop.Infastructure.DTOs;
using GiftShop.Infastructure.Interfaces;
using GiftShop.Infastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftShop.Api.Controllers.Cms;

[AllowAnonymous]
[ApiController]
[Route("/api/v1/category")]
public class CategoryController(
        ILogger<CategoryController> _logger,
        ICategoryService _categoryService) : Controller
{

    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] CategoryModel model)
    {
        return Ok(await _categoryService.GetCategories(model));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBrandByID([FromRoute] Guid id)
    {
        return Ok(await _categoryService.GetCategoriesByID(id));
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrand([FromBody] CategoryDTO categoryDTO)
    {
        return Ok(await _categoryService.CreateCategory(categoryDTO));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
    {
        return Ok(await _categoryService.DeleteCategory(id));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] CategoryDTO categoryDTO)
    {
        return Ok(await _categoryService.UpdateCategory(id, categoryDTO));
    }
}
