using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Vahallan_Ingredient_Aggregator.Models.Photo;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace Vahallan_Ingredient_Aggregator.Services.Implementations
{
    public class PhotoProcessingService : IPhotoProcessingService
    {
        private readonly PhotoStorageSettings _settings;
        private readonly ILogger<PhotoProcessingService> _logger;

        public PhotoProcessingService(
            IOptions<PhotoStorageSettings> settings,
            ILogger<PhotoProcessingService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<byte[]> CreateThumbnailAsync(IFormFile originalPhoto)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                await originalPhoto.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                using var image = SixLabors.ImageSharp.Image.Load(memoryStream);
                var ratioX = (double)_settings.ThumbnailSize.Width / image.Width;
                var ratioY = (double)_settings.ThumbnailSize.Height / image.Height;
                var ratio = Math.Min(ratioX, ratioY);

                var newWidth = (int)(image.Width * ratio);
                var newHeight = (int)(image.Height * ratio);

                image.Mutate(x => x
                    .Resize(newWidth, newHeight)
                    .AutoOrient());

                using var outputStream = new MemoryStream();
                await image.SaveAsJpegAsync(outputStream);
                return outputStream.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating thumbnail for photo");
                throw;
            }
        }

        public async Task<byte[]> ResizePhotoAsync(IFormFile photo, int maxWidth, int maxHeight)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                await photo.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                using var image = SixLabors.ImageSharp.Image.Load(memoryStream);
                var ratioX = (double)maxWidth / image.Width;
                var ratioY = (double)maxHeight / image.Height;
                var ratio = Math.Min(ratioX, ratioY);

                if (ratio < 1)
                {
                    var newWidth = (int)(image.Width * ratio);
                    var newHeight = (int)(image.Height * ratio);

                    image.Mutate(x => x
                        .Resize(newWidth, newHeight)
                        .AutoOrient());
                }

                using var outputStream = new MemoryStream();
                await image.SaveAsJpegAsync(outputStream);
                return outputStream.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resizing photo");
                throw;
            }
        }

        public bool ValidatePhoto(IFormFile file, long maxSize, string[] allowedExtensions)
        {
            if (file == null || file.Length == 0)
                return false;

            if (file.Length > maxSize)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }
    }
}