using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Vahallan_Ingredient_Aggregator.Models.Photo
{
    public class RecipePhoto
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string FilePath { get; set; }

        [MaxLength(1000)]
        public string ThumbnailPath { get; set; }

        [Required]
        public string ContentType { get; set; }

        public long FileSize { get; set; }

        [MaxLength(255)]
        public string FileName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsMainPhoto { get; set; }

        public bool IsApproved { get; set; }
        public bool IsMain { get; set; }


        [Required]
        public string UploadedById { get; set; }

        public DateTime UploadedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        // URL properties for easy access
        public string StorageUrl => FilePath;
        public string ThumbnailUrl => ThumbnailPath;
    }
}

