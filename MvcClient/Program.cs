using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(configureOptions => 
{
    configureOptions.DefaultScheme = "Cookie";
    configureOptions.DefaultChallengeScheme = "Oidc";
})
.AddCookie("Cookie")
.AddOpenIdConnect("Oidc", configureOptions => 
{
    configureOptions.Authority = "https://localhost:7150";
    configureOptions.ClientId = "client1";
    configureOptions.ClientSecret = "secret";
    configureOptions.ResponseType = "code";
    configureOptions.Scope.Add("api");
    configureOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters() 
    {
        ValidateAudience=false
    };
    configureOptions.SaveTokens = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    

app.Run();
