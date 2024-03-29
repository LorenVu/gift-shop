﻿using AutoMapper;
using GiftShop.Application.Constrants.Responses;
using GiftShop.Application.Interfaces;
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

public class BrandService(
    ILogger<BrandService> _logger, 
    IMapper _mapper, 
    IUnitOfWork _unitOfWork) : IBrandService
{
    public async Task<BaseResponse> GetBrands(string name, string code)
    {
        var response = new BaseResponse();

        try
        {
            Expression<Func<Brand, bool>> predicate = entity =>
            (string.IsNullOrEmpty(name) || entity.Name.Contains(name)) &&
            (string.IsNullOrEmpty(code) || entity.Code.Contains(code));

            var brands = _unitOfWork.Brands.GetAllByCondition(predicate);
            await brands.ToListAsync();

            response = await CreateResponse((int)HttpStatusCode.OK, false, CommonMessage.SUCCESSD, _mapper.Map<List<BrandDTO>>(brands), (int)EErrorCommon.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError($"BrandService|GetBrands|Error: {ex.Message}");
            response = await CreateResponse((int)HttpStatusCode.InternalServerError, false, CommonMessage.FAILED, null, (int)EErrorCommon.API_EXCEPTION);
        }

        return response;
    }

    public async Task<BaseResponse> GetBrandByID(Guid id)
    {
        var response = new BaseResponse();

        try
        {
            var brand = await _unitOfWork.Brands.FindByIdAsync(id);

            response = await CreateResponse((int)HttpStatusCode.OK, false, CommonMessage.SUCCESSD, _mapper.Map<BrandDTO>(brand), (int)EErrorCommon.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError($"BrandService|GetBrands|Error: {ex.Message}");
            response = await CreateResponse((int)HttpStatusCode.InternalServerError, false, CommonMessage.FAILED, null, (int)EErrorCommon.API_EXCEPTION);
        }

        return response;
    }

    public async Task<BaseResponse> CreateBrand(BrandModel brandModel)
    {
        var response = new BaseResponse();

        try
        {
            if(string.IsNullOrEmpty(brandModel.Name))
                return await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.FAILED, null, (int)EErrorCommon.INVALID_PARAMS);

            if (string.IsNullOrEmpty(brandModel.Code))
                return await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.FAILED, null, (int)EErrorCommon.INVALID_PARAMS);

            var brand = _mapper.Map<Brand>(brandModel);
            brand.ID = Guid.NewGuid();
            brand.CreatedDate = DateTime.Now;
            brand.ModifiedDate = DateTime.Now;

            await _unitOfWork.Brands.AddAsync(brand);
            var result = await _unitOfWork.SaveChangeAsync();

            response = result > 0
                ? await CreateResponse((int)HttpStatusCode.OK, false, CommonMessage.SUCCESSD, null, (int)EErrorCommon.OK)
                : await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.FAILED, null, (int)EErrorCommon.SYSTEM_TIMEOUT);
        }
        catch (Exception ex)
        {
            _logger.LogError($"BrandService|CreateBrand|Error: {ex.Message}");
            response = await CreateResponse((int)HttpStatusCode.InternalServerError, false, CommonMessage.FAILED, null, (int)EErrorCommon.API_EXCEPTION);
        }

        return response;
    }

    public async Task<BaseResponse> DeleteBrand(Guid id)
    {
        var response = new BaseResponse();

        try
        {
            if(string.IsNullOrWhiteSpace(id.ToString()))
                return await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.FAILED, null, (int)EErrorCommon.INVALID_PARAMS);

            var brand = await _unitOfWork.Brands.FindByIdAsync(id);
            
            if(brand is not null)
            {
                brand.IsDeleted = true;
                _unitOfWork.Brands.Update(brand);

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
            _logger.LogError($"BrandService|DeleteBrand|Error: {ex.Message}");
            response = await CreateResponse((int)HttpStatusCode.InternalServerError, false, CommonMessage.FAILED, null, (int)EErrorCommon.API_EXCEPTION);
        }

        return response;
    }

    public async Task<BaseResponse> UpdateBrand(Guid id, BrandModel brandModel)
    {
        var response = new BaseResponse();

        try
        {
            if (brandModel is null)
                return await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.FAILED, null, (int)EErrorCommon.INVALID_PARAMS);

            if (brandModel is not null)
            {
                var brand = await _unitOfWork.Brands.FindByIdAsync(id);

                if(brand is not null)
                {
                    brand.Name = brandModel.Name;
                    brand.Code = brandModel.Code;
                    brand.ModifiedDate = DateTime.Now;
                    brand.ModifiedUser = "longvn";

                    _unitOfWork.Brands.Update(brand);
                    var result = await _unitOfWork.SaveChangeAsync();

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
            _logger.LogError($"BrandService|DeleteBrand|Error: {ex.Message}");
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
