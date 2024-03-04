using GiftShop.Application.Constrants.Responses;
using GiftShop.Application.Interfaces;
using GiftShop.Domain.Entities;
using GiftShop.Domain.Enums;
using GiftShop.Infastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Net;

namespace GiftShop.Application.Services;

public class BrandService(ILogger<BrandService> _logger, IUnitOfWork _unitOfWork) : IBrandService
{
    public async Task<BaseResponse> GetBrands(string name, string code)
    {
        var response = new BaseResponse();

        try
        {
            Expression<Func<Brand, bool>> predicate = entity =>
            (string.IsNullOrEmpty(name) || entity.Name == name) &&
            (string.IsNullOrEmpty(code) || entity.Code == code);

            var brands = await _unitOfWork.Brands.GetAllByCondition(predicate);

            response = await CreateResponse((int)HttpStatusCode.OK, false, "Thành công", brands, (int)EErrorCommon.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError($"BrandService|GetBrands|Error: {ex.Message}");
            response = await CreateResponse((int)HttpStatusCode.OK, false, "Thành công", null, (int)EErrorCommon.API_EXCEPTION);
        }

        return response;
    }

    public async Task<BaseResponse> GetBrandByID(int id)
    {
        var response = new BaseResponse();

        try
        {
            var brand = await _unitOfWork.Brands.FindByIdAsync(id);

            response = await CreateResponse((int)HttpStatusCode.OK, false, "Thành công", brand, (int)EErrorCommon.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError($"BrandService|GetBrands|Error: {ex.Message}");
            response = await CreateResponse((int)HttpStatusCode.OK, false, "Thành công", null, (int)EErrorCommon.API_EXCEPTION);
        }

        return response;
    }

    public async Task<BaseResponse> CreateBrand(Brand brand)
    {
        var response = new BaseResponse();

        try
        {
            if(string.IsNullOrEmpty(brand.Name))
            {
                await CreateResponse((int)HttpStatusCode.OK, false, "Thành công", brand, (int)EErrorCommon.INVALID_PARAMS);
            }

            if (string.IsNullOrEmpty(brand.Code))
            {
                await CreateResponse((int)HttpStatusCode.OK, false, "Thành công", brand, (int)EErrorCommon.INVALID_PARAMS);
            }

            await _unitOfWork.Brands.AddAsync(brand);

            var result = await _unitOfWork.SaveChangeAsync();

            response = result > 0
                ? await CreateResponse((int)HttpStatusCode.OK, false, "Thành công", brand, (int)EErrorCommon.OK)
                : await CreateResponse((int)HttpStatusCode.OK, false, "Thất bại", brand, (int)EErrorCommon.SYSTEM_TIMEOUT);
        }
        catch (Exception ex)
        {
            _logger.LogError($"BrandService|CreateBrand|Error: {ex.Message}");
            response = await CreateResponse((int)HttpStatusCode.OK, false, "Thất bại", null, (int)EErrorCommon.API_EXCEPTION);
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
