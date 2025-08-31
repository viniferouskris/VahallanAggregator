using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Components;
using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.External;
using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.ViewModels;


namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Services.Interfaces
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
        Task<List<ExternalRecipeViewModel>> GetExternalRecipeIdsAsync();
        Task<int?> GetLocalRecipeIdByExternalIdAsync(string externalId);

        // Recipe listing methods
        Task<List<RecipeViewModel>> GetUserRecipesAsync(string userId);
        Task<List<RecipeViewModel>> GetSharedRecipesAsync(string userId);
        Task<List<ExternalRecipeViewModel>> GetExternalRecipesAsync();
        Task<List<RecipeViewModel>> GetAllRecipesAsync(string userId, bool isAdmin);

        // Import management
        Task<bool> IsRecipeImportedAsync(string externalId);
        Task<Recipe> ImportExternalRecipeAsync(MealDBResponse meal, string userId);
        Task<Recipe> ImportExternalRecipeAsync(
            MealDBResponse meal,
            string userId,
            IPhotoStorageService photoStorageService,
            IPhotoProcessingService photoProcessingService);

    }
}