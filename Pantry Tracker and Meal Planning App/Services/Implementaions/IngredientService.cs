using Microsoft.EntityFrameworkCore;
using Vahallan_Ingredient_Aggregator.Data;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;
using Vahallan_Ingredient_Aggregator.Models.Components;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Vahallan_Ingredient_Aggregator.Models.Photo;

//Place functions that handle CRUD operations
//Methods that interact with the database
//Operations that involve multiple ingredients
//Business logic that goes beyond a single ingredient
//Data access and persistence logic


//Calculate an ingredient's price history -> IngredientService (involves database queries)
//Check if an ingredient is on sale -> Ingredient model (just uses the ingredient's own properties)
//Find similar ingredients -> IngredientService (requires querying other ingredients)
//Convert units -> Ingredient model (operates on the ingredient's own properties)


namespace Vahallan_Ingredient_Aggregator.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMeasurementConversionService _conversionService;
        private readonly ILogger<IngredientService> _logger;
        private readonly IPhotoStorageService _photoStorageService;
        private readonly IPhotoProcessingService _photoProcessingService;

        public IngredientService(
            ApplicationDbContext context,
            IMeasurementConversionService conversionService,
            ILogger<IngredientService> logger,
        IPhotoStorageService photoStorageService,
        IPhotoProcessingService photoProcessingService)
        {
            _context = context;
            _conversionService = conversionService;
            _logger = logger;
            _photoStorageService = photoStorageService;
            _photoProcessingService = photoProcessingService;
        }

        public async Task<Ingredient> GetIngredientAsync(int id)
        {
            _logger.LogInformation($"Attempting to find ingredient with ID: {id}");

            try
            {
                var ingredient = await _context.Set<BaseIngredientComponent>()
                    .Where(i => i.Type == "Ingredient" && i.Id == id)
                    .Include(r => r.Photos)
                    .FirstOrDefaultAsync();

                if (ingredient == null)
                {
                    _logger.LogWarning($"No ingredient found with ID: {id}");

                    // Log existing ingredients
                    var allIngredients = await _context.Set<BaseIngredientComponent>()
                        .Where(i => i.Type == "Ingredient")
                        .ToListAsync();

                    _logger.LogInformation($"Current ingredients in database: {string.Join(", ", allIngredients.Select(i => $"ID:{i.Id}, Name:{i.Name}"))}");

                    throw new KeyNotFoundException($"Ingredient with ID {id} not found");
                }

                _logger.LogInformation($"Found ingredient: ID={ingredient.Id}, Name={ingredient.Name} with {ingredient?.Photos.Count ?? 0} photos");
                return (Ingredient)ingredient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving ingredient with ID {id}");
                throw;
            }
        }
        public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
        {
            try
            {
                var ingredients = await _context.Set<BaseIngredientComponent>()
                    .Where(i => i.Type == "Ingredient")
                    //added .include 2/17
                    .Include(r => r.Photos)
                    .Select(i => (Ingredient)i)
                    .ToListAsync();

                _logger.LogInformation($"Retrieved {ingredients.Count} ingredients from database");
                return ingredients;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all ingredients");
                throw;
            }
        }
        public async Task<IEnumerable<Ingredient>> GetFilteredIngredientsAsync(string userId, bool isAdmin)
        {
            var ingredients = await GetAllIngredientsAsync();

            // Get imported system ingredients
            var importedSystemIds = ingredients
                .Where(i => i.CreatedById == userId && i.SystemIngredientId.HasValue)
                .Select(i => i.SystemIngredientId.Value)
                .ToHashSet();

            return ingredients.Where(ingredient =>
                isAdmin ||
                ingredient.CreatedById == userId ||
                (ingredient.IsSystemIngredient && !importedSystemIds.Contains(ingredient.Id)))
                .ToList();
        }
        public async Task<bool> CanEditIngredientAsync(int ingredientId, string userId, bool isAdmin)
        {
            var ingredient = await GetIngredientAsync(ingredientId);
            return isAdmin || ingredient.CreatedById == userId;
        }

        public async Task<bool> CanDeleteIngredientAsync(int ingredientId, string userId, bool isAdmin)
        {
            var ingredient = await GetIngredientAsync(ingredientId);
            return isAdmin || (ingredient.CreatedById == userId && !ingredient.IsSystemIngredient);
        }

        //public async Task<bool> CanCopyIngredientAsync(int ingredientId, string userId)
        //{
        //    var ingredient = await GetIngredientAsync(ingredientId);
        //    return ingredient.IsSystemIngredient && ingredient.CreatedById != userId;
        //}
        // IngredientService.cs
        public async Task<Ingredient> CreatePersonalCopyAsync(int systemIngredientId, string userId)
        {
            var systemIngredient = await GetIngredientAsync(systemIngredientId);
            if (!systemIngredient.IsSystemIngredient)
            {
                throw new InvalidOperationException("Source ingredient is not a system ingredient");
            }

            var personalCopy = new Ingredient
            {
                Name = systemIngredient.Name,
                Unit = systemIngredient.Unit,
                CostPerPackage = systemIngredient.CostPerPackage,
                ServingsPerPackage = systemIngredient.ServingsPerPackage,
                CaloriesPerServing = systemIngredient.CaloriesPerServing,
                CreatedById = userId,
                IsSystemIngredient = false,
                SystemIngredientId = systemIngredientId
            };

            return await CreateIngredientAsync(personalCopy);
        }


        public async Task<Ingredient> CreateIngredientAsync(Ingredient ingredient)
        {
            ingredient.Type = "Ingredient";
            ingredient.CreatedAt = DateTime.UtcNow;
            ingredient.StoredQuantity = ingredient.Quantity;
            ingredient.StoredUnit = ingredient.Unit;

            _context.Set<BaseIngredientComponent>().Add(ingredient);
            await _context.SaveChangesAsync();
            return ingredient;
        }

        public async Task AddPhotosToIngredientAsync(Ingredient ingredient, IFormFileCollection photos, string userId)
        {
            //    var userId = User.Identity?.Name ?? "system";

            // Process photos if any were uploaded
            if (photos != null && photos.Any())
            {
                _logger.LogInformation($"Starting to process {photos.Count} photos for recipe {ingredient.Id}");
                var processedPhotos = new HashSet<string>(); // Track processed filenames

                foreach (var photo in photos)
            {
                try
                {
                    // Check if we've already processed this photo
                    var photoKey = $"{photo.FileName}-{photo.Length}";
                    if (processedPhotos.Contains(photoKey))
                    {
                        _logger.LogWarning($"Skipping duplicate photo: {photoKey}");
                        continue;
                    }

                    _logger.LogInformation($"Processing photo: {photo.FileName}, Size: {photo.Length} bytes");

                    // Create file paths
                    var fileName = Path.GetFileName(photo.FileName);
                    _logger.LogInformation($"Generated fileName: {fileName}");

                    // Upload photo using IPhotoStorageService
                    var photoUrl = await _photoStorageService.SavePhotoAsync(photo, userId);
                    _logger.LogInformation($"Saved photo to URL: {photoUrl}");

                    var thumbnailData = await _photoProcessingService.CreateThumbnailAsync(photo);
                    var thumbnailUrl = await _photoStorageService.SaveThumbnailAsync(thumbnailData, photoUrl);
                    _logger.LogInformation($"Saved thumbnail to URL: {thumbnailUrl}");

                    // Create photo record
                    var recipePhoto = new RecipePhoto
                    {
                        RecipeId = ingredient.Id,
                        FilePath = photoUrl,
                        ThumbnailPath = thumbnailUrl,
                        ContentType = photo.ContentType,
                        FileSize = photo.Length,
                        FileName = Path.GetFileName(photoUrl),
                        Description = $"Photo for {ingredient.Name}",
                        IsMain = !ingredient.Photos.Any(), // First photo becomes main
                        IsApproved = true,
                        UploadedById = userId,
                        UploadedAt = DateTime.UtcNow
                    };

                    ingredient.Photos.Add(recipePhoto);
                    processedPhotos.Add(photoKey); // Mark as processed
                    _logger.LogInformation($"Added photo record to recipe: {recipePhoto.FileName}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing photo {photo.FileName} for recipe {ingredient.Id}");
                }
            }

            // Update recipe with photos
            //  await _ingredientService.UpdateIngredientAsync(ingredient);
            _logger.LogInformation($"Updated recipe {ingredient.Id} with {ingredient.Photos.Count} photos");
        }

    }

        public async Task<OperationResult> UpdateIngredientAsync(int id, Ingredient updatedIngredient, string userId, bool isAdmin)
{
    try
    {
        var existingIngredient = await GetIngredientAsync(id);
        
        // Check permissions
        if (!isAdmin && existingIngredient.CreatedById != userId)
        {
            return new OperationResult 
            { 
                Success = false, 
                Message = "You don't have permission to edit this ingredient" 
            };
        }

        // Only admin can change system ingredient status
        if (!isAdmin && updatedIngredient.IsSystemIngredient != existingIngredient.IsSystemIngredient)
        {
            return new OperationResult 
            { 
                Success = false, 
                Message = "Only administrators can modify system ingredient status" 
            };
        }

        // Update properties
        existingIngredient.Name = updatedIngredient.Name;
        existingIngredient.Unit = updatedIngredient.Unit;
        existingIngredient.CostPerPackage = updatedIngredient.CostPerPackage;
        existingIngredient.ServingsPerPackage = updatedIngredient.ServingsPerPackage;
        existingIngredient.CaloriesPerServing = updatedIngredient.CaloriesPerServing;
        
        // Only admin can change system ingredient status
        if (isAdmin)
        {
            existingIngredient.IsSystemIngredient = updatedIngredient.IsSystemIngredient;
        }

        existingIngredient.ModifiedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return new OperationResult { Success = true };
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, $"Error updating ingredient {id}");
        return new OperationResult 
        { 
            Success = false, 
            Message = "An error occurred while updating the ingredient" 
        };
    }
}

        public async Task<OperationResult> DeleteIngredientAsync(int id, string userId, bool isAdmin)
{
    try
    {
        var ingredient = await GetIngredientAsync(id);

        // Check permissions
        if (!isAdmin && ingredient.CreatedById != userId)
        {
            return new OperationResult 
            { 
                Success = false, 
                Message = "You don't have permission to delete this ingredient" 
            };
        }

        // Don't allow non-admin users to delete system ingredients
        if (!isAdmin && ingredient.IsSystemIngredient)
        {
            return new OperationResult 
            { 
                Success = false, 
                Message = "System ingredients cannot be deleted by non-admin users" 
            };
        }

        _context.Set<BaseIngredientComponent>().Remove(ingredient);
        await _context.SaveChangesAsync();
        
        return new OperationResult { Success = true };
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, $"Error deleting ingredient {id}");
        return new OperationResult 
        { 
            Success = false, 
            Message = "An error occurred while deleting the ingredient" 
        };
    }
}

     
    }
}