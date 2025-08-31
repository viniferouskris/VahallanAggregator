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
         
        
        // Recipe visibility
        Task ToggleRecipeVisibilityAsync(int recipeId, string userId);

        // External recipe handling
        Task<int?> GetLocalRecipeIdByExternalIdAsync(string externalId);

        // Recipe listing methods
        Task<List<RecipeViewModel>> GetUserRecipesAsync(string userId);
        Task<List<RecipeViewModel>> GetSharedRecipesAsync(string userId);
        Task<List<RecipeViewModel>> GetAllRecipesAsync(string userId, bool isAdmin);


    }
}