using Vahallan_Ingredient_Aggregator.Models.Components;
using Vahallan_Ingredient_Aggregator.Models.ViewModels;


namespace Vahallan_Ingredient_Aggregator.Services.Interfaces
{
  
    public interface IRecipeService
    {
        // Core CRUD operations
        Task<Recipe> CreateRecipeAsync(Recipe recipe, string userId);
        Task<Recipe> GetRecipeByIdAsync(int id);
        Task UpdateRecipeAsync(Recipe recipe);
        Task DeleteRecipeAsync(int id);
        Task<bool> RecipeExistsAsync(int id);

        // SIMPLIFIED: Just ONE method to get ALL recipes
        Task<IEnumerable<RecipeViewModel>> GetAllRecipesAsync();


    }
}