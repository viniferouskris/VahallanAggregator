// File: Services/Implementations/RecipeService.cs

using Microsoft.EntityFrameworkCore;
using Vahallan_Ingredient_Aggregator.Data;
using Vahallan_Ingredient_Aggregator.Models.Components;
using Vahallan_Ingredient_Aggregator.Models.External;
using Vahallan_Ingredient_Aggregator.Models.Photo;
using Vahallan_Ingredient_Aggregator.Models.ViewModels;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;
using Vahallan_Ingredient_Aggregator.Models.Components;
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



        // New utility method to download an image from a URL
        private async Task<Stream> DownloadImageAsync(string imageUrl)
    {
        try
        {
            using var httpClient = new HttpClient();
            // Set a timeout to prevent hanging
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            // Download the image as a byte array
            var imageData = await httpClient.GetByteArrayAsync(imageUrl);

            // Return as a memory stream
            return new MemoryStream(imageData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error downloading image from URL: {imageUrl}");
            throw;
        }
    }

    // Modified ImportExternalRecipeAsync method to include photo downloading
    public async Task<Recipe> ImportExternalRecipeAsync(
        MealDBResponse mealResponse,
        string userId,
        IPhotoStorageService photoStorageService,
        IPhotoProcessingService photoProcessingService)
    {
        // Check if there are any meals in the response
        if (mealResponse?.Meals == null || !mealResponse.Meals.Any())
        {
            throw new InvalidOperationException("No meals found in the response.");
        }

        var meal = mealResponse.Meals.First(); // Get the first meal

        if (await IsRecipeImportedAsync(meal.IdMeal))
        {
            throw new InvalidOperationException($"Recipe {meal.StrMeal} is already imported.");
        }

          _logger.LogInformation($"Importing recipe: {meal.StrMeal} (ID: {meal.IdMeal})");


            var recipe = new Recipe
        {
            Name = meal.StrMeal,
            Description = $"Category: {meal.StrCategory}, Area: {meal.StrArea}",
            Instructions = meal.StrInstructions,
            ExternalId = meal.IdMeal,
            ExternalSource = "TheMealDB",
            CreatedById = userId,
            IsPublic = true,
            Type = "Recipe",
            Unit = "serving", // Set a default value
            StoredUnit = "serving", // Set a default value
            Quantity = 1, // Set a default value
            StoredQuantity = 1, // Set a default value
            NumberOfServings = 4, // Set a default value
            PrepTimeMinutes = 30, // Set a reasonable default
            CookTimeMinutes = 30, // Set a reasonable default
            Photos = new List<RecipePhoto>(),
            RecipeIngredients = new List<RecipeIngredient>()
        };

        // Add ingredients using RecipeIngredient relationships
        var ingredients = meal.GetIngredientPairs()
            .Where(pair => !string.IsNullOrWhiteSpace(pair.Ingredient));

        foreach (var (ingredientName, measurementText) in ingredients)
        {
            // Create or get the ingredient
            var existingIngredient = await _context.Ingredients
                .FirstOrDefaultAsync(i => i.Name.ToLower() == ingredientName.ToLower());

            // Parse the measurement text into quantity and unit
            (decimal quantity, string unit) = ParseMeasurement(measurementText);

            if (existingIngredient == null)
            {
                // If unit is still empty after parsing, use a default
                if (string.IsNullOrWhiteSpace(unit))
                {
                    unit = "count";
                }

                existingIngredient = new Ingredient
                {
                    Name = ingredientName,
                    CreatedById = userId,
                    CreatedAt = DateTime.UtcNow,
                    Type = "Ingredient",
                    // Set default values for required fields
                    CostPerPackage = 0, // Default value
                    ServingsPerPackage = 1, // Default value
                    CaloriesPerServing = 0, // Default value
                    Quantity = 1, // Default value
                    Unit = unit, // Use the parsed unit
                    StoredQuantity = 1,
                    StoredUnit = unit
                };

                _context.Ingredients.Add(existingIngredient);
                await _context.SaveChangesAsync(); // Save to get the Id
            }

            // Create the recipe-ingredient relationship
            var recipeIngredient = new RecipeIngredient
            {
                Recipe = recipe,
                Ingredient = existingIngredient,
                Quantity = quantity, // Use the parsed quantity
                Unit = unit // Use the parsed unit
            };

            recipe.RecipeIngredients.Add(recipeIngredient);
        }

        // Save the recipe first to get its ID
        await CreateRecipeAsync(recipe, userId);

        // Now download and add the photo from TheMealDB if available
        if (!string.IsNullOrEmpty(meal.StrMealThumb))
        {
            try
            {
                _logger.LogInformation($"Downloading photo from URL: {meal.StrMealThumb}");

                // Download the image from TheMealDB
                using var imageStream = await DownloadImageAsync(meal.StrMealThumb);

                // Convert the stream to a FormFile for compatibility with your existing services
                var fileName = $"theMealDB_{meal.IdMeal}.jpg";
                    // Create a form file from the stream using our utility
                    var formFile = Utilities.StreamToFormFileConverter.ConvertToFormFile(
                        imageStream,
                        fileName,
                        "image/jpeg");

                    // Use the existing photo storage service to save the photo
                    var photoUrl = await photoStorageService.SavePhotoAsync(formFile, userId);

                    // Create a reusable copy of the stream for thumbnail generation
                    using var thumbnailStream = await Utilities.StreamToFormFileConverter.CreateReusableStreamAsync(imageStream);
                    var thumbnailFormFile = Utilities.StreamToFormFileConverter.ConvertToFormFile(
                        thumbnailStream,
                        fileName,
                        "image/jpeg");


                    // Generate a thumbnail using the photo processing service
                    var thumbnailData = await photoProcessingService.CreateThumbnailAsync(formFile);
                var thumbnailUrl = await photoStorageService.SaveThumbnailAsync(thumbnailData, photoUrl);

                // Create a photo record
                var recipePhoto = new RecipePhoto
                {
                    RecipeId = recipe.Id,
                    FilePath = photoUrl,
                    ThumbnailPath = thumbnailUrl,
                    ContentType = "image/jpeg",
                    FileSize = imageStream.Length,
                    FileName = fileName,
                    Description = $"Photo for {recipe.Name} imported from TheMealDB",
                    IsMain = true, // Mark as main photo
                    IsApproved = true,
                    UploadedById = userId,
                    UploadedAt = DateTime.UtcNow
                };

                // Add the photo to the recipe
                recipe.Photos.Add(recipePhoto);

                // Update the recipe with the new photo
                await UpdateRecipeAsync(recipe);

                _logger.LogInformation($"Successfully added TheMealDB photo to recipe {recipe.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error downloading TheMealDB photo for recipe {recipe.Id}. The recipe was created but without the photo.");
                // Continue without the photo - don't fail the whole import
            }
        }

        return recipe;
    }
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

        public async Task<IEnumerable<RecipeViewModel>> GetAllRecipesAsync(string userId, bool isAdmin)
        {
            try
            {
                _logger.LogInformation($"Retrieving recipes for user: {userId}, isAdmin: {isAdmin}");

                IQueryable<Recipe> query = _context.Recipes
                    .Include(r => r.RecipeIngredients);

                // If not admin, only show public recipes and user's own recipes
                if (!isAdmin)
                {
                    query = query.Where(r => r.IsPublic || r.CreatedById == userId);
                }

                var recipes = await query.ToListAsync();

                var viewModels = recipes.Select(r => new RecipeViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    PrepTime = r.PrepTimeMinutes,
                    CookTime = r.CookTimeMinutes,
                    TotalTime = r.PrepTimeMinutes + r.CookTimeMinutes,
                    IsPublic = r.IsPublic,
                    IsOwner = r.CreatedById == userId,
                    CreatedBy = r.CreatedById
                }).ToList();

                _logger.LogInformation($"Retrieved {viewModels.Count} recipes");
                return viewModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving recipes");
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


        public async Task<List<RecipeViewModel>> GetUserRecipesAsync(string userId)
        {
            return await _context.Recipes
                .Where(r => r.CreatedById == userId)
                .Select(r => new RecipeViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    PrepTime = r.PrepTimeMinutes,
                    CookTime = r.CookTimeMinutes,
                    TotalTime = r.PrepTimeMinutes + r.CookTimeMinutes,
                    IsPublic = r.IsPublic,
                    IsOwner = true,
                    CreatedBy = r.CreatedById,
                    IngredientsCount = r.RecipeIngredients.Count
                })
                .ToListAsync();
        }

        public async Task<List<RecipeViewModel>> GetSharedRecipesAsync(string userId)
        {
            return await _context.Recipes
                .Where(r => r.CreatedById != userId &&
                           r.IsPublic &&
                           r.ExternalSource == null)
                .Select(r => new RecipeViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    PrepTime = r.PrepTimeMinutes,
                    CookTime = r.CookTimeMinutes,
                    TotalTime = r.PrepTimeMinutes + r.CookTimeMinutes,
                    IsPublic = r.IsPublic,
                    IsOwner = false,
                    CreatedBy = r.CreatedById,
                    IngredientsCount = r.RecipeIngredients.Count
                })
                .ToListAsync();
        }

        public async Task<List<ExternalRecipeViewModel>> GetExternalRecipesAsync()
        {
            var externalRecipes = await _context.Recipes
                .Where(r => r.ExternalSource != null)
                .Select(r => new ExternalRecipeViewModel
                {
                    ExternalId = r.ExternalId,
                    Name = r.Name,
                    Description = r.Description,
                    //PrepTime = r.PrepTimeMinutes,
                    //CookTime = r.CookTimeMinutes,
                    //TotalTime = r.PrepTimeMinutes + r.CookTimeMinutes,
                    //IsPublic = true,
                    //IsOwner = false,
                    //CreatedBy = r.CreatedById,
                    //IngredientsCount = r.Components.Count,
                    //ExternalSource = r.ExternalSource,
                    //ExternalId = r.ExternalId
                })
                .ToListAsync();

            return new List<ExternalRecipeViewModel>();
        }

        public async Task<bool> IsRecipeImportedAsync(string externalId)
        {
            return await _context.Recipes
                .AnyAsync(r => r.ExternalId == externalId);
        }
        //public async Task<Recipe> ImportExternalRecipeAsync(MealDBResponse mealResponse, string userId)
        //{
        //    // Check if there are any meals in the response
        //    if (mealResponse?.Meals == null || !mealResponse.Meals.Any())
        //    {
        //        throw new InvalidOperationException("No meals found in the response.");
        //    }

        //    var meal = mealResponse.Meals.First(); // Get the first meal

        //    if (await IsRecipeImportedAsync(meal.IdMeal))
        //    {
        //        throw new InvalidOperationException($"Recipe {meal.StrMeal} is already imported.");
        //    }

        //    var recipe = new Recipe
        //    {
        //        Name = meal.StrMeal,
        //        Description = $"Category: {meal.StrCategory}, Area: {meal.StrArea}",
        //        Instructions = meal.StrInstructions,
        //        ExternalId = meal.IdMeal,
        //        ExternalSource = "TheMealDB",
        //        CreatedById = userId,
        //        IsPublic = true,
        //        Type = "Recipe",
        //        Unit = "serving", // Set a default value
        //        StoredUnit = "serving", // Set a default value
        //        Quantity = 1, // Set a default value
        //        StoredQuantity = 1, // Set a default value
        //        NumberOfServings = 4, // Set a default value
        //        PrepTimeMinutes = 30, // Set a reasonable default
        //        CookTimeMinutes = 30, // Set a reasonable default
        //        Photos = new List<RecipePhoto>(),
        //        RecipeIngredients = new List<RecipeIngredient>()
        //    };

        //    // Add ingredients using RecipeIngredient relationships
        //    var ingredients = meal.GetIngredientPairs()
        //        .Where(pair => !string.IsNullOrWhiteSpace(pair.Ingredient));

        //    foreach (var (ingredientName, measurementText) in ingredients)
        //    {
        //        // Create or get the ingredient
        //        var existingIngredient = await _context.Ingredients
        //            .FirstOrDefaultAsync(i => i.Name.ToLower() == ingredientName.ToLower());

        //        // Parse the measurement text into quantity and unit
        //        (decimal quantity, string unit) = ParseMeasurement(measurementText);

        //        if (existingIngredient == null)
        //        {
        //            // If unit is still empty after parsing, use a default
        //            if (string.IsNullOrWhiteSpace(unit))
        //            {
        //                unit = "count";
        //            }

        //            existingIngredient = new Ingredient
        //            {
        //                Name = ingredientName,
        //                CreatedById = userId,
        //                CreatedAt = DateTime.UtcNow,
        //                Type = "Ingredient",
        //                // Set default values for required fields
        //                CostPerPackage = 0, // Default value
        //                ServingsPerPackage = 1, // Default value
        //                CaloriesPerServing = 0, // Default value
        //                Quantity = 1, // Default value
        //                Unit = unit, // Use the parsed unit
        //                StoredQuantity = 1,
        //                StoredUnit = unit
        //            };

        //            _context.Ingredients.Add(existingIngredient);
        //            await _context.SaveChangesAsync(); // Save to get the Id
        //        }

        //        // Create the recipe-ingredient relationship
        //        var recipeIngredient = new RecipeIngredient
        //        {
        //            Recipe = recipe,
        //            Ingredient = existingIngredient,
        //            Quantity = quantity, // Use the parsed quantity
        //            Unit = unit // Use the parsed unit
        //        };

        //        recipe.RecipeIngredients.Add(recipeIngredient);
        //    }

        //    await CreateRecipeAsync(recipe, userId);
        //    return recipe;
        //}

        /// <summary>
        /// Parses a measurement string like "1 cup" or "3 cloves" into quantity and unit
        /// </summary>
        /// <param name="measurement">The measurement string to parse</param>
        /// <returns>A tuple with (quantity, unit)</returns>
        private (decimal quantity, string unit) ParseMeasurement(string measurement)
        {
            if (string.IsNullOrWhiteSpace(measurement))
            {
                return (1, "count"); // Default values if no measurement
            }

            // Remove leading/trailing spaces and convert to lowercase for easier parsing
            measurement = measurement.Trim().ToLower();

            // Handle special cases like "to taste", "pinch", etc.
            if (measurement.Contains("to taste") || measurement == "to taste")
            {
                return (1, "to taste");
            }
            if (measurement.Contains("pinch") || measurement == "pinch")
            {
                return (1, "pinch");
            }

            // Handle dash-separated ranges like "1-2 cups" - just take the average
            if (measurement.Contains("-"))
            {
                var parts = measurement.Split('-');
                if (parts.Length == 2)
                {
                    var firstNumStr = new string(parts[0].Where(c => char.IsDigit(c) || c == '.').ToArray());
                    var remaining = parts[1].Trim();

                    if (decimal.TryParse(firstNumStr, out decimal firstNum))
                    {
                        var secondNumEndIdx = 0;
                        while (secondNumEndIdx < remaining.Length && (char.IsDigit(remaining[secondNumEndIdx]) || remaining[secondNumEndIdx] == '.'))
                        {
                            secondNumEndIdx++;
                        }

                        var secondNumStr = remaining.Substring(0, secondNumEndIdx);
                        if (decimal.TryParse(secondNumStr, out decimal secondNum))
                        {
                            var avgNum = (firstNum + secondNum) / 2;
                            var unit = remaining.Substring(secondNumEndIdx).Trim();
                            return (avgNum, unit);
                        }
                    }
                }
            }

            // Handle fraction expressions like "1/2 cup" or mixed fractions like "1 1/2 cups"
            if (measurement.Contains("/"))
            {
                var parts = measurement.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                decimal quantity = 0;
                int startUnitIndex = 0;

                if (parts.Length >= 1 && parts[0].Contains("/"))
                {
                    // Simple fraction like "1/2 cup"
                    var fracParts = parts[0].Split('/');
                    if (fracParts.Length == 2 && decimal.TryParse(fracParts[0], out decimal num) &&
                        decimal.TryParse(fracParts[1], out decimal denom) && denom != 0)
                    {
                        quantity = num / denom;
                        startUnitIndex = 1;
                    }
                }
                else if (parts.Length >= 2 && parts[1].Contains("/"))
                {
                    // Mixed number like "1 1/2 cups"
                    if (decimal.TryParse(parts[0], out decimal whole))
                    {
                        var fracParts = parts[1].Split('/');
                        if (fracParts.Length == 2 && decimal.TryParse(fracParts[0], out decimal num) &&
                            decimal.TryParse(fracParts[1], out decimal denom) && denom != 0)
                        {
                            quantity = whole + (num / denom);
                            startUnitIndex = 2;
                        }
                    }
                }

                if (quantity > 0 && startUnitIndex < parts.Length)
                {
                    // Join the remaining parts as the unit
                    string unit = string.Join(" ", parts.Skip(startUnitIndex));
                    // Make unit singular if it's plural and quantity is 1
                    if (quantity == 1 && unit.EndsWith("s") && !unit.EndsWith("ss"))
                    {
                        unit = unit.TrimEnd('s');
                    }
                    return (quantity, unit);
                }
            }

            // Try to parse a standard format like "1 cup" or "2 tablespoons"
            var match = System.Text.RegularExpressions.Regex.Match(
                measurement,
                @"^([\d.]+)\s+(.+)$"
            );

            if (match.Success)
            {
                if (decimal.TryParse(match.Groups[1].Value, out decimal qty))
                {
                    var unit = match.Groups[2].Value.Trim();
                    // Make unit singular if it's plural and quantity is 1
                    if (qty == 1 && unit.EndsWith("s") && !unit.EndsWith("ss"))
                    {
                        unit = unit.TrimEnd('s');
                    }
                    return (qty, unit);
                }
            }

            // Handle text-based numbers like "one", "two", etc.
            var textNumbers = new Dictionary<string, decimal>
    {
        {"one", 1}, {"two", 2}, {"three", 3}, {"four", 4}, {"five", 5},
        {"six", 6}, {"seven", 7}, {"eight", 8}, {"nine", 9}, {"ten", 10},
        {"half", 0.5M}, {"quarter", 0.25M}, {"handful", 1}, {"some", 1}
    };

            foreach (var textNum in textNumbers)
            {
                if (measurement.StartsWith(textNum.Key + " "))
                {
                    var unit = measurement.Substring(textNum.Key.Length).Trim();
                    return (textNum.Value, unit);
                }
            }

            // If we can't parse the quantity, treat the whole measurement as the unit
            return (1, measurement);
        }
        //public async Task<Recipe> ImportExternalRecipeAsync(MealDBResponse mealResponse, string userId)
        //{
        //    // Check if there are any meals in the response
        //    if (mealResponse?.Meals == null || !mealResponse.Meals.Any())
        //    {
        //        throw new InvalidOperationException("No meals found in the response.");
        //    }

        //    var meal = mealResponse.Meals.First(); // Get the first meal

        //    if (await IsRecipeImportedAsync(meal.IdMeal))
        //    {
        //        throw new InvalidOperationException($"Recipe {meal.StrMeal} is already imported.");
        //    }

        //    var recipe = new Recipe
        //    {
        //        Name = meal.StrMeal,
        //        Description = $"Category: {meal.StrCategory}, Area: {meal.StrArea}",
        //        Instructions = meal.StrInstructions,
        //        ExternalId = meal.IdMeal,
        //        ExternalSource = "TheMealDB",
        //        CreatedById = userId,
        //        IsPublic = true,
        //        Type = "Recipe",
        //        Photos = new List<RecipePhoto>(),
        //        RecipeIngredients = new List<RecipeIngredient>()
        //    };

        //    // Add ingredients using RecipeIngredient relationships
        //    var ingredients = meal.GetIngredientPairs()
        //        .Where(pair => !string.IsNullOrWhiteSpace(pair.Ingredient));

        //    foreach (var (ingredient, measurement) in ingredients)
        //    {
        //        // Create or get the ingredient
        //        var existingIngredient = await _context.Ingredients
        //            .FirstOrDefaultAsync(i => i.Name.ToLower() == ingredient.ToLower());

        //        if (existingIngredient == null)
        //        {
        //            existingIngredient = new Ingredient
        //            {
        //                Name = ingredient,
        //                CreatedById = userId,
        //                CreatedAt = DateTime.UtcNow,
        //                Type = "Ingredient",
        //                // Set default values for required fields
        //                CostPerPackage = 0, // You might want to set a different default
        //                ServingsPerPackage = 1,
        //                CaloriesPerServing = 0, // You might want to set a different default
        //               StoredQuantity = 1, // You might want to convert this
        //                StoredUnit = measurement // You might want to convert this
        //            };
        //            _context.Ingredients.Add(existingIngredient);
        //            await _context.SaveChangesAsync(); // Save to get the Id
        //        }

        //        // Create the recipe-ingredient relationship
        //        var recipeIngredient = new RecipeIngredient
        //        {
        //            Recipe = recipe,
        //            Ingredient = existingIngredient,
        //            Quantity = 1, // Default quantity, you might want to parse this from measurement
        //            Unit = measurement

        //        };

        //        recipe.RecipeIngredients.Add(recipeIngredient);
        //    }

        //    await CreateRecipeAsync(recipe, userId);
        //    return recipe;
        //}
        public async Task ToggleRecipeVisibilityAsync(int recipeId, string userId)
        {
            var recipe = await _context.Recipes
                .FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe == null)
            {
                throw new KeyNotFoundException($"Recipe with ID {recipeId} not found.");
            }

            if (recipe.CreatedById != userId)
            {
                throw new UnauthorizedAccessException("You don't have permission to modify this recipe.");
            }

            recipe.IsPublic = !recipe.IsPublic;
            recipe.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        ////////////external Recipe
        ///

        public async Task<int?> GetLocalRecipeIdByExternalIdAsync(string externalId)
        {
            var recipe = await _context.Recipes
                .FirstOrDefaultAsync(r => r.ExternalId == externalId);

            return recipe?.Id;
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

        Task<List<ExternalRecipeViewModel>> IRecipeService.GetExternalRecipeIdsAsync()
        {
            throw new NotImplementedException();
        }

        Task<List<RecipeViewModel>> IRecipeService.GetAllRecipesAsync(string userId, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public Task<Recipe> ImportExternalRecipeAsync(MealDBResponse meal, string userId)
        {
            throw new NotImplementedException();
        }
    }
}