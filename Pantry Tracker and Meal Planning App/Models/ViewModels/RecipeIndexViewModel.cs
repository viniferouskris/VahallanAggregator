namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.ViewModels
{
    public class RecipeIndexViewModel
    {
        public List<RecipeViewModel> UserRecipes { get; set; } = new();
        public List<RecipeViewModel> SharedRecipes { get; set; } = new();
        public List<ExternalRecipeViewModel> ExternalRecipes { get; set; } = new();
        public string ActiveTab { get; set; } = "user"; // Default tab
    }
}
