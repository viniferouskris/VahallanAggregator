using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Photo;
using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Services.Interfaces;

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Components
{
    public abstract class BaseIngredientComponent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Quantity { get; set; } = 0;

        [Required]
        [StringLength(20)]
        public string Unit { get; set; }

        public decimal StoredQuantity { get; set; }
        public string StoredUnit { get; set; }

        [Required]
        public string CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        // Required for EF Core TPH inheritance
        [Required]
        public string Type { get; set; }

        // Virtual navigation property for photos
        public virtual ICollection<RecipePhoto> Photos { get; set; }

        [NotMapped]
        public string MainPhotoUrl => Photos?.FirstOrDefault(p => p.IsMain)?.StorageUrl;

        protected BaseIngredientComponent()
        {
            CreatedAt = DateTime.UtcNow;
            Type = GetType().Name;
            Photos = new List<RecipePhoto>();
        }

        // Abstract methods
        public abstract decimal GetTotalCost();
        public abstract decimal GetTotalCalories();
        public abstract List<Ingredient> GetIngredientList();

        // Virtual methods with default implementations
        public virtual void Add(BaseIngredientComponent component)
        {
            throw new NotImplementedException($"Add operation not supported for {GetType().Name}");
        }

        public virtual void Remove(BaseIngredientComponent component)
        {
            throw new NotImplementedException($"Remove operation not supported for {GetType().Name}");
        }

        public virtual BaseIngredientComponent GetChild(int index)
        {
            throw new NotImplementedException($"GetChild operation not supported for {GetType().Name}");
        }

        public virtual BaseIngredientComponent Clone()
        {
            var clone = (BaseIngredientComponent)MemberwiseClone();
            clone.Id = 0;
            clone.CreatedAt = DateTime.UtcNow;
            clone.ModifiedAt = null;
            clone.Photos = new List<RecipePhoto>();
            return clone;
        }

        public virtual bool Validate(out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Name))
                errors.Add("Name is required");

            if (Quantity <= 0)
                errors.Add("Quantity must be greater than zero");

            if (string.IsNullOrWhiteSpace(Unit))
                errors.Add("Unit is required");

            return errors.Count == 0;
        }
    }
}