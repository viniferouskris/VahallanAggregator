using Vahallan_Ingredient_Aggregator.Models.Components;

public class MealPlan : BaseIngredientComponent
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ServingSize { get; set; }
    public virtual ICollection<MealPlanRecipe> MealPlanRecipes { get; set; } = new List<MealPlanRecipe>();
    public string UserId { get; set; }

    public MealPlan()
    {
        Type = "MealPlan";
    }

    public override decimal GetTotalCost()
    {
        return MealPlanRecipes.Sum(mpr => mpr.Recipe.GetTotalCost() * ((decimal)mpr.ServingSize / mpr.Recipe.NumberOfServings));
    }

    public override decimal GetTotalCalories()
    {
        return MealPlanRecipes.Sum(mpr => mpr.Recipe.GetTotalCalories() * ((decimal)mpr.ServingSize / mpr.Recipe.NumberOfServings));
    }

    public override List<Ingredient> GetIngredientList()
    {
        var ingredients = new List<Ingredient>();
        foreach (var mpr in MealPlanRecipes)
        {
            var recipeIngredients = mpr.Recipe.GetIngredientList();
            var scalingFactor = (decimal)mpr.ServingSize / mpr.Recipe.NumberOfServings;

            foreach (var ingredient in recipeIngredients)
            {
                var scaledIngredient = (Ingredient)ingredient.Clone();
             //   scaledIngredient.Scale(scalingFactor);
                ingredients.Add(scaledIngredient);
            }
        }
        return ingredients;
    }

    public void AddRecipe(Recipe recipe, DateTime prepareOnDate, int servingSize)
    {
        var mealPlanRecipe = new MealPlanRecipe
        {
            Recipe = recipe,
            PrepareOnDate = prepareOnDate,
            ServingSize = servingSize,
            MealPlan = this
        };
        MealPlanRecipes.Add(mealPlanRecipe);
    }

    public void RemoveRecipe(int recipeId, DateTime prepareOnDate)
    {
        var mealPlanRecipe = MealPlanRecipes
            .FirstOrDefault(mpr => mpr.RecipeId == recipeId && mpr.PrepareOnDate.Date == prepareOnDate.Date);

        if (mealPlanRecipe != null)
        {
            MealPlanRecipes.Remove(mealPlanRecipe);
        }
    }

    public IEnumerable<MealPlanRecipe> GetRecipesForDate(DateTime date)
    {
        return MealPlanRecipes
            .Where(mpr => mpr.PrepareOnDate.Date == date.Date)
            .OrderBy(mpr => mpr.PrepareOnDate);
    }

    public override bool Validate(out List<string> errors)
    {
        var isValid = base.Validate(out errors);

        if (StartDate >= EndDate)
        {
            errors.Add("Start date must be before end date");
            isValid = false;
        }

        if (ServingSize <= 0)
        {
            errors.Add("Serving size must be greater than zero");
            isValid = false;
        }

        if (string.IsNullOrEmpty(UserId))
        {
            errors.Add("User ID is required");
            isValid = false;
        }

        return isValid;
    }
}

// Junction table entity for many-to-many relationship between MealPlan and Recipe
public class MealPlanRecipe
{
    public int Id { get; set; }
    public int MealPlanId { get; set; }
    public int RecipeId { get; set; }
    public DateTime PrepareOnDate { get; set; }
    public int ServingSize { get; set; }
    public string Notes { get; set; }

    public virtual MealPlan MealPlan { get; set; }
    public virtual Recipe Recipe { get; set; }
}