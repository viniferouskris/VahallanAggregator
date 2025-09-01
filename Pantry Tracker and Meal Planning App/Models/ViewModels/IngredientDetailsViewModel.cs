using Vahallan_Ingredient_Aggregator.Models.Photo;

namespace Vahallan_Ingredient_Aggregator.Models.ViewModels
{
    public class IngredientDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public decimal CostPerPackage { get; set; }
        public decimal UnitsPerPackage { get; set; }
        public decimal ServingCost => CostPerPackage / UnitsPerPackage;
        public bool IsSystemIngredient { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string SystemIngredientName { get; set; }
        public int? SystemIngredientId { get; set; }
        public bool IsPromoted { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanCopy { get; set; }

        // Usage statistics
        public int RecipeCount { get; set; }
        public DateTime? LastUsed { get; set; }
        public decimal AverageCostPerRecipe { get; set; }
        public ICollection<RecipePhoto> Photos { get; set; } = new List<RecipePhoto>();
        public string MaterialType { get; set; } = "General";
        public string Vendor { get; set; } = "";

    }
}
