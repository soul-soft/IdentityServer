//using Microsoft.AspNetCore.Http;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c => 
//{
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
//    {
//        Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.ApiKey,
//        BearerFormat = "JWT",
//        Scheme = "Bearer"
//    });
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme{
//                Reference = new OpenApiReference {
//                            Type = ReferenceType.SecurityScheme,
//                            Id = "Bearer"}
//            },new string[] { }
//        }
//    });
//});
//builder.Services.AddAuthentication("Bearer")
//.AddJwtBearer("Bearer", options =>
//{
//    options.Authority = "https://localhost:7150/";
//    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
//    {
//        OnMessageReceived = async (c) =>
//        {
//            await Task.CompletedTask;
//        }
//    };
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateAudience = false
//    };
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();
//    //.RequireAuthorization();

//app.Run();


using ApiHost;

var app = new MyApplicationBuilder();
app.UseMiddleware(async (c, next) => 
{
    Console.WriteLine("身份认证处理完成");
    await next();
});
app.UseMiddleware(async (c, next) =>
{
    Console.WriteLine("资源响应完成");
    await next();
});
var host = app.Build();

//模拟一次请求
var context1 = new CqrsContext();
await host(context1);
//模拟一次请求
var context2 = new CqrsContext();
await host(context2);

Console.ReadLine();