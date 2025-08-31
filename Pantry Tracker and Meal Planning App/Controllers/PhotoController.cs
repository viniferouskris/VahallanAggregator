using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vahallan_Ingredient_Aggregator.Data;
using Vahallan_Ingredient_Aggregator.Models.Photo;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;

namespace Vahallan_Ingredient_Aggregator.Controllers
{
    [Authorize]
    public class PhotoController : Controller
    {
        private readonly IPhotoStorageService _photoStorage;
        private readonly IPhotoProcessingService _photoProcessing;
        private readonly IRecipeService _recipeService;
        private readonly ILogger<PhotoController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PhotoController(
            IPhotoStorageService photoStorage,
            IPhotoProcessingService photoProcessing,
            IRecipeService recipeService,
            ILogger<PhotoController> logger,
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _photoStorage = photoStorage;
            _photoProcessing = photoProcessing;
            _recipeService = recipeService;
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet("TestPhotoAccess")]
        [Route("/Photo/TestPhotoAccess")]  // Add this explicit routeP
        public IActionResult TestPhotoAccess(int recipeId)
        {
            try
            {
                var photos = _context.RecipePhotos
            .Where(p => p.RecipeId == recipeId)
            .ToList();

                var rootPath = _webHostEnvironment.WebRootPath;

                var results = photos.Select(p => new
                {
                    Photo = p,
                    OriginalExists = System.IO.File.Exists(Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        p.FilePath.TrimStart('/'))),
                    ThumbnailExists = System.IO.File.Exists(Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        p.ThumbnailPath.TrimStart('/')))
                }).ToList();

                return Json(new { success = true, results });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error testing photo access");
                return Json(new { success = false, error = ex.Message });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile photo, int recipeId)
        {
            try
            {
                // Validate recipe ownership
                var recipe = await _recipeService.GetRecipeByIdAsync(recipeId);
                if (recipe.CreatedById != User.Identity.Name)
                {
                    return Forbid();
                }

                if (photo == null || photo.Length == 0)
                {
                    return BadRequest("No photo was uploaded.");
                }

                // Validate photo
                if (!_photoStorage.ValidatePhoto(photo))
                {
                    return BadRequest("Invalid photo format or size.");
                }

                // Save original photo
                var photoUrl = await _photoStorage.SavePhotoAsync(photo, User.Identity.Name);

                // Create and save thumbnail
                var thumbnailData = await _photoProcessing.CreateThumbnailAsync(photo);
                var thumbnailUrl = await _photoStorage.SaveThumbnailAsync(thumbnailData, photoUrl);

                // Create photo record
                var recipePhoto = new RecipePhoto
                {
                    RecipeId = recipeId,
                    FilePath = photoUrl,
                    ThumbnailPath = thumbnailUrl,
                    ContentType = photo.ContentType,
                    FileSize = photo.Length,
                    IsMain = !recipe.Photos.Any(), // First photo becomes main photo
                    UploadedAt = DateTime.UtcNow,
                    UploadedById = User.Identity.Name
                };

                // Add photo to recipe
                recipe.Photos.Add(recipePhoto);
                await _recipeService.UpdateRecipeAsync(recipe);

                return Json(new
                {
                    success = true,
                    photoId = recipePhoto.Id,
                    thumbnailUrl = recipePhoto.ThumbnailUrl,
                    isMain = recipePhoto.IsMain
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading photo for recipe {RecipeId}", recipeId);
                return StatusCode(500, "Error uploading photo");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetMainPhoto(int recipeId, int photoId)
        {
            try
            {
                var recipe = await _recipeService.GetRecipeByIdAsync(recipeId);
                if (recipe.CreatedById != User.Identity.Name)
                {
                    return Forbid();
                }

                // Update main photo status
                foreach (var photo in recipe.Photos)
                {
                    photo.IsMain = photo.Id == photoId;
                }

                await _recipeService.UpdateRecipeAsync(recipe);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting main photo {PhotoId} for recipe {RecipeId}", photoId, recipeId);
                return StatusCode(500, "Error setting main photo");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int recipeId, int photoId)
        {
            try
            {
                var recipe = await _recipeService.GetRecipeByIdAsync(recipeId);
                if (recipe.CreatedById != User.Identity.Name)
                {
                    return Forbid();
                }

                var photo = recipe.Photos.FirstOrDefault(p => p.Id == photoId);
                if (photo == null)
                {
                    return NotFound();
                }

                // Delete from storage
                await _photoStorage.DeletePhotoAsync(photo.StorageUrl);
                if (!string.IsNullOrEmpty(photo.ThumbnailUrl))
                {
                    await _photoStorage.DeletePhotoAsync(photo.ThumbnailUrl);
                }

                // Remove from recipe
                recipe.Photos.Remove(photo);

                // If we deleted the main photo, set a new one
                if (photo.IsMain && recipe.Photos.Any())
                {
                    recipe.Photos.First().IsMain = true;
                }

                await _recipeService.UpdateRecipeAsync(recipe);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting photo {PhotoId} from recipe {RecipeId}", photoId, recipeId);
                return StatusCode(500, "Error deleting photo");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetGallery(int recipeId)
        {
            try
            {
                var recipe = await _recipeService.GetRecipeByIdAsync(recipeId);
                var photos = recipe.Photos.Select(p => new
                {
                    id = p.Id,
                    thumbnailUrl = p.ThumbnailUrl,
                    isMain = p.IsMain,
                    uploadedAt = p.UploadedAt
                });

                return Json(photos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting photo gallery for recipe {RecipeId}", recipeId);
                return StatusCode(500, "Error loading photo gallery");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPhoto(int recipeId, int photoId)
        {
            try
            {
                var recipe = await _recipeService.GetRecipeByIdAsync(recipeId);
                var photo = recipe.Photos.FirstOrDefault(p => p.Id == photoId);

                if (photo == null)
                {
                    return NotFound();
                }

                var photoData = await _photoStorage.GetPhotoAsync(photo.StorageUrl);
                return File(photoData, photo.ContentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting photo {PhotoId} for recipe {RecipeId}", photoId, recipeId);
                return StatusCode(500, "Error loading photo");
            }
        }
    }
}