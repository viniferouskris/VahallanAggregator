namespace Vahallan_Ingredient_Aggregator.Services.Interfaces
{
    public interface IPhotoProcessingService
    {
        Task<byte[]> CreateThumbnailAsync(IFormFile originalPhoto);
        Task<byte[]> ResizePhotoAsync(IFormFile photo, int maxWidth, int maxHeight);
        bool ValidatePhoto(IFormFile file, long maxSize, string[] allowedExtensions);
    }
}
