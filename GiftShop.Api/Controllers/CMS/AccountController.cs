using GiftShop.Application.Constrants.Requests;
using GiftShop.Application.Interfaces;
using GiftShop.Application.Services;
using GiftShop.Domain.Commons.Helpers;
using GiftShop.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GiftShop.Api.Controllers.CMS
{
    [ApiController]
    [Route("api/v1/account")]
    public class AccountController(
        IAuthenService _authenService,
        IUserService _userService,
        ILogger<AccountController> _logger
        ) : Controller
    {

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<IActionResult> BasicLogin([FromBody] BasicLoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        errorCode = (short)EErrorCommon.INVALID_PARAMS
                    });
                }

                request.DeviceID = "222";
                //request.DeviceID = HttpContext.GetClientIP();
                var authResponse = await _authenService.BasicLogin(request);

                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"AuthenController|RegisterAccount|Error: {ex.Message}|Input: {JsonConvert.SerializeObject(request)}");
                //SentrySdk.CaptureException(ex);
                return BadRequest(new
                {
                    success = false,
                    errorCode = (short)EErrorCommon.API_EXCEPTION
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAccount([FromBody] RegisterAccountRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        errorCode = (short)EErrorCommon.INVALID_PARAMS
                    });
                }

                request.DeviceID = "222";
                //request.DeviceID = HttpContext.GetClientIP();
                var authResponse = await _authenService.RegisterAccount(request);

                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"AuthenController|RegisterAccount|Error: {ex.Message}|Input: {JsonConvert.SerializeObject(request)}");
                //SentrySdk.CaptureException(ex);
                return BadRequest(new
                {
                    success = false,
                    errorCode = (short)EErrorCommon.API_EXCEPTION
                });
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("me")]
        public async Task<IActionResult> GetInFo()
        {
            return Ok(await _userService.GetUserInfo(HttpHelpers.UserID));
        }
    }
}
