namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Photo
{
    public class PhotoStorageSettings
    {
        public long MaxFileSize { get; set; } = 5242880; // 5MB default
        public string[] AllowedExtensions { get; set; } = new[] { ".jpg", ".jpeg", ".png" };
        public ThumbnailSize ThumbnailSize { get; set; } = new ThumbnailSize
        {
            Width = 300,
            Height = 300
        };
    }
}
