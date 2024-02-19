using GiftShop.Application.Constrants.Requests;
using GiftShop.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using GiftShop.Domain.Entities;

namespace GiftShop.Web.Controllers;

[AllowAnonymous]
public class AuthenController(
    UserManager<ApplicationUser> _userManager,
    IAuthenService _authenService, 
    ILogger<AuthenController> _logger, 
    IConfiguration _configuration) : Controller
{
    public async Task<IActionResult> SignIn(int ResponseCode)
    {
        await Task.CompletedTask;
        ViewData["ResponseCode"] = ResponseCode;
        ViewData["ClientId"] = _configuration.GetSection("Authentication:Google:ClientId").Value;
        return View();
    }

    public async Task<IActionResult> Register()
    {
        await Task.CompletedTask;
        return View();
    }

    public async Task<IActionResult> ForgotPassword()
    {
        await Task.CompletedTask;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
    {
        if (!ModelState.IsValid)
            return View(forgotPasswordModel);

        var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);

        if (user == null)
            return RedirectToAction(nameof(ForgotPasswordConfirmation));

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

        //var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);
        //await _emailSender.SendEmailAsync(message);

        return RedirectToAction(nameof(ForgotPasswordConfirmation));
    }

    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        var model = new ResetPasswordRequest { Token = token, Email = email };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordModel)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> BasicLogin(BasicLoginRequest request)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest();
        }

        return Ok(await _authenService.BasicLogin(request));
    }

    [AllowAnonymous]
    public async Task LoginGoogle()
    {
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
        {
            RedirectUri = Url.Action("GoogleResponse")
        });
    }

    [AllowAnonymous]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var email = result?.Principal?.Identities?
            .FirstOrDefault()?.Claims.Where(c => c.Type == ClaimTypes.Email)
               .Select(c => c.Value).SingleOrDefault();
        if (!string.IsNullOrEmpty(email))
        {
            var response = await _authenService.SocialNetWorkLogin(email, "123");

            if (response != null && response.Claims != null)
            {
                
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(response.Claims));

                return Redirect("https://localhost:7278/" + "Home/Index");
            }
        }

        return RedirectToAction("Login", "Authen", new { ResponseCode = -1 });
    }
}
