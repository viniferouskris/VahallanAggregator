using Vahallan_Ingredient_Aggregator.Models.Components;
using Vahallan_Ingredient_Aggregator.Models.Photo;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;
using Vahallan_Ingredient_Aggregator.Models.Components;
using System.ComponentModel.DataAnnotations.Schema;

//Place functions that are intrinsic to what an Ingredient "is" or "does"
//Methods that operate on the ingredient's own properties
//Business logic specific to a single ingredient instance

namespace Vahallan_Ingredient_Aggregator.Models.Components
{
    public class Ingredient : BaseIngredientComponent
    {
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPerPackage { get; set; }
        public decimal UnitsPerPackage { get; set; }
        public string MaterialType { get; set; } = "General";  // Paper, Paint, Metallic, etc.
        public string Vendor { get; set; } = string.Empty;
        // Add reference to original system ingredient if this is a user copy
        public int? SystemIngredientId { get; set; }

        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();


        //[NotMapped]
        //public decimal ServingCost => CostPerPackage / UnitsPerPackage;

  
        [NotMapped]
        public decimal CostPerUnit => CostPerPackage / UnitsPerPackage;  // Cost per individual unit


        public Ingredient()
        {
            Type = "Ingredient";
        }
        public override decimal GetTotalCost()
        {
            return (decimal)(UnitsPerPackage * CostPerPackage);
        }

        public override List<Ingredient> GetIngredientList()
        {
            return new List<Ingredient> { this };
        }

        public override BaseIngredientComponent Clone()
        {
            var clone = (Ingredient)base.Clone();
            clone.CostPerPackage = this.CostPerPackage;
            clone.UnitsPerPackage = this.UnitsPerPackage;
            return clone;
        }

        public override bool Validate(out List<string> errors)
        {
            var isValid = base.Validate(out errors);

            if (CostPerPackage <= 0)
            {
                errors.Add("Cost per package must be greater than zero");
                isValid = false;
            }

            if (UnitsPerPackage <= 0)
            {
                errors.Add("Servings per package must be greater than zero");
                isValid = false;
            }


            return isValid;
        }
    }


    // Add this class to help return results when edit or deleting ingredients
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}