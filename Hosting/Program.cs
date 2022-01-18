using Hosting.Configuration;
using IdentityServer;
using IdentityServer.Endpoints;
using IdentityServer.Validation;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddTransient<IExtensionGrantValidator, MyExtensionGrantValidator>();
builder.Services.AddTransient<IUserInfoGenerator, UserInfoResponseGenerator>();
builder.Services.AddTransient<IPasswordGrantValidator, PasswordGrantValidator>();
builder.Services.AddIdentityServer(o =>
        {
            o.EmitScopesAsSpaceDelimitedStringInJwt = false;
        })
        .AddInMemoryStores(setup =>
        {
            setup.AddClients(Config.Clients);
            setup.AddResources(Config.ApiScopes);
            setup.AddResources(Config.IdentityResources);
            setup.AddSigningCredentials(new X509Certificate2("idsvr.pfx", "nbjc"));
            //setup.AddSigningCredentials(CryptoRandom.CreateRsaSecurityKey());
        });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseIdentityServer()
    .RequireCors("p", (n) => true);
app.MapControllers();
app.Run();
