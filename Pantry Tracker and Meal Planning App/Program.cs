using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vahallan_Ingredient_Aggregator.Data;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;
using Vahallan_Ingredient_Aggregator.Services.Implementaions;
using Vahallan_Ingredient_Aggregator.Models;
using Vahallan_Ingredient_Aggregator.Services.Implementations;
using Microsoft.Extensions.Options;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;
using Vahallan_Ingredient_Aggregator.Models.Photo;
using Vahallan_Ingredient_Aggregator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configure the SQL Server Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()  // This enables role management
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    // Admin policy
    options.AddPolicy("RequireAdmin", policy =>
    policy.RequireRole("Admin"));

// Recipe owner policy
//options.AddPolicy("RecipeOwnerOrAdmin", policy =>
//    policy.RequireAssertion(context =>
//    {
//        var user = context.User;
//        var recipeId = context.Resource as int?; // You'll need to pass this in

//        return user.IsInRole("Admin") ||
//               (recipeId.HasValue && IsRecipeOwner(user, recipeId.Value));
//    }));

// Premium user policy example
options.AddPolicy("PremiumUser", policy =>
    policy.RequireAssertion(context =>
        context.User.HasClaim(c =>
            c.Type == "SubscriptionType" &&
            c.Value == "Premium")));
});





// Add this to your Program.cs file where you configure services
builder.Services.AddHttpClient();

// Add PhotoStorage and Processing Services
builder.Services.Configure<PhotoStorageSettings>(
    builder.Configuration.GetSection("AppSettings:PhotoStorageSettings"));

builder.Services.AddScoped<IPhotoStorageService, LocalPhotoStorageService>();
builder.Services.AddScoped<IPhotoProcessingService, PhotoProcessingService>();

// Register application services
builder.Services.AddScoped<IMeasurementConversionService, MeasurementConversionService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IPhotoProcessingService, PhotoProcessingService>();

builder.Services.AddScoped<IImageDownloaderService, ImageDownloaderService>();

// Configure a named HttpClient for image downloading
builder.Services.AddHttpClient("ImageDownloader", client => {
    // Configure default headers
    client.DefaultRequestHeaders.Add("User-Agent", "ShoppingListGenerator");
    // Default timeout
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
await AdminInitializer.InitializeAsync(app.Services);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();