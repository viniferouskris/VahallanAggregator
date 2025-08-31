using Vahallan_Ingredient_Aggregator.Models.Components;

namespace Vahallan_Ingredient_Aggregator.Services.Interfaces
{
    // IIngredientService.cs
    public interface IIngredientService
    {
        // Existing methods
        Task<Ingredient> GetIngredientAsync(int id);
        Task<IEnumerable<Ingredient>> GetAllIngredientsAsync();
        Task<Ingredient> CreateIngredientAsync(Ingredient ingredient);
        Task AddPhotosToIngredientAsync(Ingredient ingredient, IFormFileCollection photos, string userId);

        // Add these new methods with proper return types
        Task<OperationResult> UpdateIngredientAsync(int id, Ingredient ingredient, string userId, bool isAdmin);
        Task<OperationResult> DeleteIngredientAsync(int id, string userId, bool isAdmin);
    }
}

