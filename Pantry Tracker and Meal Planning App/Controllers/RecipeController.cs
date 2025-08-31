using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;
using Vahallan_Ingredient_Aggregator.Models.DTOs;
using Vahallan_Ingredient_Aggregator.Models.ViewModels;
using Vahallan_Ingredient_Aggregator.Services.Implementations;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;
using Vahallan_Ingredient_Aggregator.Models.External;
using Vahallan_Ingredient_Aggregator.Models.Photo;
using Microsoft.SqlServer.Server;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.VisualBasic;
using Vahallan_Ingredient_Aggregator.Models.Components;


namespace Vahallan_Ingredient_Aggregator.Controllers
{
    [Authorize]
    public class RecipeController : Controller
    {
        private readonly IRecipeService _recipeService;
        private readonly IIngredientService _ingredientService;
        private readonly ITheMealDBService _mealDbService;
        private readonly ILogger<RecipeController> _logger;
        private readonly IPhotoStorageService _photoStorageService;
        private readonly IPhotoProcessingService _photoProcessingService;


        public RecipeController(
            IRecipeService recipeService,
            ILogger<RecipeController> logger,
            ITheMealDBService mealDbService,
            IIngredientService ingredientService,
            IPhotoStorageService photoStorageService,
            IPhotoProcessingService photoProcessingService)
        {
            _recipeService = recipeService;
            _ingredientService = ingredientService;
            _mealDbService = mealDbService;
            _logger = logger;
            _photoStorageService = photoStorageService;
            _photoProcessingService = photoProcessingService;
        }
        // Only admins can access this
        [Authorize(Roles = "Admin")]
        //public async Task<IActionResult> ManageAll()
        //{
        //    var recipes = await _recipeService.GetAllRecipesAsync(User.Identity.Name, true);
        //    return View(recipes);
        //}

        [Authorize(Policy = "RequireAdmin")]
        public IActionResult AdminDashboard()
        {
            return View();
        }

        [Authorize(Policy = "PremiumUser")]
        public IActionResult PremiumFeatures()
        {
            return View();
        }

        public async Task<IActionResult> Index(string tab = "user")
        {
            try
            {
                var userId = User.Identity?.Name;
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var viewModel = new RecipeIndexViewModel
                {
                    ActiveTab = tab
                };

                // Load data based on active tab
                switch (tab.ToLower())
                {
                    case "user":
                        viewModel.UserRecipes = (await _recipeService.GetUserRecipesAsync(userId)).ToList();
                        break;

                    case "shared":
                        viewModel.SharedRecipes = (await _recipeService.GetSharedRecipesAsync(userId)).ToList();
                        break;

                    case "external":
                        viewModel.ExternalRecipes = (await _recipeService.GetExternalRecipesAsync()).ToList();
                        break;

                    default:
                        viewModel.UserRecipes = (await _recipeService.GetUserRecipesAsync(userId)).ToList();
                        break;
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Recipe Index action");
                TempData["Error"] = "An error occurred while loading recipes.";
                return View(new RecipeIndexViewModel());
            }
        }

        //public async Task<IActionResult> Index()
        //{
        //    try
        //    {
        //        var userId = User.Identity?.Name;
        //        var isAdmin = User.IsInRole("Admin");

        //        _logger.LogInformation($"User {userId} accessing recipes. IsAdmin: {isAdmin}");

        //        if (userId == null)
        //        {
        //            _logger.LogWarning("User ID is null");
        //            return RedirectToAction("Login", "Account");
        //        }

        //        var recipes = await _recipeService.GetAllRecipesAsync(userId, isAdmin);

        //        _logger.LogInformation($"Retrieved {recipes.Count()} recipes");
        //        return View(recipes);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error in Recipe Index action");
        //        TempData["Error"] = "An error occurred while loading recipes.";
        //        return View(Enumerable.Empty<RecipeViewModel>());
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> ToggleVisibility(int id)
        {
            try
            {
                await _recipeService.ToggleRecipeVisibilityAsync(id, User.Identity.Name);
                return Json(new { success = true });
            }
            catch (UnauthorizedAccessException)
            {
                return Json(new { success = false, message = "You don't have permission to modify this recipe." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while updating the recipe." });
            }
        }


        // GET: Recipe/Create
        // Owner or admin required
        [Authorize]

        public async Task<IActionResult> Create()
        {
            try
            {
                // Get all available ingredients for the dropdown
                var ingredients = await _ingredientService.GetAllIngredientsAsync();
                ViewBag.AvailableIngredients = ingredients.Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.Name} ({i.Unit})"  // Include unit in the display text
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
                // Log the error
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Recipe/Create
        // Owner or admin required
        //[Authorize]


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([FromForm] Recipe recipe, [FromForm] string ingredientsJson)
        //{
        //    var photos = Request.Form.Files;
        //    _logger.LogInformation($"Starting recipe creation at {DateTime.UtcNow:HH:mm:ss.fff}");
        //    _logger.LogInformation($"Number of photos received: {photos?.Count ?? 0}");

        //    // Log the request content type
        //    _logger.LogInformation($"Request Content-Type: {Request.ContentType}");

        //    try
        //    {
        //        if (string.IsNullOrEmpty(ingredientsJson))
        //        {
        //            return Json(new { success = false, message = "At least one ingredient is required." });
        //        }

        //        // Save the recipe first to get its ID
        //        var userId = User.Identity?.Name ?? "system";
        //        recipe.CreatedById = userId;
        //        recipe.CreatedAt = DateTime.UtcNow;
        //        var createdRecipe = await _recipeService.CreateRecipeAsync(recipe, userId);
        //        _logger.LogInformation($"Created recipe with ID: {createdRecipe.Id}");

        //        // Process ingredients
        //        var ingredients = JsonSerializer.Deserialize<List<RecipeIngredientDto>>(
        //            ingredientsJson.Replace("\\\"", "\"").Trim('"'),
        //            new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true,
        //                NumberHandling = JsonNumberHandling.AllowReadingFromString
        //            });

        //        foreach (var ingredientDto in ingredients)
        //        {
        //            var sourceIngredient = await _ingredientService.GetIngredientAsync(ingredientDto.IngredientId);
        //            var component = new Ingredient
        //            {
        //                Name = sourceIngredient.Name,
        //                Quantity = ingredientDto.Quantity,
        //                Unit = ingredientDto.Unit,
        //                StoredUnit = sourceIngredient.Unit,
        //                CreatedById = userId,
        //                CreatedAt = DateTime.UtcNow
        //            };
        //            recipe.Add(component);
        //        }

        //        // Process photos if any were uploaded
        //        if (photos != null && photos.Any())
        //        {
        //            _logger.LogInformation($"Starting to process {photos.Count} photos for recipe {createdRecipe.Id}");
        //            var processedPhotos = new HashSet<string>(); // Track processed filenames

        //            foreach (var photo in photos)
        //            {
        //                try
        //                {
        //                    // Check if we've already processed this photo
        //                    var photoKey = $"{photo.FileName}-{photo.Length}";
        //                    if (processedPhotos.Contains(photoKey))
        //                    {
        //                        _logger.LogWarning($"Skipping duplicate photo: {photoKey}");
        //                        continue;
        //                    }

        //                    _logger.LogInformation($"Processing photo: {photo.FileName}, Size: {photo.Length} bytes");

        //                    // Create file paths
        //                    var fileName = Path.GetFileName(photo.FileName);
        //                    _logger.LogInformation($"Generated fileName: {fileName}");

        //                    // Upload photo using IPhotoStorageService
        //                    var photoUrl = await _photoStorageService.SavePhotoAsync(photo, userId);
        //                    _logger.LogInformation($"Saved photo to URL: {photoUrl}");

        //                    var thumbnailData = await _photoProcessingService.CreateThumbnailAsync(photo);
        //                    var thumbnailUrl = await _photoStorageService.SaveThumbnailAsync(thumbnailData, photoUrl);
        //                    _logger.LogInformation($"Saved thumbnail to URL: {thumbnailUrl}");

        //                    // Create photo record
        //                    var recipePhoto = new RecipePhoto
        //                    {
        //                        RecipeId = createdRecipe.Id,
        //                        FilePath = photoUrl,
        //                        ThumbnailPath = thumbnailUrl,
        //                        ContentType = photo.ContentType,
        //                        FileSize = photo.Length,
        //                        FileName = Path.GetFileName(photoUrl),
        //                        Description = $"Photo for {recipe.Name}",
        //                        IsMain = !createdRecipe.Photos.Any(), // First photo becomes main
        //                        IsApproved = true,
        //                        UploadedById = userId,
        //                        UploadedAt = DateTime.UtcNow
        //                    };

        //                    createdRecipe.Photos.Add(recipePhoto);
        //                    processedPhotos.Add(photoKey); // Mark as processed
        //                    _logger.LogInformation($"Added photo record to recipe: {recipePhoto.FileName}");
        //                }
        //                catch (Exception ex)
        //                {
        //                    _logger.LogError(ex, $"Error processing photo {photo.FileName} for recipe {createdRecipe.Id}");
        //                }
        //            }

        //            // Update recipe with photos
        //            await _recipeService.UpdateRecipeAsync(createdRecipe);
        //            _logger.LogInformation($"Updated recipe {createdRecipe.Id} with {createdRecipe.Photos.Count} photos");
        //        }

        //        return Json(new
        //        {
        //            success = true,
        //            redirectUrl = Url.Action("Details", new { id = createdRecipe.Id })
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error creating recipe");
        //        return Json(new { success = false, message = "Error creating recipe" });
        //    }
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Recipe recipe, [FromForm] string ingredientsJson)
        {
            var photos = Request.Form.Files;
            _logger.LogInformation($"Starting recipe creation at {DateTime.UtcNow:HH:mm:ss.fff}");
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

                // Set base properties
                var userId = User.Identity?.Name ?? "system";
                recipe.CreatedById = userId;
                recipe.CreatedAt = DateTime.UtcNow;
                recipe.Type = "Recipe"; // Required for TPH inheritance

                // Add ingredients using RecipeIngredients
                foreach (var ingredientDto in ingredients)
                {
                    var sourceIngredient = await _ingredientService.GetIngredientAsync(ingredientDto.IngredientId);
                    var recipeIngredient = new RecipeIngredient
                    {
                        Recipe = recipe,
                        Ingredient = sourceIngredient,
                        Quantity = ingredientDto.Quantity,
                        Unit = ingredientDto.Unit,
                        //StoredQuantity = ingredientDto.Quantity, // You might want to convert this
                        //StoredUnit = ingredientDto.Unit // You might want to convert this
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

                // Check if user has access to this recipe
                if (!recipe.IsPublic && recipe.CreatedById != currentUserId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var viewModel = new RecipeDetailsViewModel
                {
                    Id = recipe.Id,
                    Name = recipe.Name ?? string.Empty,
                    Description = recipe.Description ?? string.Empty,
                    Instructions = recipe.Instructions ?? string.Empty,
                    PrepTimeMinutes = recipe.PrepTimeMinutes,
                    CookTimeMinutes = recipe.CookTimeMinutes,
                    IsPublic = recipe.IsPublic,
                    CreatedBy = recipe.CreatedById ?? string.Empty,
                    IsOwner = recipe.CreatedById == currentUserId,
                    NumberOfServings = recipe.NumberOfServings,
                    Ingredients = recipe.RecipeIngredients?
                        .Where(ri => ri?.Ingredient != null)
                        .Select(ri => new RecipeIngredientViewModel
                        {
                            Name = ri.Ingredient.Name ?? string.Empty,
                            Quantity = ri.Quantity,
                            Unit = ri.Unit ?? string.Empty
                        })
                        .ToList() ?? new List<RecipeIngredientViewModel>(),
                    Photos = recipe.Photos?.ToList() ?? new List<RecipePhoto>()
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

        // Add this method to RecipeController.cs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Attempting to delete recipe {id}");

                // Get the recipe first to check ownership
                var recipe = await _recipeService.GetRecipeByIdAsync(id);

                // Check if user has permission to delete this recipe
                if (recipe.CreatedById != User.Identity?.Name && !User.IsInRole("Admin"))
                {
                    _logger.LogWarning($"Unauthorized deletion attempt for recipe {id} by user {User.Identity?.Name}");
                    return Json(new { success = false, message = "You don't have permission to delete this recipe." });
                }

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

                _logger.LogInformation($"Successfully deleted recipe {id}");
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

       // [HttpGet]
//        public async Task<IActionResult> Details(int id)
//        {
////            Later, you can enhance this by:

////            Adding actual usage statistics tracking
////            Implementing recipe linking
////Adding price history tracking
////Including nutrition information
////Adding comparison with similar ingredients
//            try
//            {
//                _logger.LogInformation($"Loading recipe details for id: {id}");


//                var recipe = await _recipeService.GetRecipeByIdAsync(id);

//                // Debug log the photo count
//                _logger.LogInformation($"Recipe {id} has {recipe.Photos?.Count ?? 0} photos");

//                var currentUserId = User.Identity.Name;

//                // Check if user has access to this recipe
//                if (!recipe.IsPublic && recipe.CreatedById != currentUserId && !User.IsInRole("Admin"))
//                {
//                    return Forbid();
//                }

//                var viewModel = new RecipeDetailsViewModel
//                {
//                    Id = recipe.Id,
//                    Name = recipe.Name,
//                    Description = recipe.Description,
//                    Instructions = recipe.Instructions,
//                    PrepTimeMinutes = recipe.PrepTimeMinutes,
//                    CookTimeMinutes = recipe.CookTimeMinutes,
//                    IsPublic = recipe.IsPublic,
//                    CreatedBy = recipe.CreatedById,
//                    IsOwner = recipe.CreatedById == currentUserId,
//                    NumberOfServings = recipe.NumberOfServings,
//                    Ingredients = recipe.Components.Select(c => new RecipeIngredientViewModel
//                    {
//                        Name = c.Name,
//                        Quantity = (decimal)c.Quantity,
//                        Unit = c.Unit
//                    }).ToList(),
//                    Photos = recipe.Photos?.ToList() ?? new List<RecipePhoto>()

//                };

//                // Debug log the viewmodel photo count
//                _logger.LogInformation($"ViewModel has {viewModel.Photos?.Count ?? 0} photos");

//                _logger.LogInformation($"Photo paths:");
//                foreach (var photo in viewModel.Photos)
//                {
//                    _logger.LogInformation($"FilePath: {photo.FilePath}");
//                    _logger.LogInformation($"ThumbnailPath: {photo.ThumbnailPath}");
//                }
//                _logger.LogInformation("Photo paths in viewModel:");
//                foreach (var photo in viewModel.Photos)
//                {
//                    _logger.LogInformation($"FilePath: {photo.FilePath}, ThumbnailPath: {photo.ThumbnailPath}");
//                }
//                return View(viewModel);
//            }
//            catch (KeyNotFoundException)
//            {
//                return NotFound();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving recipe details");
//                return RedirectToAction("Error", "Home");
//            }
//        }

        ////////////////
        //////////////
        ///Exernal Recipe Handling
        ///
        /////////////////////
        /////////////////////
        ///



        [HttpGet]
        public async Task<IActionResult> GetRandomRecipes()
        {
            try
            {
                var results = new List<ExternalRecipeViewModel>();
                for (int i = 0; i < 6; i++) // Get 6 random recipes
                {
                    var meal = await _mealDbService.GetRandomMealAsync();
                    if (meal != null)
                    {
                        results.Add(new ExternalRecipeViewModel
                        {
                            ExternalId = meal.IdMeal,
                            Name = meal.StrMeal,
                            Category = meal.StrCategory,
                            Area = meal.StrArea,
                            ThumbnailUrl = meal.StrMealThumb,
                            Description = $"Category: {meal.StrCategory}, Area: {meal.StrArea}",
                            Source = "TheMealDB",
                            IsImported = await _recipeService.IsRecipeImportedAsync(meal.IdMeal)
                        });
                    }
                }

                return Json(new { results });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting random recipes");
                return Json(new { error = "Failed to load random recipes" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchExternal(string searchTerm)
        {
            try
            {
                var searchResults = await _mealDbService.SearchMealsAsync(searchTerm);
                var results = new List<ExternalRecipeViewModel>();

                foreach (var meal in searchResults)
                {
                    if (meal is MealDBMeal mealTyped)
                    {
                        var isImported = await _recipeService.IsRecipeImportedAsync(mealTyped.IdMeal);
                        var ingredients = new List<string>();
                        var measurements = new List<string>();

                        // Get all ingredients and measurements from meal properties
                        for (int i = 1; i <= 20; i++)
                        {
                            var ingredient = mealTyped.GetType().GetProperty($"StrIngredient{i}")?.GetValue(mealTyped) as string;
                            var measurement = mealTyped.GetType().GetProperty($"StrMeasure{i}")?.GetValue(mealTyped) as string;

                            if (!string.IsNullOrWhiteSpace(ingredient) && !string.IsNullOrWhiteSpace(measurement))
                            {
                                ingredients.Add(ingredient.Trim());
                                measurements.Add(measurement.Trim());
                            }
                        }

                        results.Add(new ExternalRecipeViewModel
                        {
                            ExternalId = mealTyped.IdMeal,
                            Name = mealTyped.StrMeal,
                            Category = mealTyped.StrCategory,
                            Area = mealTyped.StrArea,
                            ThumbnailUrl = mealTyped.StrMealThumb,
                           // Instructions = mealTyped.StrInstructions,
                            Description = $"Category: {mealTyped.StrCategory}, Area: {mealTyped.StrArea}",
                            Source = "TheMealDB",
                          //  Ingredients = ingredients,
                          //  Measurements = measurements,
                            IsImported = isImported
                        });
                    }
                }

                return Json(new { results });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching external recipes: {Error}", ex.Message);
                return Json(new { error = "Failed to search recipes" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportExternalRecipe([FromBody] ImportRecipeRequest request)
        {
            try
            {
                var meal = await _mealDbService.GetByIdAsync(request.Id);
                if (meal == null)
                {
                    return NotFound();
                }

                // Create a proper MealDBResponse object
                var mealResponse = new MealDBResponse
                {
                    Meals = new List<MealDBMeal> { meal }
                };

                var userId = User.Identity?.Name ?? "system";

                // Pass the photo services to the import method
                await _recipeService.ImportExternalRecipeAsync(
                    mealResponse,
                    userId,
                    _photoStorageService,
                    _photoProcessingService);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing external recipe");
                return StatusCode(500, new { error = "Failed to import recipe" });
            }
        }
        public class ImportRecipeRequest
        {
            public string Id { get; set; }
        }

    }
}