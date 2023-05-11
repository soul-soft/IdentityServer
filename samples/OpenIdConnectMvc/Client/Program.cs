using System.IdentityModel.Tokens.Jwt;
using Client;
using Client.Apis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
//注册api资源
builder.Services.AddTransient<ApiDelegatingHandler>();
builder.Services.AddHttpClient<ApiClient>()
    .AddHttpMessageHandler<ApiDelegatingHandler>();
//添加认证方案
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(configureOptions =>
    {
        configureOptions.DefaultScheme = "Cookie";
        //使用idp作为默认的质询方案
        configureOptions.DefaultChallengeScheme = "Idp";
    })
    .AddCookie("Cookie", configureOptions =>
    {
        configureOptions.Cookie.Name = "Idc";
    })
    .AddOpenIdConnect("Idp", configureOptions =>
    {
        configureOptions.ClientId = "mvc";
        configureOptions.ClientSecret = "mvc";
        configureOptions.SaveTokens = true;
        configureOptions.ResponseMode = OpenIdConnectParameterNames.Code;
        configureOptions.Authority = "https://localhost:8080";
        configureOptions.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents()
        {
            //Idp退出之后，退出本地cookie登入
            OnSignedOutCallbackRedirect = async context =>
            {
                await context.HttpContext.SignOutAsync("Cookie");
            }
        };
    });
//添加授权策略
builder.Services.AddAuthorization(configure =>
{
    configure.AddPolicy("default", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapDefaultControllerRoute().RequireAuthorization("default");

app.Run("https://localhost:8081");
