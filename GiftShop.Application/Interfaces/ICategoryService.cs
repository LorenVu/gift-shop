using GiftShop.Application.Constrants.Responses;
using GiftShop.Infastructure.DTOs;
using GiftShop.Infastructure.Models;

namespace GiftShop.Application.Interfaces;

public interface ICategoryService
{
    Task<BaseResponse> GetCategories(CategoryModel model);
    Task<BaseResponse> GetCategoriesByID(Guid id);
    Task<BaseResponse> CreateCategory(CategoryDTO brand);
    Task<BaseResponse> DeleteCategory(Guid id);
    Task<BaseResponse> UpdateCategory(Guid id, CategoryDTO brand);
}
