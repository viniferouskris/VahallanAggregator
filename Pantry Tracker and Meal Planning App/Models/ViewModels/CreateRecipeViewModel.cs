using System.ComponentModel.DataAnnotations;

namespace Vahallan_Ingredient_Aggregator.Models.ViewModels
{
    public class CreateRecipeViewModel
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [StringLength(5000)]
        public string Instructions { get; set; }

        [Display(Name = "Prep Time (minutes)")]
        [Range(0, int.MaxValue)]
        public int PrepTimeMinutes { get; set; }

        [Display(Name = "Cook Time (minutes)")]
        [Range(0, int.MaxValue)]
        public int CookTimeMinutes { get; set; }

        public bool IsPublic { get; set; }

        public string IngredientsJson { get; set; }
        [Display(Name = "Collection")]
        [StringLength(100)]
        public string Collection { get; set; } = string.Empty;

        [Display(Name = "Show in Ingredients List")]
        public bool ShowInIngredientsList { get; set; } = false;

        [Display(Name = "Accuracy Level")]
        public RecipeAccuracyLevel AccuracyLevel { get; set; } = RecipeAccuracyLevel.Estimate;
    }

}
