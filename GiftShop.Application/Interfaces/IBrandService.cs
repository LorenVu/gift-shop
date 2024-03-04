using GiftShop.Application.Constrants.Responses;
using GiftShop.Domain.Entities;

namespace GiftShop.Application.Interfaces;

public interface IBrandService
{
    Task<BaseResponse> GetBrands(string name, string code);
    Task<BaseResponse> GetBrandByID(int id);
    Task<BaseResponse> CreateBrand(Brand brand);
}
