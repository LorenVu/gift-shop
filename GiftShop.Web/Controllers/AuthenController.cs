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
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace GiftShop.Web.Controllers;

[AllowAnonymous]
public class AuthenController(
    UserManager<ApplicationUser> _userManager,
    ISendMailService _mailService,
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
        var callback = Url.Action(nameof(ResetPassword), "Authen", new { token, email = user.Email }, Request.Scheme);

        var message = new MailContent(user.Email, "Reset password token", callback);
        await _mailService.SendEmailAsync(message);

        return RedirectToAction(nameof(ForgotPasswordConfirmation));
    }

    public async Task<IActionResult> ForgotPasswordConfirmation()
    {
        await Task.CompletedTask;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ResetPassword(string token, string email)
    {
        await Task.CompletedTask;
        var model = new ResetPasswordRequest { Token = token, Email = email };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordModel)
    {
        if (!ModelState.IsValid)
            return View(resetPasswordModel);

        var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);

        if (user == null)
            RedirectToAction(nameof(ResetPasswordConfirmation));

        var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);

        if (!resetPassResult.Succeeded)
        {
            foreach (var error in resetPassResult.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return View();
        }

        return RedirectToAction(nameof(ResetPasswordConfirmation));
    }

    public async Task<IActionResult> ResetPasswordConfirmation()
    {
        await Task.CompletedTask;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> BasicLogin(BasicLoginRequest request)
    {
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
            .Select(c => c.Value)
            .SingleOrDefault();

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
