using IdentityProvider.IdentityServer;
using IdentityProvider.IdentityServer.Services;
using IdentityProvider.IdentityServer.Validators;
using IdentityServer.Hosting;
using IdentityServer.Hosting.DependencyInjection;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentityServer()
    .AddInMemoryClients(IdentityServerConfig.Clients)
    .AddInMemoryResources(IdentityServerConfig.Resources)
    .AddProfileService<ProfileService>()
    .AddInMemoryDeveloperSigningCredentials()
    .AddResourceOwnerCredentialRequestValidator<ResourceOwnerCredentialRequestValidator>();
//认证方案
builder.Services.AddAuthentication(configureOptions =>
{
    //使用identityserver作为默认的认证方案
    configureOptions.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
    //使用cookie作为默认的挑战方案
    configureOptions.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    //添加cookie认证方案
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    //添加identityserver认证方案
    .AddIdentityServer(IdentityServerAuthenticationDefaults.AuthenticationScheme);
//授权策略
builder.Services.AddAuthorization(configure =>
{
    configure.AddPolicy("default", p =>
    {
        p.RequireClaim(JwtClaimTypes.Subject);
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseIdentityServer();
app.UseHttpsRedirection();
//启用认证
app.UseAuthentication();
//启用授权
app.UseAuthorization();

app.MapDefaultControllerRoute()
    //为所有控制器添加默认的授权策略
    .RequireAuthorization("default");

app.Run();
