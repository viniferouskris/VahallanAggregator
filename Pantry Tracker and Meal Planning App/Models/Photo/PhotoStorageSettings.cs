namespace Vahallan_Ingredient_Aggregator.Models.Photo
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
