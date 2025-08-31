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

        [Column(TypeName = "decimal(10,2)")]
        public decimal CaloriesPerServing { get; set; }

        public decimal ServingsPerPackage { get; set; }

        public bool IsSystemIngredient { get; set; }
        public bool IsPromoted { get; set; }
        public DateTime? PromotionStartDate { get; set; }
        public DateTime? PromotionEndDate { get; set; }

        // Add reference to original system ingredient if this is a user copy
        public int? SystemIngredientId { get; set; }

        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();


        [NotMapped]
        public decimal ServingCost => CostPerPackage / ServingsPerPackage;




        public Ingredient()
        {
            Type = "Ingredient";
        }
        public override decimal GetTotalCost()
        {
            return (decimal)(Quantity * ServingCost);
        }

        public override decimal GetTotalCalories()
        {
            return (decimal)(Quantity * CaloriesPerServing);
        }

        public override List<Ingredient> GetIngredientList()
        {
            return new List<Ingredient> { this };
        }
        public Ingredient CreateUserCopy(string userId)
        {
            if (!IsSystemIngredient)
            {
                throw new InvalidOperationException("Can only create user copies from system ingredients");
            }

            return new Ingredient
            {
                Name = this.Name,
                Unit = this.Unit,
                CostPerPackage = this.CostPerPackage,
                ServingsPerPackage = this.ServingsPerPackage,
                CaloriesPerServing = this.CaloriesPerServing,
                CreatedById = userId,
                CreatedAt = DateTime.UtcNow,
                IsSystemIngredient = false,
                SystemIngredientId = this.Id
            };
        }


        // Modified to accept the service as a parameter
        //public void UpdateMeasurement(decimal quantity, string unit, IMeasurementConversionService conversionService)
        //{
        //    Quantity = quantity;
        //    Unit = unit;

        //    if (conversionService != null)
        //    {
        //        UpdateStoredMeasurement(conversionService);
        //    }
        //}

        public override BaseIngredientComponent Clone()
        {
            var clone = (Ingredient)base.Clone();
            clone.CostPerPackage = this.CostPerPackage;
            clone.CaloriesPerServing = this.CaloriesPerServing;
            clone.ServingsPerPackage = this.ServingsPerPackage;
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

            if (ServingsPerPackage <= 0)
            {
                errors.Add("Servings per package must be greater than zero");
                isValid = false;
            }

            if (CaloriesPerServing < 0)
            {
                errors.Add("Calories per serving cannot be negative");
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