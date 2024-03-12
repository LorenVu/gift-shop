using GiftShop.Application;
using GiftShop.Domain.Commons.Extentions.Service;
using GiftShop.Domain.Commons.Helpers;
using GiftShop.Domain.Entities;
using GiftShop.Infastructure;
using GiftShop.Infastructure.DataAccess;
using GiftShop.Infastructure.Interfaces;
using GiftShop.Infastructure.Mapper;
using GiftShop.Infastructure.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers().AddJsonOptions(
                options => options.JsonSerializerOptions.PropertyNamingPolicy = null); ;
builder.Services.AddEndpointsApiExplorer();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerConfig();
}

var sentryDsn = builder.Configuration.GetValue<string>("Sentry:Dsn");
if (!string.IsNullOrEmpty(sentryDsn))
{
    builder.WebHost.UseSentry(sentryDsn);
}

builder.Services.AddHttpClient();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAuthenticationConfig(builder.Configuration);
builder.Services.AddSignalR();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
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

builder.Services.AddAutoMapper(typeof(MapProfile));


builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();

var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
if (!string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddHealthChecks()
        .AddSqlServer(connectionString, timeout: TimeSpan.FromSeconds(5));
}


var app = builder.Build();
HttpHelpers.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true)
        .AllowCredentials());

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chat-hub");

app.Run();
