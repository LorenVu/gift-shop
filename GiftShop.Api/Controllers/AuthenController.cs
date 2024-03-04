using GiftShop.Application.Constrants.Requests;
using GiftShop.Application.Constrants.Responses;
using GiftShop.Application.Services;
using GiftShop.Domain.Common.Message;
using GiftShop.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftShop.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("/api/v1/authen")]
public class AuthenController(
    AuthenService _authenService, 
    ILogger<AuthenController> _logger
    ) : Controller
{
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(BasicLoginRequest request)
    {
        var response = new AuthenticationResponse();

        if(!ModelState.IsValid)
        {
            response.ErrorCode = (int)EErrorCommon.INVALID_PARAMS;
            response.ErrorMessage = AuthenMessage.INPUT_INVALID;
            return BadRequest(response);
        }

        try
        {
            response = await _authenService.BasicLogin(request);
        }
        catch (Exception ex)
        {
            response.ErrorCode = (int)EErrorCommon.INVALID_PARAMS;
            response.ErrorMessage = AuthenMessage.INPUT_INVALID;

            _logger.LogError($"AuthenController|SignIn|Error: {ex.Message}");
        }

        return Ok(response);
    }
}
