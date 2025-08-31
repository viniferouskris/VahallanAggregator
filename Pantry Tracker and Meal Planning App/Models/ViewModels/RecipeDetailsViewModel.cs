using System.Collections.Generic;
using Vahallan_Ingredient_Aggregator.Models.Photo;

namespace Vahallan_Ingredient_Aggregator.Models.ViewModels
{
    public class RecipeDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public int PrepTimeMinutes { get; set; }
        public int CookTimeMinutes { get; set; }
        public bool IsPublic { get; set; }
        public string CreatedBy { get; set; }
        public bool IsOwner { get; set; }
        public decimal NumberOfServings { get; set; }
        public List<RecipeIngredientViewModel> Ingredients { get; set; }
        public ICollection<RecipePhoto> Photos { get; set; } = new List<RecipePhoto>();

        public decimal TotalCost { get; set; }

        public decimal TotalCalories { get; set; }

        public string Collection { get; set; }
        public bool ShowInIngredientsList { get; set; }
        public RecipeAccuracyLevel AccuracyLevel { get; set; }
    }

    public class RecipeIngredientViewModel
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }
}
