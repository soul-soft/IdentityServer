using IdentityServer.Hosting.DependencyInjection;
using Idp.IdentityServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//使用内置的cookie作为默认的质询方案
builder.Services.AddAuthentication(configureOptions =>
    {
        configureOptions.DefaultChallengeScheme = "Cookie";
    })
    .AddCookie("Cookie");
//添加授权策略
builder.Services.AddAuthorization(configure =>
{
    configure.AddPolicy("default", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});
//注册身份认证Idp
builder.Services.AddIdentityServer()
    .AddInMemoryClients(IdpResource.Clients);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseIdentityServer();

//启用内置认证
app.UseAuthentication();
//启用授权
app.UseAuthorization();

app.MapDefaultControllerRoute()
    //全局启用授权
    .RequireAuthorization("default");

app.Run();
