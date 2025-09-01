using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Vahallan_Ingredient_Aggregator.Models.Photo;

public class IngredientViewModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Material Name")]
    [StringLength(200)]
    public string Name { get; set; }

    [Required]
    [Display(Name = "Quantity")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public decimal Quantity { get; set; }

    [Required]
    [Display(Name = "Unit")]
    public string Unit { get; set; }

    [Required]
    [Display(Name = "Cost Per Package")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Cost must be greater than 0")]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal CostPerPackage { get; set; }

    [Required]
    [Display(Name = "Units Per Package")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Servings must be greater than 0")]
    public decimal UnitsPerPackage { get; set; }

    public string MaterialType { get; set; } = "General";
    public string Vendor { get; set; } = "";

    // Add these new properties
    //  public bool IsSystemIngredient { get; set; }
    //  public bool IsPromoted { get; set; }
    public int? SystemIngredientId { get; set; }
    public string? ReturnUrl { get; set; }


    public ICollection<RecipePhoto> Photos { get; set; } = new List<RecipePhoto>();


    [Display(Name = "Cost Per Serving")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal ServingCost
    {
        get
        {
            // Protect against division by zero
            if (UnitsPerPackage <= 0)
            {
                return 0;
            }
            return CostPerPackage / UnitsPerPackage;
        }
    }
        // List of standard units for dropdown
    public static List<string> StandardUnits = new List<string>
    {
        "g", "kg", "oz", "lb",        // Weight
        "ml", "l", "cup", "tbsp", "tsp", // Volume
        "count", "dozen"               // Count
    };
}