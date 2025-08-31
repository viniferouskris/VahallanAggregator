using System.Text.Json;
using Microsoft.Extensions.Logging;
using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Services.Interfaces;
using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.External;
using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models;

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Services.Implementations
{
    public class TheMealDBService : ITheMealDBService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TheMealDBService> _logger;
        private const string BaseUrl = "https://www.themealdb.com/api/json/v1/1";

        public TheMealDBService(HttpClient httpClient, ILogger<TheMealDBService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<MealDBMeal>> SearchByNameAsync(string name)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/search.php?s={Uri.EscapeDataString(name)}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var searchResult = JsonSerializer.Deserialize<MealDBResponse>(content);

                return searchResult?.Meals ?? new List<MealDBMeal>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching meals by name: {Name}", name);
                throw;
            }
        }

        public async Task<MealDBMeal> GetByIdAsync(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/lookup.php?i={id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<MealDBResponse>(content);

                return result?.Meals?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting meal by ID: {Id}", id);
                throw;
            }
        }

        public async Task<MealDBMeal> GetRandomMealAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/random.php");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<MealDBResponse>(content);

                return result?.Meals?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting random meal");
                throw;
            }
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/list.php?c=list");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                // Parse the categories from the response
                // Note: You'll need to adjust this based on the actual response format
                return new List<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                throw;
            }
        }

        public async Task<List<MealDBMeal>> FilterByIngredientAsync(string ingredient)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/filter.php?i={Uri.EscapeDataString(ingredient)}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<MealDBResponse>(content);

                return result?.Meals ?? new List<MealDBMeal>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering by ingredient: {Ingredient}", ingredient);
                throw;
            }
        }

        public async Task<List<MealDBMeal>> FilterByCategoryAsync(string category)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/filter.php?c={Uri.EscapeDataString(category)}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<MealDBResponse>(content);

                return result?.Meals ?? new List<MealDBMeal>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering by category: {Category}", category);
                throw;
            }
        }

        public async Task<List<MealDBMeal>> FilterByAreaAsync(string area)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/filter.php?a={Uri.EscapeDataString(area)}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<MealDBResponse>(content);

                return result?.Meals ?? new List<MealDBMeal>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering by area: {Area}", area);
                throw;
            }
        }

        public async Task<IEnumerable<object>> SearchMealsAsync(string searchTerm)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/search.php?s={Uri.EscapeDataString(searchTerm)}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var searchResult = JsonSerializer.Deserialize<MealDBResponse>(content);

                return searchResult?.Meals ?? new List<MealDBMeal>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching meals by name: {Name}", searchTerm);
                throw;
            }
        }

        public Task GetMealByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}