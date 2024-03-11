using GiftShop.Application.Constrants.Requests;
using GiftShop.Application.Interfaces;
using GiftShop.Application.Services;
using GiftShop.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GiftShop.Api.Controllers.CMS
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/account")]
    public class AccountController(
        IAuthenService _authenService,
        ILogger<AccountController> _logger
        ) : Controller
    {

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

                request.DeviceID = HttpContext.GetClientIP();
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
    }
}
