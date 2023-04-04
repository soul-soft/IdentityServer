using Hosting.Configuration;
using IdentityServer;
using IdentityServer.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication()
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
builder.Services.AddIdentityServer(configureOptions =>
    {
        //o.Endpoints.PathPrefix = "/oauth2";
        configureOptions.Issuer = "https://www.example.com";
    })
    .AddResourceOwnerCredentialRequestValidator<ResourceOwnerCredentialRequestValidator>()
    .AddExtensionGrantValidator<MyExtensionGrantValidator>()
    .AddProfileService<ProfileService>()
    .AddInMemoryStore(store =>
    {
        store.AddClients(Config.Clients);
        store.AddResources(Config.Resources);
        store.AddSigningCredentials(new X509Certificate2("idsvr.pfx", "nbjc"));
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

app.UseIdentityServer();

app.UseAuthentication();

app.UseAuthorization();

app.MapDefaultControllerRoute()
    .RequireAuthorization("default");
app.Run();
