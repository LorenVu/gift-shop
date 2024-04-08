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
using System.Linq;
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
            (!model.Stock || entity.Stock == model.Stock);

            var prorudcts = _unitOfWork.Products.GetProductWithProperty(predicate);

            if (model.OrderByDiscount) prorudcts = model.OrderByDiscount ? prorudcts.OrderByDescending(x => x.Discount) : prorudcts.OrderBy(x => x.Discount);
            if (model.OrderByPrize) prorudcts = model.OrderByPrize ? prorudcts.OrderByDescending(x => x.Prize) : prorudcts.OrderBy(x => x.Prize);

            prorudcts.Where(b => b.CreatedDate >= model.FromDate.ToDateTime("dd/MM/yyyy") && b.CreatedDate <= model.ToDate.ToDateTime("dd/MM/yyyy"));

            await prorudcts.ToListAsync();
            response = await CreateResponse((int)HttpStatusCode.OK, false, CommonMessage.SUCCESSD, _mapper.Map<List<ProductDTO>>(prorudcts), (int)EErrorCommon.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError($"ProductService|GetProducts|Error: {ex.Message}");
            response = await CreateResponse((int)HttpStatusCode.InternalServerError, false, CommonMessage.FAILED, null, (int)EErrorCommon.API_EXCEPTION);
        }

        return response;
    }

    public async Task<BaseResponse> GetProductByID(Guid id)
    {
        var response = new BaseResponse();

        try
        {
            var product = await _unitOfWork.Products.FindProductByIDWithProperty(id);

            response = await CreateResponse((int)HttpStatusCode.OK, false, CommonMessage.SUCCESSD, _mapper.Map<ProductDTO>(product), (int)EErrorCommon.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError($"ProductService|GetProductID|Error: {ex.Message}");
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

            product.Images.ToList().ForEach(p =>
            {
                p.ID = Guid.NewGuid();
                p.ProductID = product.ID;
            });

            var result = await _unitOfWork.Products.CreateProduct(product);

            response = result > 0
                ? await CreateResponse((int)HttpStatusCode.OK, false, CommonMessage.SUCCESSD, null, (int)EErrorCommon.OK)
                : await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.FAILED, null, (int)EErrorCommon.SYSTEM_TIMEOUT);
        }
        catch(Exception ex)
        {
            _logger.LogError($"ProductService|CreateProduct|Error: {ex.Message}");
            response = await CreateResponse((int)HttpStatusCode.InternalServerError, false, CommonMessage.FAILED, null, (int)EErrorCommon.API_EXCEPTION);
        }

        return response;
    }

    public async Task<BaseResponse> DeleteProduct(Guid id)
    {
        var response = new BaseResponse();

        try
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
                return await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.FAILED, null, (int)EErrorCommon.INVALID_PARAMS);

            var product = await _unitOfWork.Products.FindByIdAsync(id);

            if (product is not null)
            {
                product.IsDeleted = true;
                _unitOfWork.Products.Update(product);

                var result = await _unitOfWork.SaveChangeAsync();

                response = result > 0
                ? await CreateResponse((int)HttpStatusCode.OK, false, CommonMessage.SUCCESSD, null, (int)EErrorCommon.OK)
                : await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.FAILED, null, (int)EErrorCommon.SYSTEM_TIMEOUT);
            }
            else
                response = await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.NOTFOUND_WITH_ID, null, (int)EErrorCommon.INVALID_PARAMS);

        }
        catch (Exception ex)
        {
            _logger.LogError($"ProductService|DeleteProduct|Error: {ex.Message}");
            response = await CreateResponse((int)HttpStatusCode.InternalServerError, false, CommonMessage.FAILED, null, (int)EErrorCommon.API_EXCEPTION);
        }

        return response;
    }

    public async Task<BaseResponse> UpdateProduct(Guid id, ProductDTO productDTO)
    {
        var response = new BaseResponse();

        try
        {
            if (productDTO is null)
                return await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.FAILED, null, (int)EErrorCommon.INVALID_PARAMS);

            if (productDTO is not null)
            {
                var product = await _unitOfWork.Products.FindProductByIDWithProperty(id);

                product = _mapper.Map<Product>(productDTO);

                if (product is not null)
                {
                    product.ModifiedDate = DateTime.Now;
                    product.ModifiedUser = "longvn";

                    //_unitOfWork.Products.Update(product);
                    //_unitOfWork.Properties.UpdateRange(product.Properties);

                    var result = await _unitOfWork.Products.UpdateProduct(product);

                    response = result > 0
                    ? await CreateResponse((int)HttpStatusCode.OK, false, CommonMessage.SUCCESSD, null, (int)EErrorCommon.OK)
                    : await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.FAILED, null, (int)EErrorCommon.SYSTEM_TIMEOUT);
                }
                else
                {
                    response = await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.NOTFOUND_WITH_ID, null, (int)EErrorCommon.INVALID_PARAMS);
                }
            }
            else
                response = await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.NOTFOUND_WITH_ID, null, (int)EErrorCommon.INVALID_PARAMS);

        }
        catch (Exception ex)
        {
            _logger.LogError($"ProductService|UpdateProduct|Error: {ex.Message}");
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
