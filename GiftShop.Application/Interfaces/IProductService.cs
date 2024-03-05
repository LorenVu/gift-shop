using GiftShop.Application.Constrants.Responses;
using GiftShop.Infastructure.DTOs;
using GiftShop.Infastructure.Models;

namespace GiftShop.Application.Interfaces;

public interface IProductService
{
    Task<BaseResponse> GetProducts(ProductModel model);
    Task<BaseResponse> GetProductByID(Guid id);
    Task<BaseResponse> CreateProduct(ProductDTO productDto);
    Task<BaseResponse> DeleteProduct(Guid id);
    Task<BaseResponse> UpdateProduct(Guid id, ProductDTO productDTO);
}
