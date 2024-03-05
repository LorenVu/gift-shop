using AutoMapper;
using GiftShop.Application.Constrants.Responses;
using GiftShop.Application.Interfaces;
using GiftShop.Domain.Commons.Extentions.DateTimes;
using GiftShop.Domain.Commons.Message;
using GiftShop.Domain.Entities;
using GiftShop.Domain.Enums;
using GiftShop.Infastructure.DTOs;
using GiftShop.Infastructure.Interfaces;
using GiftShop.Infastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Net;

namespace GiftShop.Application.Services;

public class ProductService(
    ILogger<BrandService> _logger,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : IProductService
{
    public async Task<BaseResponse> GetProducts(ProductModel model)
    {
        var response = new BaseResponse();

        try
        {
            Expression<Func<Product, bool>> predicate = entity =>
            (string.IsNullOrEmpty(model.Name) || entity.Name.Contains(model.Name)) &&
            (string.IsNullOrEmpty(model.Code) || entity.Code.Contains(model.Code)) &&
            (model.Stock || entity.Stock == model.Stock) &&
            (string.IsNullOrEmpty(model.FromDate) && string.IsNullOrEmpty(model.ToDate) || 
                entity.CreatedDate >= model.FromDate.ToDateTime("dd/MM/yyyy") && entity.CreatedDate <= model.ToDate.ToDateTime("dd/MM/yyyy"));

            var brands = _unitOfWork.Products.GetAllByCondition(predicate);

            if(model.OrderByDiscount) brands.OrderByDescending(x => x.Discount);
            if(model.OrderByCurrency) brands.OrderByDescending(x => x.Currency);
            if(model.OrderByPrize) brands.OrderByDescending(x => x.Prize);

            await brands.ToListAsync();

            response = await CreateResponse((int)HttpStatusCode.OK, false, CommonMessage.SUCCESSD, _mapper.Map<List<ProductDTO>>(brands), (int)EErrorCommon.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError($"ProductService|GetProducts|Error: {ex.Message}");
            response = await CreateResponse((int)HttpStatusCode.InternalServerError, false, CommonMessage.FAILED, null, (int)EErrorCommon.API_EXCEPTION);
        }

        return response;
    }

    public async Task<BaseResponse> CreateProduct(ProductDTO productDto)
    {
        var response = new BaseResponse();

        try
        {
            var product = _mapper.Map<Product>(productDto);
            product.ID = Guid.NewGuid();
            product.CreatedDate = DateTime.Now;
            product.ModifiedDate = DateTime.Now;

            product.Properties.ToList().ForEach(p =>
            {
                p.ID = Guid.NewGuid();
                p.ProductID = product.ID;
            });

            var result = await _unitOfWork.Products.CreateProductWithProperty(product);

            response = result > 0
                ? await CreateResponse((int)HttpStatusCode.OK, false, CommonMessage.SUCCESSD, null, (int)EErrorCommon.OK)
                : await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.FAILED, null, (int)EErrorCommon.SYSTEM_TIMEOUT);
        }
        catch(Exception ex)
        {
            _logger.LogError($"BrandService|CreateProduct|Error: {ex.Message}");
            response = await CreateResponse((int)HttpStatusCode.InternalServerError, false, CommonMessage.FAILED, null, (int)EErrorCommon.API_EXCEPTION);
        }

        return response;
    }

    #region === Private Method ===
    private async Task<BaseResponse> CreateResponse(int status, bool hasError, string message, object data, int errorCode)
    {
        await Task.CompletedTask;
        return new BaseResponse(status, hasError, message, data, errorCode);
    }
    #endregion === Private Method ===
}
