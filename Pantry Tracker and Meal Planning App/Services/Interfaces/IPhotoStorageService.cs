namespace Vahallan_Ingredient_Aggregator.Services.Interfaces
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
