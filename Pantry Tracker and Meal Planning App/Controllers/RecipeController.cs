using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;
using Vahallan_Ingredient_Aggregator.Models.DTOs;
using Vahallan_Ingredient_Aggregator.Models.ViewModels;
using System.Text.Json;
using System.Text.Json.Serialization;
using Vahallan_Ingredient_Aggregator.Models.Photo;
using Vahallan_Ingredient_Aggregator.Models.Components;

namespace Vahallan_Ingredient_Aggregator.Controllers
{
    [Authorize]
    public class RecipeController : Controller
    {
        private readonly IRecipeService _recipeService;
        private readonly IIngredientService _ingredientService;
        private readonly ILogger<RecipeController> _logger;
        private readonly IPhotoStorageService _photoStorageService;
        private readonly IPhotoProcessingService _photoProcessingService;

        public RecipeController(
            IRecipeService recipeService,
            ILogger<RecipeController> logger,
            IIngredientService ingredientService,
            IPhotoStorageService photoStorageService,
            IPhotoProcessingService photoProcessingService)
        {
            _recipeService = recipeService;
            _ingredientService = ingredientService;
            _logger = logger;
            _photoStorageService = photoStorageService;
            _photoProcessingService = photoProcessingService;
        }

        // GET: Recipe - Show all public recipes to all users
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = User.Identity?.Name;
                var isAdmin = User.IsInRole("Admin");

                // Get all public recipes for everyone to see
                var recipes = await _recipeService.GetAllPublicRecipesAsync();

                ViewBag.IsAdmin = isAdmin;
                ViewBag.CanManageRecipes = isAdmin; // Only admins can manage
                ViewBag.CurrentUserId = userId;

                return View(recipes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving recipes");
                return View(new List<RecipeViewModel>());
            }
        }

        // GET: Recipe/Details/5 - All users can view details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                _logger.LogInformation($"Loading recipe details for id: {id}");

                var recipe = await _recipeService.GetRecipeByIdAsync(id);
                if (recipe == null)
                {
                    return NotFound();
                }

                var currentUserId = User.Identity?.Name;
                var isAdmin = User.IsInRole("Admin");

                // All recipes are public now - no access control needed
                var viewModel = new RecipeDetailsViewModel
                {
                    Id = recipe.Id,
                    Name = recipe.Name ?? string.Empty,
                    Description = recipe.Description ?? string.Empty,
                    Instructions = recipe.Instructions ?? string.Empty,
                    PrepTimeMinutes = recipe.PrepTimeMinutes,
                    CookTimeMinutes = recipe.CookTimeMinutes,
                    IsPublic = true, // Always true now
                    CreatedBy = recipe.CreatedById ?? string.Empty,
                    IsOwner = isAdmin, // Only admin can manage
                    NumberOfServings = recipe.NumberOfServings,
                    Collection = recipe.Collection,
                    ShowInIngredientsList = recipe.ShowInIngredientsList,
                    AccuracyLevel = recipe.AccuracyLevel,
                   // PatternCode = recipe.PatternCode,
                    StandardSquareFeet = recipe.StandardSquareFeet,
                    Ingredients = recipe.RecipeIngredients?
                        .Where(ri => ri?.Ingredient != null)
                        .Select(ri => new RecipeIngredientViewModel
                        {
                            Name = ri.Ingredient.Name ?? string.Empty,
                            Quantity = ri.Quantity,
                            Unit = ri.Unit ?? string.Empty,
                            MaterialType = ri.Ingredient.MaterialType,
                            Vendor = ri.Ingredient.Vendor
                        })
                        .ToList() ?? new List<RecipeIngredientViewModel>(),
                    Photos = recipe.Photos?.ToList() ?? new List<RecipePhoto>(),
                  //  CanEdit = isAdmin,
                 //   CanDelete = isAdmin
                };

                _logger.LogInformation($"ViewModel has {viewModel.Photos?.Count ?? 0} photos");
                return View(viewModel);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving recipe details");
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Recipe/Create - Admin only
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            try
            {
                // Get all available ingredients for the dropdown
                var ingredients = await _ingredientService.GetAllIngredientsAsync();
                ViewBag.AvailableIngredients = ingredients.Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.Name} ({i.Unit}) - {i.MaterialType}"  // Include material type
                }).ToList();

                // Pass units separately
                ViewBag.IngredientUnits = ingredients.ToDictionary(
                    i => i.Id,
                    i => i.Unit
                );

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading create recipe form");
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Recipe/Create - Admin only
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] Recipe recipe, [FromForm] string ingredientsJson)
        {
            var photos = Request.Form.Files;
            _logger.LogInformation($"Admin creating recipe at {DateTime.UtcNow:HH:mm:ss.fff}");
            _logger.LogInformation($"Number of photos received: {photos?.Count ?? 0}");

            try
            {
                if (string.IsNullOrEmpty(ingredientsJson))
                {
                    return Json(new { success = false, message = "At least one ingredient is required." });
                }

                // Parse ingredients from JSON
                var ingredients = JsonSerializer.Deserialize<List<RecipeIngredientDto>>(
                    ingredientsJson.Replace("\\\"", "\"").Trim('"'),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        NumberHandling = JsonNumberHandling.AllowReadingFromString
                    });

                // Set base properties - admin creates all recipes as public
                var userId = User.Identity?.Name ?? "admin";
                recipe.CreatedById = userId;
                recipe.CreatedAt = DateTime.UtcNow;
                recipe.Type = "Recipe"; // Required for TPH inheritance
                recipe.IsPublic = true; // All recipes are public in this system

                // Add ingredients using RecipeIngredients
                recipe.RecipeIngredients = new List<RecipeIngredient>();
                foreach (var ingredientDto in ingredients)
                {
                    var sourceIngredient = await _ingredientService.GetIngredientAsync(ingredientDto.IngredientId);
                    var recipeIngredient = new RecipeIngredient
                    {
                        Recipe = recipe,
                        Ingredient = sourceIngredient,
                        Quantity = ingredientDto.Quantity,
                        Unit = ingredientDto.Unit
                    };
                    recipe.RecipeIngredients.Add(recipeIngredient);
                }

                // Save recipe to get Id
                var createdRecipe = await _recipeService.CreateRecipeAsync(recipe, userId);

                // Process photos if any were uploaded
                if (photos != null && photos.Any())
                {
                    _logger.LogInformation($"Processing {photos.Count} photos for recipe {createdRecipe.Id}");
                    var processedPhotos = new HashSet<string>();

                    foreach (var photo in photos)
                    {
                        try
                        {
                            var photoKey = $"{photo.FileName}-{photo.Length}";
                            if (processedPhotos.Contains(photoKey))
                            {
                                _logger.LogWarning($"Skipping duplicate photo: {photoKey}");
                                continue;
                            }

                            // Save photo and create thumbnail
                            var photoUrl = await _photoStorageService.SavePhotoAsync(photo, userId);
                            var thumbnailData = await _photoProcessingService.CreateThumbnailAsync(photo);
                            var thumbnailUrl = await _photoStorageService.SaveThumbnailAsync(thumbnailData, photoUrl);

                            // Create photo record
                            var recipePhoto = new RecipePhoto
                            {
                                RecipeId = createdRecipe.Id,
                                FilePath = photoUrl,
                                ThumbnailPath = thumbnailUrl,
                                ContentType = photo.ContentType,
                                FileSize = photo.Length,
                                FileName = Path.GetFileName(photoUrl),
                                Description = $"Photo for {recipe.Name}",
                                IsMain = !createdRecipe.Photos.Any(), // First photo becomes main
                                IsApproved = true,
                                UploadedById = userId,
                                UploadedAt = DateTime.UtcNow
                            };

                            createdRecipe.Photos.Add(recipePhoto);
                            processedPhotos.Add(photoKey);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error processing photo {photo.FileName}");
                        }
                    }

                    // Update recipe with photos
                    await _recipeService.UpdateRecipeAsync(createdRecipe);
                }

                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("Details", new { id = createdRecipe.Id })
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating recipe");
                return Json(new { success = false, message = "Error creating recipe" });
            }
        }

        // DELETE: Recipe/Delete - Admin only
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Admin attempting to delete recipe {id}");

                // Get the recipe first
                var recipe = await _recipeService.GetRecipeByIdAsync(id);

                // Delete associated photos first
                if (recipe.Photos != null && recipe.Photos.Any())
                {
                    foreach (var photo in recipe.Photos)
                    {
                        try
                        {
                            // Delete the original photo
                            if (!string.IsNullOrEmpty(photo.FilePath))
                            {
                                await _photoStorageService.DeletePhotoAsync(photo.FilePath);
                            }

                            // Delete the thumbnail
                            if (!string.IsNullOrEmpty(photo.ThumbnailPath))
                            {
                                await _photoStorageService.DeletePhotoAsync(photo.ThumbnailPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log but continue with deletion
                            _logger.LogError(ex, $"Error deleting photo files for recipe {id}");
                        }
                    }
                }

                // Delete the recipe
                await _recipeService.DeleteRecipeAsync(id);

                _logger.LogInformation($"Admin successfully deleted recipe {id}");
                return Json(new { success = true });
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning($"Attempted to delete non-existent recipe {id}");
                return Json(new { success = false, message = "Recipe not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting recipe {id}");
                return Json(new { success = false, message = "An error occurred while deleting the recipe." });
            }
        }

        // Remove the old tab-based Index method and other user-specific methods
        // Remove ToggleVisibility method since all recipes are public now

        // Keep your existing AdminDashboard and other methods if needed
        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            return View();
        }
    }
}