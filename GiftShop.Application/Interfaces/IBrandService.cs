using GiftShop.Application.Constrants.Responses;
using GiftShop.Infastructure.Models;

namespace GiftShop.Application.Interfaces;

public interface IBrandService
{
    Task<BaseResponse> GetBrands(string name, string code);
    Task<BaseResponse> GetBrandByID(Guid id);
    Task<BaseResponse> CreateBrand(BrandModel brand);
    Task<BaseResponse> DeleteBrand(Guid id);
    Task<BaseResponse> UpdateBrand(Guid id, BrandModel brand);
}
