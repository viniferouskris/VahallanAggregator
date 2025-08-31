using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.External;

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Services.Interfaces
{
    public interface ITheMealDBService
    {
        /// <summary>
        /// Searches for meals by name
        /// </summary>
        Task<List<MealDBMeal>> SearchByNameAsync(string name);

        /// <summary>
        /// Gets a meal by its ID
        /// </summary>
        Task<MealDBMeal> GetByIdAsync(string id);

        /// <summary>
        /// Gets a random meal
        /// </summary>
        Task<MealDBMeal> GetRandomMealAsync();

        /// <summary>
        /// Lists all meal categories
        /// </summary>
        Task<List<string>> GetCategoriesAsync();

        /// <summary>
        /// Filters meals by main ingredient
        /// </summary>
        Task<List<MealDBMeal>> FilterByIngredientAsync(string ingredient);

        /// <summary>
        /// Filters meals by category
        /// </summary>
        Task<List<MealDBMeal>> FilterByCategoryAsync(string category);

        /// <summary>
        /// Filters meals by area (cuisine)
        /// </summary>
        Task<List<MealDBMeal>> FilterByAreaAsync(string area);
        Task<IEnumerable<object>> SearchMealsAsync(string searchTerm);
        Task GetMealByIdAsync(string id);
    }
}