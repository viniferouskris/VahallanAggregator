namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Services.Interfaces
{
    public interface IPhotoProcessingService
    {
        Task<byte[]> CreateThumbnailAsync(IFormFile originalPhoto);
        Task<byte[]> ResizePhotoAsync(IFormFile photo, int maxWidth, int maxHeight);
        bool ValidatePhoto(IFormFile file, long maxSize, string[] allowedExtensions);
    }
}
