using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Standard Sheet Size (sq ft)")]
        public decimal StandardSheetSize { get; set; } = 25m; // Default 2.5' x 10' = 25 sq ft
        public List<RecipeIngredientViewModel> Ingredients { get; set; }
        public ICollection<RecipePhoto> Photos { get; set; } = new List<RecipePhoto>();

        public decimal TotalCost { get; set; }


        public string Collection { get; set; }
        public bool ShowInIngredientsList { get; set; }
        public RecipeAccuracyLevel AccuracyLevel { get; set; }

        public decimal CostPerSquareFoot => StandardSheetSize > 0 ? TotalCost / StandardSheetSize : 0;

        // COMPUTED: Material amounts per square foot (for scaling large orders)
        public List<MaterialPerSquareFootViewModel> MaterialsPerSquareFoot { get; set; } = new List<MaterialPerSquareFootViewModel>();
    }

    public class RecipeIngredientViewModel
    {
        public string Name { get; set; }

        [Display(Name = "Amount per Sheet")]
        public decimal Quantity { get; set; }

        [Display(Name = "Working Unit")]
        public string Unit { get; set; }  // cups, ounces, etc. (working units)

        // Wallpaper properties
        public string MaterialType { get; set; }
        public string Vendor { get; set; }

        // COMPUTED: Amount per square foot for scaling
        public decimal QuantityPerSquareFoot { get; set; }

        // Display the purchasing unit info
        [Display(Name = "Purchase Unit")]
        public string PurchaseUnit { get; set; }  // gallons, quarts, lbs (how you buy it)

        [Display(Name = "Cost Per Purchase Unit")]
        public decimal CostPerPurchaseUnit { get; set; }
    }
    // Helper class for displaying scaled amounts
    public class MaterialPerSquareFootViewModel
    {
        public string MaterialName { get; set; }
        public decimal AmountPerSquareFoot { get; set; }
        public string Unit { get; set; }
        public decimal CostPerSquareFoot { get; set; }
    }
}
