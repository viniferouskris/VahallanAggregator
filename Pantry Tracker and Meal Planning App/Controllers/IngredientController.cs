using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using Vahallan_Ingredient_Aggregator.Models.Components;
using Vahallan_Ingredient_Aggregator.Models.Photo;
using Vahallan_Ingredient_Aggregator.Models.ViewModels;
using Vahallan_Ingredient_Aggregator.Services.Implementations;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
//Handle HTTP requests
//Manage the view logic
//Convert between ViewModels and domain models
//Coordinate with services

[Authorize]
public class IngredientController : Controller
{
    private readonly IIngredientService _ingredientService;
    private readonly IMeasurementConversionService _measurementService;
    private readonly ILogger<IngredientController> _logger;
    private readonly IPhotoStorageService _photoStorageService;
    private readonly IPhotoProcessingService _photoProcessingService;

    public IngredientController(
        IIngredientService ingredientService,
        IMeasurementConversionService measurementService,
        ILogger<IngredientController> logger,
        IPhotoStorageService photoStorageService,
        IPhotoProcessingService photoProcessingService)
    {
        _ingredientService = ingredientService;
        _measurementService = measurementService;
        _logger = logger;
        _photoStorageService = photoStorageService;
        _photoProcessingService = photoProcessingService;
    }

    // GET: Ingredient
public async Task<IActionResult> Index()
{
    try
    {
        var userId = User.Identity?.Name;
        var isAdmin = User.IsInRole("Admin");
        var ingredients = await _ingredientService.GetAllIngredientsAsync();
        var viewModels = ingredients.Select(i => new IngredientViewModel
            {
                Id = i.Id,
                Name = i.Name,
            Unit = i.Unit,
            CostPerPackage = i.CostPerPackage,
            ServingsPerPackage = i.ServingsPerPackage,
            CaloriesPerServing = i.CaloriesPerServing,
            IsSystemIngredient = i.IsSystemIngredient,
            IsPromoted = i.IsPromoted,
            SystemIngredientId = i.SystemIngredientId
        }).ToList();

        ViewBag.IsAdmin = isAdmin;
        ViewBag.CurrentUserId = userId;
        return View(viewModels);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving ingredients");
        return View(new List<IngredientViewModel>());
    }
}
    public IActionResult Create()
    {
        var isAdmin = User.IsInRole("Admin");
        ViewBag.Units = new SelectList(IngredientViewModel.StandardUnits);
        ViewBag.IsAdmin = isAdmin;
        ViewBag.IsSystemIngredient = false; // Set default value


        var viewModel = new IngredientViewModel
        {
            // Set default values
            Quantity = 1, // This fixes the validation issue
            ServingsPerPackage = 1,
            CaloriesPerServing = 0
        };

        return View(viewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IngredientViewModel viewModel)
    {
         var userId = User.Identity?.Name ?? "system";

        try
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Convert ViewModel to Domain Model
            var ingredient = new Ingredient
            {
                Name = viewModel.Name,
                Unit = viewModel.Unit,
                Quantity = 1,
                CostPerPackage = viewModel.CostPerPackage,
                ServingsPerPackage = viewModel.ServingsPerPackage,
                CaloriesPerServing = viewModel.CaloriesPerServing,
                CreatedById = User.Identity.Name,
                IsSystemIngredient = viewModel.IsSystemIngredient && User.IsInRole("Admin")
            };

            // Let the service handle photo processing
            if (Request.Form.Files.Any())
            {
                await _ingredientService.AddPhotosToIngredientAsync(ingredient, Request.Form.Files,userId);
            }

            // Create the ingredient
            await _ingredientService.CreateIngredientAsync(ingredient);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ingredient");
            ModelState.AddModelError("", "Error creating ingredient");
            return View(viewModel);
        }
    }



    // GET: Ingredient/Details/5
    public async Task<IActionResult> Details(int? id)
    {
//        Later, you can enhance this by:

//        Adding actual usage statistics tracking
//        Implementing recipe linking
//Adding price history tracking
//Including nutrition information
//Adding comparison with similar ingredients
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var ingredient = await _ingredientService.GetIngredientAsync(id.Value);
            var currentUserId = User.Identity.Name;
            var isAdmin = User.IsInRole("Admin");

            // Check if user has permission to view
            if (!ingredient.IsSystemIngredient && !isAdmin && ingredient.CreatedById != currentUserId)
            {
                return Forbid();
            }

            var viewModel = new IngredientDetailsViewModel
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Unit = ingredient.Unit,
                CostPerPackage = ingredient.CostPerPackage,
                ServingsPerPackage = ingredient.ServingsPerPackage,
                CaloriesPerServing = ingredient.CaloriesPerServing,
                IsSystemIngredient = ingredient.IsSystemIngredient,
                CreatedBy = ingredient.CreatedById,
                CreatedAt = ingredient.CreatedAt,
                ModifiedAt = ingredient.ModifiedAt,
                SystemIngredientId = ingredient.SystemIngredientId,
                IsPromoted = ingredient.IsPromoted,

                // Set permissions
                CanEdit = isAdmin || ingredient.CreatedById == currentUserId,
                CanDelete = isAdmin || (ingredient.CreatedById == currentUserId && !ingredient.IsSystemIngredient),
                CanCopy = ingredient.IsSystemIngredient && ingredient.CreatedById != currentUserId,

                // TODO: Implement these when adding usage tracking
                RecipeCount = 0, // Replace with actual count
                LastUsed = null, // Replace with actual last used date
                AverageCostPerRecipe = 0, // Replace with actual average
                Photos = ingredient.Photos?.ToList() ?? new List<RecipePhoto>()

            };

            return View(viewModel);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving ingredient details for ID: {id}");
            return RedirectToAction("Error", "Home");
        }
    }
    // GET: Ingredient/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        try
        {
            var ingredient = await _ingredientService.GetIngredientAsync(id.Value);
            var currentUserId = User.Identity.Name;
            var isAdmin = User.IsInRole("Admin");

            // Check if user has permission to edit
            if (!isAdmin && ingredient.CreatedById != currentUserId && !ingredient.IsSystemIngredient)
            {
                return Forbid();
            }

            var viewModel = new IngredientViewModel
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Quantity = 1,
                Unit = ingredient.Unit,
                CostPerPackage = ingredient.CostPerPackage,
                ServingsPerPackage = ingredient.ServingsPerPackage,
                CaloriesPerServing = ingredient.CaloriesPerServing,
                IsSystemIngredient = ingredient.IsSystemIngredient,
                SystemIngredientId = ingredient.SystemIngredientId,
                Photos = ingredient.Photos?.ToList() ?? new List<RecipePhoto>() // Add this line
            };

            ViewBag.Units = new SelectList(IngredientViewModel.StandardUnits);
            ViewBag.IsAdmin = isAdmin;
            return View(viewModel);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, IngredientViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid && ModelState.ErrorCount == 1 && ModelState.ContainsKey("Quantity"))
        {
            ModelState.Remove("Quantity");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Units = new SelectList(IngredientViewModel.StandardUnits);
            ViewBag.IsAdmin = User.IsInRole("Admin");
            return View(viewModel);
        }

        try
        {
            var userId = User.Identity?.Name;
            var existingIngredient = await _ingredientService.GetIngredientAsync(id);

            // Update basic properties
            existingIngredient.Name = viewModel.Name;
            existingIngredient.Unit = viewModel.Unit;
            existingIngredient.CostPerPackage = viewModel.CostPerPackage;
            existingIngredient.ServingsPerPackage = viewModel.ServingsPerPackage;
            existingIngredient.CaloriesPerServing = viewModel.CaloriesPerServing;
            existingIngredient.IsSystemIngredient = viewModel.IsSystemIngredient;

            // Process new photos if any were uploaded
            if (Request.Form.Files.Any())
            {
                await _ingredientService.AddPhotosToIngredientAsync(existingIngredient, Request.Form.Files, userId);
            }

            var result = await _ingredientService.UpdateIngredientAsync(
                id,
                existingIngredient,
                userId,
                User.IsInRole("Admin")
            );

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                ViewBag.Units = new SelectList(IngredientViewModel.StandardUnits);
                ViewBag.IsAdmin = User.IsInRole("Admin");
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ingredient");
            ModelState.AddModelError("", "An error occurred while updating the ingredient");
            ViewBag.Units = new SelectList(IngredientViewModel.StandardUnits);
            ViewBag.IsAdmin = User.IsInRole("Admin");
            return View(viewModel);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePhoto([FromBody] PhotoDeleteRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest("Invalid request");
            }

            _logger.LogInformation($"Attempting to delete photo {request.PhotoId} from ingredient {request.IngredientId}");

            var ingredient = await _ingredientService.GetIngredientAsync(request.IngredientId);

            if (ingredient == null)
            {
                return NotFound("Ingredient not found");
            }

            var photo = ingredient.Photos?.FirstOrDefault(p => p.Id == request.PhotoId);

            if (photo == null)
            {
                return NotFound("Photo not found");
            }

            // Delete photo files
            try
            {
                if (!string.IsNullOrEmpty(photo.FilePath))
                {
                    await _photoStorageService.DeletePhotoAsync(photo.FilePath);
                }
                if (!string.IsNullOrEmpty(photo.ThumbnailPath))
                {
                    await _photoStorageService.DeletePhotoAsync(photo.ThumbnailPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting photo files");
            }

            // Remove photo from ingredient
            ingredient.Photos.Remove(photo);
            await _ingredientService.UpdateIngredientAsync(
                request.IngredientId,
                ingredient,
                User.Identity?.Name,
                User.IsInRole("Admin")
            );

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting photo");
            return Json(new { success = false, message = ex.Message });
        }
    }

    public class PhotoDeleteRequest
    {
        public int IngredientId { get; set; }
        public int PhotoId { get; set; }
    }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var result = await _ingredientService.DeleteIngredientAsync(
                id,
                User.Identity?.Name,
                User.IsInRole("Admin")
            );

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Details), new { id });
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ingredient");
            TempData["Error"] = "An error occurred while deleting the ingredient";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
    // GET: Ingredient/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var ingredient = await _ingredientService.GetIngredientAsync(id.Value);
            var currentUserId = User.Identity.Name;
            var isAdmin = User.IsInRole("Admin");

            // Check if user has permission to delete
            if (!isAdmin && ingredient.CreatedById != currentUserId)
            {
                return Forbid();
            }

            var viewModel = new IngredientViewModel
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Unit = ingredient.Unit,
                CostPerPackage = ingredient.CostPerPackage,
                ServingsPerPackage = ingredient.ServingsPerPackage,
                CaloriesPerServing = ingredient.CaloriesPerServing,
                IsSystemIngredient = ingredient.IsSystemIngredient
            };

            return View(viewModel);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }



    //// GET: Ingredient/CopyFromSystem/5
    //[HttpGet]
    //public async Task<IActionResult> CopyFromSystem(int id)
    //{
    //    try
    //    {
    //        var userId = User.Identity?.Name;
    //        if (!await _ingredientService.CanCopyIngredientAsync(id, userId))
    //        {
    //            return BadRequest("Cannot copy this ingredient");
    //        }

    //        var ingredient = await _ingredientService.GetIngredientAsync(id);
    //        var viewModel = new IngredientViewModel
    //        {
    //            // ... populate viewModel
    //        };

    //        return View("Create", viewModel);
    //    }
    //    catch (KeyNotFoundException)
    //    {
    //        return NotFound();
    //    }
    //}
}