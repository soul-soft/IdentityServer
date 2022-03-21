using Hosting.Configuration;
using IdentityServer;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLoaclApiAuthentication();
builder.Services.AddAuthorization()
    .AddAuthorization(configure =>
    {
        configure.AddPolicy("default", p => p.RequireAuthenticatedUser());
    });
builder.Services.AddIdentityServer(o =>
    {
        o.IssuerUri = "https://www.example.com";
    })
    .AddResourceOwnerCredentialRequestValidator<ResourceOwnerCredentialRequestValidator>()
    .AddExtensionGrantValidator<MyExtensionGrantValidator>()
    .AddProfileService<ProfileService>()
    .AddInMemoryStores(setup =>
    {
        setup.AddClients(Config.Clients);
        setup.AddResources(Config.Resources);
        setup.AddSigningCredentials(new X509Certificate2("idsvr.pfx","nbjc"));
        //setup.AddDeveloperSigningCredentials();
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseIdentityServer();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization("default");
app.Run();
