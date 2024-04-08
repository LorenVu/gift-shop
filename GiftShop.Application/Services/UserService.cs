using GiftShop.Application.Constrants.Responses;
using GiftShop.Application.Interfaces;
using GiftShop.Domain.Commons.Message;
using GiftShop.Domain.Enums;
using GiftShop.Infastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;

namespace GiftShop.Application.Services;

public class UserService(
    IUserRepository _userRepository, 
    ILogger<UserService> _logger) : IUserService
{
    public async Task<BaseResponse> GetUserInfo(string id)
    {
        var response = new BaseResponse();
        try
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                response.Status = (int)HttpStatusCode.BadRequest;
                response.Error = true;
                response.Message = CommonMessage.FAILED;
                response.ErrorCode = (int)EErrorCommon.INVALID_PARAMS;
            }
            else
            {
                response.Data = await _userRepository.GetUserInfo(id);
            }
        }
        catch(Exception ex)
        {
            _logger.LogError($"UserService|GetUserInfo|Error: {ex.Message}");
            response.Status = (int)HttpStatusCode.InternalServerError;
            response.Error = true;
            response.Message = CommonMessage.FAILED;
            response.ErrorCode = (int)EErrorCommon.IDENTITY_EXCEPTION;
        }

        return response;
    }
}
