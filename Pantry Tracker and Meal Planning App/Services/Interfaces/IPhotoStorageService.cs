namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Services.Interfaces
{
    public interface IPhotoStorageService
    {
        Task<string> SavePhotoAsync(IFormFile file, string userId);
        Task<string> SaveThumbnailAsync(byte[] thumbnailData, string originalPhotoId);
        Task DeletePhotoAsync(string photoId);
        Task<byte[]> GetPhotoAsync(string photoId);
        Task<byte[]> GetThumbnailAsync(string photoId);
        bool ValidatePhoto(IFormFile file);
    }
}
