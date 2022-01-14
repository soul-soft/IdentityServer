using Hosting.Configuration;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddIdentityServer()
        .AddExtensionGrantValidator<MyExtensionGrantValidator>()
        .AddResourceOwnerPasswordGrantValidator<ResourceOwnerPasswordGrantValidator>()
        .AddInMemoryStores(setup =>
        {
            setup.AddClients(Config.Clients);
            setup.AddResources(Config.ApiScopes);
            setup.AddResources(Config.IdentityResources);
            setup.AddSigningCredentials(new X509Certificate2("idsvr.pfx","nbjc"));
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

app.UseIdentityServer();

app.MapControllers();

app.Run();
