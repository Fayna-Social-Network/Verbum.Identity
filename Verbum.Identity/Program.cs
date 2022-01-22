using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Verbum.Identity;
using Verbum.Identity.Data;
using Verbum.Identity.Models;
using Verbum.Identity.Repositories;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager _configuration = builder.Configuration;
IWebHostEnvironment env = builder.Environment;


builder.Services.AddDbContext<AuthDbContext>(options =>
             options.UseNpgsql(_configuration.GetConnectionString("DatabaseContext")));

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(_configuration.GetConnectionString("VerbumDatabase")));


builder.Services.AddTransient<AuthRepository>();

builder.Services.AddIdentity<AppUser, IdentityRole>(config =>
{
    config.Password.RequiredLength = 6;
    config.Password.RequireDigit = true;
    config.Password.RequireUppercase = true;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireLowercase = true;
}
)
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddIdentityServer()
    .AddAspNetIdentity<AppUser>()
    .AddInMemoryApiResources(Configuration.ApiResources)
    .AddInMemoryIdentityResources(Configuration.IdentityResources)
    .AddInMemoryApiScopes(Configuration.ApiScopes)
    .AddInMemoryClients(Configuration.Clients)
    .AddDeveloperSigningCredential();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "Verbum.Identity.Cookie";
    config.LoginPath = "/Auth/Login";
    config.LogoutPath = "/Auth/Logout";
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

var app = builder.Build();


if (env.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions { 
    FileProvider = new PhysicalFileProvider(
        Path.Combine(env.ContentRootPath, "Styles")),
    RequestPath = "/styles"
});

app.UseRouting();

app.UseIdentityServer();


//app.UseAuthentication();
//app.UseAuthorization();
//app.UseHttpsRedirection();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});



//app.MapControllers();

app.Run();
