using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SoleMates.Data;
using SoleMates.Helpers;
using SoleMates.Interfaces;
using SoleMates.Models;
using SoleMates.Repository;
using SoleMates.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IClubRepository, ClubRepository>();
builder.Services.AddScoped<IRaceRepository, RaceRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.Configure<RequestLocalizationOptions>(
    opt =>
    {
        var supportedCultures = new List<CultureInfo>
        {
            new CultureInfo("en"),
            new CultureInfo("tr")
        };
        opt.DefaultRequestCulture = new RequestCulture("en");
        opt.SupportedCultures = supportedCultures;
        opt.SupportedUICultures = supportedCultures;
    });
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    //options.UseInMemoryDatabase("SoleMates");
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<AppUser, IdentityRole>(o =>
{
    // configure identity options
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 3;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
builder.Services.AddMvc()
    .AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    await Seed.SeedUsersAndRolesAsync(app);
    //Seed.SeedData(app);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-7.0&tabs=visual-studio

app.MapGet("api/clubs", (ApplicationDbContext context) =>
{
    return Results.Ok(context.Clubs.
    Include(i => i.Address).
    Include(i => i.AppUser).
    Select(r => new
    {
        Id = r.Id,
        Title = r.Title,
        Description = r.Description,
        ImageUrl = r.Image,
        Street = r.Address.Street,
        City = r.Address.City,
        Category = r.ClubCategory,
        CreatorId = r.AppUser.Id,
        CreatedBy = r.AppUser.UserName,
        CreationDate = r.DateCreated,
        LastModificationDate = r.DateModified
    }));
});

app.MapGet("api/clubs/{city}", (string city, ApplicationDbContext context) =>
{
    var clubs = context.Clubs
        .Include(i => i.Address)
        .Include(i => i.AppUser)
        .Where(c => c.Address.City == city);
    if (clubs.Any())
    {
        return Results.Ok(clubs.Select(r => new
        {
            Id = r.Id,
            Title = r.Title,
            Description = r.Description,
            ImageUrl = r.Image,
            Street = r.Address.Street,
            City = r.Address.City,
            Category = r.ClubCategory,
            CreatorId = r.AppUser.Id,
            CreatedBy = r.AppUser.UserName,
            CreationDate = r.DateCreated,
            LastModificationDate = r.DateModified
        }));
    }
    return Results.NotFound();
});

app.MapGet("api/races", (ApplicationDbContext context) =>
{
    return Results.Ok(context.Races.
    Include(i => i.Address).
    Include(i => i.AppUser).
    Select(r => new
    {
        Id = r.Id,
        Title = r.Title,
        Description = r.Description,
        ImageUrl = r.Image,
        Street = r.Address.Street,
        City = r.Address.City,
        Category = r.RaceCategory,
        CreatorId = r.AppUser.Id,
        CreatedBy = r.AppUser.UserName,
        CreationDate = r.DateCreated,
        LastModificationDate = r.DateModified
    }));
});

app.MapGet("api/races/{city}", (string city, ApplicationDbContext context) =>
{
    var races = context.Races
        .Include(i => i.Address)
        .Include(i => i.AppUser)
        .Where(c => c.Address.City == city);
    if (races.Any())
    {
        return Results.Ok(races.Select(r => new
        {
            Id = r.Id,
            Title = r.Title,
            Description = r.Description,
            ImageUrl = r.Image,
            Street = r.Address.Street,
            City = r.Address.City,
            Category = r.RaceCategory,
            CreatorId = r.AppUser.Id,
            CreatedBy = r.AppUser.UserName,
            CreationDate = r.DateCreated,
            LastModificationDate = r.DateModified
        }));
    }
    return Results.NotFound();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

var options = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions >> ();

app.UseRequestLocalization(options.Value);

//var supportedCultures = new[] { "en", "tr" };
//var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
//    .AddSupportedCultures(supportedCultures)
//    .AddSupportedUICultures(supportedCultures);

//app.UseRequestLocalization(localizationOptions);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
