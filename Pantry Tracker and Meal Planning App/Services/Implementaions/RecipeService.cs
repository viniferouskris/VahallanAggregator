// File: Services/Implementations/RecipeService.cs

using Microsoft.EntityFrameworkCore;
using Vahallan_Ingredient_Aggregator.Data;
using Vahallan_Ingredient_Aggregator.Models.Components;
using Vahallan_Ingredient_Aggregator.Models.Photo;
using Vahallan_Ingredient_Aggregator.Models.ViewModels;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;
using System.Net.Http;
using System.IO;

namespace Vahallan_Ingredient_Aggregator.Services.Implementations
{
    public class RecipeService : IRecipeService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RecipeService> _logger;
        private readonly IImageDownloaderService _imageDownloader;


        public RecipeService(ApplicationDbContext context, ILogger<RecipeService> logger, IImageDownloaderService imageDownloader)
        {
            _context = context;
            _logger = logger;
            _imageDownloader = imageDownloader;
        }
        // Add this to RecipeService.cs




    
    public async Task<Recipe> GetRecipeByIdAsync(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.Photos)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
            {
                throw new KeyNotFoundException($"Recipe with ID {id} not found");
            }

            _logger.LogInformation($"Found recipe with {recipe.Photos?.Count ?? 0} photos " +
                $"and {recipe.RecipeIngredients?.Count ?? 0} ingredients");

            return recipe;
        }
        // Add this method to your existing RecipeService class

        public async Task<IEnumerable<RecipeViewModel>> GetAllRecipesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving ALL recipes");

                var recipes = await _context.Recipes
                    .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                    .Include(r => r.Photos)
                    .ToListAsync(); // Get ALL recipes, no filtering

                var viewModels = recipes.Select(r => new RecipeViewModel
                {
                    Id = r.Id,
                    Name = r.Name ?? string.Empty,
                    Description = r.Description ?? string.Empty,

                    // Wallpaper-specific properties (map if they exist, default if they don't)
                    Collection = r.Collection ?? string.Empty,
                    StandardSheetSize = r.StandardSheetSize, // Make sure Recipe has this property
                    AccuracyLevel = (Models.ViewModels.RecipeAccuracyLevel)r.AccuracyLevel,

                    // Display properties
                    MainPhotoUrl = r.Photos?.FirstOrDefault(p => p.IsMain)?.FilePath
                                  ?? r.Photos?.FirstOrDefault()?.FilePath,
                    IngredientsCount = r.RecipeIngredients?.Count ?? 0,

                    // Simple properties
                    IsPublic = true, // All recipes are "public" now
                    IsOwner = false, // Will be determined in controller if needed
                    CreatedBy = r.CreatedById ?? string.Empty,

                    // Backward compatibility
                    PrepTime = r.PrepTimeMinutes,
                    CookTime = r.CookTimeMinutes,
                    TotalTime = r.PrepTimeMinutes + r.CookTimeMinutes,
                    ShowInIngredientsList = r.ShowInIngredientsList, // Add this line to your mapping




                }).ToList();

                _logger.LogInformation($"Retrieved {viewModels.Count} total recipes");
                return viewModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all recipes");
                throw;
            }
        }

        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            recipe.ModifiedAt = DateTime.UtcNow;
            _context.Entry(recipe).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecipeAsync(int id)
        {
            var recipe = await GetRecipeByIdAsync(id);
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RecipeExistsAsync(int id)
        {
            return await _context.Recipes.AnyAsync(r => r.Id == id);
        }


  

        public async Task<Recipe> CreateRecipeAsync(Recipe recipe, string userId)
        {
            try
            {
                recipe.CreatedById = userId;
                recipe.CreatedAt = DateTime.UtcNow;

                // Initialize photos collection if null
                if (recipe.Photos == null)
                {
                    recipe.Photos = new List<RecipePhoto>();
                }

                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Created recipe {recipe.Id} with {recipe.Photos.Count} photos");
                return recipe;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating recipe");
                throw;
            }
        }
        public Task<IEnumerable<string>> GetExternalRecipeIdsAsync()
        {
            throw new NotImplementedException();
        }

  




    }
}