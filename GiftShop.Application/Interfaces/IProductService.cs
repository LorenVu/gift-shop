using GiftShop.Application.Constrants.Responses;
using GiftShop.Infastructure.DTOs;
using GiftShop.Infastructure.Models;

namespace GiftShop.Application.Interfaces;

public interface IProductService
{
    Task<BaseResponse> GetProducts(ProductModel model);
    Task<BaseResponse> CreateProduct(ProductDTO productDto);
}
