namespace Vahallan_Ingredient_Aggregator.Models.ViewModels
{
    public class RecipeIndexViewModel
    {
        public List<RecipeViewModel> UserRecipes { get; set; } = new();
        public List<RecipeViewModel> SharedRecipes { get; set; } = new();
        public List<ExternalRecipeViewModel> ExternalRecipes { get; set; } = new();
        public string ActiveTab { get; set; } = "user"; // Default tab
    }
}
