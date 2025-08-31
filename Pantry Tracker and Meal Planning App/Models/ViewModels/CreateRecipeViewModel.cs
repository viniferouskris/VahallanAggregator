using System.ComponentModel.DataAnnotations;

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.ViewModels
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
    }

}
