using Microsoft.EntityFrameworkCore;
using Pantry_Tracker_and_Meal_Planning_App.Data;
using Pantry_Tracker_and_Meal_Planning_App.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Pantry_Tracker_and_Meal_Planning_App.Models.Components
{
    // IngredientService.cs
    public class IngredientService : IIngredientService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMeasurementConversionService _conversionService;

        public IngredientService(
            ApplicationDbContext context,
            IMeasurementConversionService conversionService)
        {
            _context = context;
            _conversionService = conversionService;
        }

        public async Task<Ingredient> CreateIngredientAsync(Ingredient ingredient)
        {
            if (!ingredient.Validate(out var errors))
            {
                throw new ValidationException(string.Join(", ", errors));
            }

            ingredient.UpdateMeasurement(ingredient.Quantity, ingredient.Unit, _conversionService);
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return ingredient;
        }

        public async Task<Ingredient> GetIngredientAsync(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with ID {id} not found");
            }
            return ingredient;
        }

        public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
        {
            return await _context.Ingredients.ToListAsync();
        }

        public async Task UpdateIngredientAsync(Ingredient ingredient)
        {
            if (!ingredient.Validate(out var errors))
            {
                throw new ValidationException(string.Join(", ", errors));
            }

            var existingIngredient = await GetIngredientAsync(ingredient.Id);

            // Update properties
            existingIngredient.Name = ingredient.Name;
            existingIngredient.CostPerPackage = ingredient.CostPerPackage;
            existingIngredient.ServingsPerPackage = ingredient.ServingsPerPackage;
            existingIngredient.CaloriesPerServing = ingredient.CaloriesPerServing;
            existingIngredient.ModifiedAt = DateTime.UtcNow;

            // Update measurements if they've changed
            if (existingIngredient.Quantity != ingredient.Quantity ||
                existingIngredient.Unit != ingredient.Unit)
            {
                existingIngredient.UpdateMeasurement(
                    ingredient.Quantity,
                    ingredient.Unit,
                    _conversionService);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteIngredientAsync(int id)
        {
            var ingredient = await GetIngredientAsync(id);
            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateIngredientMeasurementAsync(int id, decimal quantity, string unit)
        {
            var ingredient = await GetIngredientAsync(id);
            ingredient.UpdateMeasurement(quantity, unit, _conversionService);
            await _context.SaveChangesAsync();
        }
    }
}
