using AutoMapper;
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

public class CategoryService(
    ILogger<CategoryService> _logger, 
    IMapper _mapper, 
    IUnitOfWork _unitOfWork) : ICategoryService
{
    public async Task<BaseResponse> GetCategories(CategoryModel model)
    {
        var response = new BaseResponse();

        try
        {
            Expression<Func<Category, bool>> predicate = entity =>
            (string.IsNullOrEmpty(model.Name) || entity.Name.Contains(model.Name)) &&
            (string.IsNullOrEmpty(model.Code) || entity.Code.Contains(model.Code)); //&&
            //(model.IsDeleted != null || entity.IsDeleted == model.IsDeleted) &&
            //(model.Status != null || entity.Status == model.Status);

            var categories = _unitOfWork.Categories.GetAllByCondition(predicate);
            await categories.ToListAsync();

            response = await CreateResponse((int)HttpStatusCode.OK, false, CommonMessage.SUCCESSD, _mapper.Map<List<CategoryDTO>>(categories), (int)EErrorCommon.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError($"CategoryService|GetCategories|Error: {ex.Message}");
            response = await CreateResponse((int)HttpStatusCode.InternalServerError, false, CommonMessage.FAILED, null, (int)EErrorCommon.API_EXCEPTION);
        }

        return response;
    }

    public async Task<BaseResponse> GetCategoriesByID(Guid id)
    {
        var response = new BaseResponse();

        try
        {
            var brand = await _unitOfWork.Categories.FindByIdAsync(id);

            response = await CreateResponse((int)HttpStatusCode.OK, false, CommonMessage.SUCCESSD, _mapper.Map<CategoryDTO>(brand), (int)EErrorCommon.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError($"CategoryService|GetCategoriesByID|Error: {ex.Message}");
            response = await CreateResponse((int)HttpStatusCode.InternalServerError, false, CommonMessage.FAILED, null, (int)EErrorCommon.API_EXCEPTION);
        }

        return response;
    }

    public async Task<BaseResponse> CreateCategory(CategoryDTO categoryDTO)
    {
        var response = new BaseResponse();

        try
        {
            if(string.IsNullOrEmpty(categoryDTO.Name))
                return await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.FAILED, null, (int)EErrorCommon.INVALID_PARAMS);

            if (string.IsNullOrEmpty(categoryDTO.Code))
                return await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.FAILED, null, (int)EErrorCommon.INVALID_PARAMS);

            var category = _mapper.Map<Category>(categoryDTO);
            category.ID = Guid.NewGuid();
            category.CreatedDate = DateTime.Now;
            category.ModifiedDate = DateTime.Now;

            await _unitOfWork.Categories.AddAsync(category);
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

    public async Task<BaseResponse> DeleteCategory(Guid id)
    {
        var response = new BaseResponse();

        try
        {
            if(string.IsNullOrWhiteSpace(id.ToString()))
                return await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.FAILED, null, (int)EErrorCommon.INVALID_PARAMS);

            var category = await _unitOfWork.Categories.FindByIdAsync(id);
            
            if(category is not null)
            {
                category.IsDeleted = true;
                _unitOfWork.Categories.Update(category);

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

    public async Task<BaseResponse> UpdateCategory(Guid id, CategoryDTO categoryDTO)
    {
        var response = new BaseResponse();

        try
        {
            if (categoryDTO is null)
                return await CreateResponse((int)HttpStatusCode.BadRequest, false, CommonMessage.FAILED, null, (int)EErrorCommon.INVALID_PARAMS);

            if (categoryDTO is not null)
            {
                var category = await _unitOfWork.Categories.FindByIdAsync(id);

                if(category is not null)
                {
                    category.Name = categoryDTO.Name;
                    category.Code = categoryDTO.Code;
                    category.ModifiedDate = DateTime.Now;
                    category.ModifiedUser = "longvn";

                    _unitOfWork.Categories.Update(category);
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
