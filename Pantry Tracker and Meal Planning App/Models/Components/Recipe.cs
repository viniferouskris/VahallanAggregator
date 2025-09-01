using Azure;
using Vahallan_Ingredient_Aggregator.Models.Components;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Vahallan_Ingredient_Aggregator.Models.Photo;
using Vahallan_Ingredient_Aggregator.Models.Components;


namespace Vahallan_Ingredient_Aggregator.Models.Components
{
    public class Recipe : BaseIngredientComponent
    {
        public string Description { get; set; }
        public string Instructions { get; set; }
        public int PrepTimeMinutes { get; set; }
        public int CookTimeMinutes { get; set; }
        public int Version { get; set; } = 1;
        public int? OriginalRecipeId { get; set; }
        [Display(Name = "Number of Servings")]
        [Column(TypeName = "decimal")]
        public decimal NumberOfServings { get; set; }
        public bool IsPublic { get; set; }

        //// New properties for external recipe sources
        //public string? ExternalId { get; set; }
        //public string? ExternalSource { get; set; }
        //public string? ExternalUrl { get; set; }
        //public DateTime? ImportedAt { get; set; }

        //public bool IsExternalRecipe => !string.IsNullOrEmpty(ExternalId);

        public string Collection { get; set; } = string.Empty;
        public bool ShowInIngredientsList { get; set; } = false;
        public RecipeAccuracyLevel AccuracyLevel { get; set; } = RecipeAccuracyLevel.Estimate;
        public string PatternCode { get; set; }  // For wallpaper pattern codes
        public decimal StandardSquareFeet { get; set; }  // Standard size for scaling


        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

       // public virtual ICollection<BaseIngredientComponent> Components { get; set; }
     //   public virtual ICollection<RecipePhoto> Photos { get; set; }
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

        //public virtual ICollection<RecipePhoto> Photos { get; set; } = new List<RecipePhoto>();
        //public string MainPhotoUrl => Photos.FirstOrDefault(p => p.IsMain)?.StorageUrl;

        public Recipe()
        {
            Type = "Recipe";
            Unit = "serving";
            StoredUnit = "serving";
            Quantity = 1;
            StoredQuantity = 1;
            // Components = new List<BaseIngredientComponent>();
            RecipeIngredients = new List<RecipeIngredient>();
            Tags = new List<Tag>();
            //Photos = new List<RecipePhoto>();
            Version = 1;

        }
        //public override void Add(BaseIngredientComponent component)
        //{
        //    component.Id = Id;
        //    RecipeIngredients.Add(component);
        //}
        // Composite pattern implementation using RecipeIngredients
        public override decimal GetTotalCost()
        {
            return RecipeIngredients.Sum(ri =>
                ri.Ingredient.CostPerPackage / ri.Ingredient.UnitsPerPackage * ri.Quantity);
        }

        public override List<Ingredient> GetIngredientList()
        {
            return RecipeIngredients
                .Select(ri =>
                {
                    var ingredient = ri.Ingredient.Clone() as Ingredient;
                    ingredient.Quantity = ri.Quantity;
                    ingredient.Unit = ri.Unit;
                    return ingredient;
                })
                .ToList();
        }

        // Methods to maintain the composite pattern interface
        public void Add(Ingredient ingredient, decimal quantity, string unit)
        {
            var recipeIngredient = new RecipeIngredient
            {
                Recipe = this,
                Ingredient = ingredient,
                Quantity = quantity,
                Unit = unit
            };
            RecipeIngredients.Add(recipeIngredient);
        }
        //public override void Remove(BaseIngredientComponent component)
        //{
        //    Components.Remove(component);
        //}

        //public override BaseIngredientComponent GetChild(int index)
        //{
        //    return Components.ElementAtOrDefault(index);
        //}

        //public override void Scale(decimal factor)
        //{
        //    base.Scale(factor);
        //    foreach (var component in Components)
        //    {
        //        component.Scale(factor);
        //    }
        //}

        //// Create a new version of the recipe
        //public Recipe CreateNewVersion(string userId)
        //{
        //    var newVersion = (Recipe)this.Clone();
        //    newVersion.OriginalRecipeId = this.OriginalRecipeId ?? this.Id;
        //    newVersion.Version = this.Version + 1;
        //    newVersion.CreatedById = userId;
        //    newVersion.Components = new List<BaseIngredientComponent>();

        //    // Clone all components
        //    foreach (var component in this.Components)
        //    {
        //        var clonedComponent = component.Clone();
        //        newVersion.Add(clonedComponent);
        //    }

        //    // Copy tags
        //    foreach (var tag in this.Tags)
        //    {
        //        newVersion.Tags.Add(tag);
        //    }

        //    return newVersion;
        //}

        public override bool Validate(out List<string> errors)
        {
            var isValid = base.Validate(out errors);

            if (string.IsNullOrWhiteSpace(Instructions))
            {
                errors.Add("Instructions are required");
                isValid = false;
            }

            if (PrepTimeMinutes < 0)
            {
                errors.Add("Prep time cannot be negative");
                isValid = false;
            }

            if (CookTimeMinutes < 0)
            {
                errors.Add("Cook time cannot be negative");
                isValid = false;
            }

            if (!RecipeIngredients.Any())
            {
                errors.Add("Recipe must have at least one ingredient");
                isValid = false;
            }

            // Validate all components
            //foreach (var component in RecipeIngredients)
            //{
            //    if (!component.Validate(out var componentErrors))
            //    {
            //        errors.AddRange(componentErrors);
            //        isValid = false;
            //    }
            //}

            return isValid;
        }

        public override BaseIngredientComponent Clone()
        {
            var clone = (Recipe)base.Clone();
            clone.Description = this.Description;
            clone.Instructions = this.Instructions;
            clone.PrepTimeMinutes = this.PrepTimeMinutes;
            clone.CookTimeMinutes = this.CookTimeMinutes;
            clone.Version = this.Version;
            clone.OriginalRecipeId = this.OriginalRecipeId;
            return clone;
        }
    } 
}