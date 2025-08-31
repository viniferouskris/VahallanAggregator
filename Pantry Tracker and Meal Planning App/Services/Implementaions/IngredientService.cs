using Microsoft.EntityFrameworkCore;
using Vahallan_Ingredient_Aggregator.Data;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;
using Vahallan_Ingredient_Aggregator.Models.Components;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Vahallan_Ingredient_Aggregator.Models.Photo;

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

            // SIMPLIFIED: Return all ingredients for admin, only user's ingredients for regular users
            return ingredients.Where(ingredient =>
                isAdmin || ingredient.CreatedById == userId)
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
            return isAdmin || ingredient.CreatedById == userId;
        }

        // REMOVED: CanCopyIngredientAsync method since IsSystemIngredient is gone

        // SIMPLIFIED: CreatePersonalCopyAsync - now just creates a copy without system ingredient logic
        public async Task<Ingredient> CreatePersonalCopyAsync(int sourceIngredientId, string userId)
        {
            var sourceIngredient = await GetIngredientAsync(sourceIngredientId);

            var personalCopy = new Ingredient
            {
                Name = sourceIngredient.Name,
                Unit = sourceIngredient.Unit,
                CostPerPackage = sourceIngredient.CostPerPackage,
                ServingsPerPackage = sourceIngredient.ServingsPerPackage,
                CaloriesPerServing = sourceIngredient.CaloriesPerServing,
                MaterialType = sourceIngredient.MaterialType,
                Vendor = sourceIngredient.Vendor,
                CreatedById = userId,
                SystemIngredientId = sourceIngredientId
                // REMOVED: IsSystemIngredient = false
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
            // Process photos if any were uploaded
            if (photos != null && photos.Any())
            {
                _logger.LogInformation($"Starting to process {photos.Count} photos for ingredient {ingredient.Id}");
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
                        _logger.LogInformation($"Added photo record to ingredient: {recipePhoto.FileName}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error processing photo {photo.FileName} for ingredient {ingredient.Id}");
                    }
                }

                _logger.LogInformation($"Updated ingredient {ingredient.Id} with {ingredient.Photos.Count} photos");
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

                // REMOVED: IsSystemIngredient permission checks

                // Update properties
                existingIngredient.Name = updatedIngredient.Name;
                existingIngredient.Unit = updatedIngredient.Unit;
                existingIngredient.CostPerPackage = updatedIngredient.CostPerPackage;
                existingIngredient.ServingsPerPackage = updatedIngredient.ServingsPerPackage;
                existingIngredient.CaloriesPerServing = updatedIngredient.CaloriesPerServing;
                existingIngredient.MaterialType = updatedIngredient.MaterialType;
                existingIngredient.Vendor = updatedIngredient.Vendor;

                // REMOVED: IsSystemIngredient assignment

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

                // REMOVED: IsSystemIngredient permission check

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