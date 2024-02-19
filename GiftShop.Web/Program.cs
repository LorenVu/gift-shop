using GiftShop.Application;
using GiftShop.Domain.Entities;
using GiftShop.Infastructure;
using GiftShop.Infastructure.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Config
builder.Services.AddControllersWithViews();
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = long.MaxValue;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = long.MaxValue;
});

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = long.MaxValue;
    o.MultipartBoundaryLengthLimit = int.MaxValue;
    o.MultipartHeadersCountLimit = int.MaxValue;
    o.MultipartHeadersLengthLimit = int.MaxValue;
    o.BufferBodyLengthLimit = long.MaxValue;
    o.BufferBody = true;
    o.ValueCountLimit = int.MaxValue;
});
#endregion

#region === Config google authen ===
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Authen/SignIn";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
})
.AddGoogle(options =>
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = googleAuthNSection["ClientId"];
    options.ClientSecret = googleAuthNSection["ClientSecret"];
});
#endregion


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();
builder.Services.AddServices();
builder.Services.AddRepositories();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 7;
    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase = false;
    opt.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use((context, next) =>
{
    context.Request.Scheme = "https";
    return next();
});

app.UseForwardedHeaders();

app.UseStaticFiles();

app.UseRouting();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
