using Hosting.Configuration;
using IdentityServer.EntityFramework;
using IdentityServer.Hosting.DependencyInjection;
using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var ff = "secret".Sha512();
builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication("Cookie")
    .AddCookie("Cookie", configureOptions =>
    {

    });

builder.Services.AddAuthorization()
    .AddAuthorization(configureOptions =>
    {
        configureOptions.AddPolicy("default", p => p.RequireAuthenticatedUser());
    });
builder.Services.AddStackExchangeRedisCache(c =>
{
    c.Configuration = "124.71.130.192,password=Juzhen88!";
});
builder.Services.AddDbContext<IdentityServerDbContext>(configureOptions => 
{
    var connectStr = "server=127.0.0.1;user id=root;password=1024;database=identity_server;connection timeout=180;";
    configureOptions.UseMySql(connectStr, ServerVersion.AutoDetect(connectStr), o => o.MigrationsAssembly("Hosting"));
});
builder.Services.AddIdentityServer(configureOptions =>
    {
        //o.Endpoints.PathPrefix = "/oauth2";
        configureOptions.Issuer = "https://www.example.com";
    })
    //.AddCacheStore()
    //.AddTokenStore()
    //.AddAuthorizationCodeStore()
    .AddExtensionGrantValidator<MyExtensionGrantValidator>()
    .AddResourceOwnerCredentialRequestValidator<ResourceOwnerCredentialRequestValidator>()
    .AddProfileService<ProfileService>()
    //.AddCacheStore()
    //.AddTokenStore()
    //.AddAuthorizationCodeStore()
    //.AddInMemoryClients(Config.Clients)
    //.AddInMemoryResources(Config.Resources)
    .AddInMemoryDeveloperSigningCredentials()
    .AddEntityFrameworkStore(configureOptions =>
    {
        configureOptions.TableNameToLower = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseDefaultFiles();
app.UseHttpsRedirection();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute().RequireAuthorization("default"); ;
});

app.Run();
