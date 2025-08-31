using Microsoft.Extensions.Options;
using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Photo;
using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Services.Interfaces;

public class LocalPhotoStorageService : IPhotoStorageService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly PhotoStorageSettings _settings;
    private readonly ILogger<LocalPhotoStorageService> _logger;
    private static readonly HashSet<string> _processedFiles = new HashSet<string>();

    public LocalPhotoStorageService(
        IWebHostEnvironment webHostEnvironment,
        IOptions<PhotoStorageSettings> settings,
        ILogger<LocalPhotoStorageService> logger)
    {
        _webHostEnvironment = webHostEnvironment;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<string> SavePhotoAsync(IFormFile file, string userId)
    {
        try
        {
            _logger.LogInformation($"Starting SavePhotoAsync for file: {file.FileName}, Size: {file.Length}");

            // Create a unique key for this file
            var fileKey = $"{file.FileName}-{file.Length}-{userId}";

            // Check if we've already processed this exact file
            if (_processedFiles.Contains(fileKey))
            {
                _logger.LogWarning($"File already processed: {fileKey}");
                // Return the existing path
                var existingPath = Path.Combine("recipe-photos", "originals", $"{fileKey}.jpg");
                return $"/{existingPath.Replace('\\', '/')}";
            }

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            _logger.LogInformation($"Generated unique filename: {uniqueFileName}");

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "recipe-photos", "originals");
            var photoFilePath = Path.Combine(uploadsFolder, uniqueFileName);

            _logger.LogInformation($"Ensuring directory exists: {uploadsFolder}");
            Directory.CreateDirectory(uploadsFolder);

            _logger.LogInformation($"Saving file to: {photoFilePath}");
            using (var stream = new FileStream(photoFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Add to processed files
            _processedFiles.Add(fileKey);

            var relativePath = $"/recipe-photos/originals/{uniqueFileName}";
            _logger.LogInformation($"Successfully saved photo. Returning path: {relativePath}");
            return relativePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error saving photo: {file.FileName}");
            throw;
        }
    }

    public async Task<string> SaveThumbnailAsync(byte[] thumbnailData, string originalPhotoId)
    {
        try
        {
            _logger.LogInformation($"Starting SaveThumbnailAsync for originalPhotoId: {originalPhotoId}");

            // Ensure originalPhotoId is properly formatted
            var originalFileName = Path.GetFileName(originalPhotoId.TrimStart('/'));
            var thumbnailFileName = $"{Path.GetFileNameWithoutExtension(originalFileName)}_thumb.jpg";

            _logger.LogInformation($"Generated thumbnail filename: {thumbnailFileName}");

            var thumbnailsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "recipe-photos", "thumbnails");
            var thumbnailPath = Path.Combine(thumbnailsFolder, thumbnailFileName);

            _logger.LogInformation($"Ensuring thumbnails directory exists: {thumbnailsFolder}");
            Directory.CreateDirectory(thumbnailsFolder);

            _logger.LogInformation($"Saving thumbnail to: {thumbnailPath}");
            await File.WriteAllBytesAsync(thumbnailPath, thumbnailData);

            var relativePath = $"/recipe-photos/thumbnails/{thumbnailFileName}";
            _logger.LogInformation($"Successfully saved thumbnail. Returning path: {relativePath}");
            return relativePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error saving thumbnail for photo: {originalPhotoId}");
            throw;
        }
    }

    public bool ValidatePhoto(IFormFile file)
    {
        try
        {
            _logger.LogInformation($"Validating photo: {file.FileName}, Size: {file.Length}");

            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("File is null or empty");
                return false;
            }

            if (file.Length > _settings.MaxFileSize)
            {
                _logger.LogWarning($"File exceeds max size: {file.Length} > {_settings.MaxFileSize}");
                return false;
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var isValid = _settings.AllowedExtensions.Contains(extension);

            _logger.LogInformation($"File validation result: {isValid}, Extension: {extension}");
            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating photo");
            return false;
        }
    }

    // Add this method to LocalPhotoStorageService.cs
    public async Task DeletePhotoAsync(string photoPath)
    {
        try
        {
            if (string.IsNullOrEmpty(photoPath))
            {
                _logger.LogWarning("Attempted to delete photo with empty path");
                return;
            }

            // Remove leading slash if present
            photoPath = photoPath.TrimStart('/');

            // Get the full physical path
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, photoPath);

            _logger.LogInformation($"Attempting to delete photo at path: {fullPath}");

            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
                _logger.LogInformation($"Successfully deleted photo: {fullPath}");
            }
            else
            {
                _logger.LogWarning($"Photo not found at path: {fullPath}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting photo at path: {photoPath}");
            throw;
        }
    }

    public Task<byte[]> GetPhotoAsync(string photoId)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> GetThumbnailAsync(string photoId)
    {
        throw new NotImplementedException();
    }

    // ... rest of your existing methods ...
}