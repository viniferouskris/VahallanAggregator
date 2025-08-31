using Microsoft.EntityFrameworkCore;
using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Data;
using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Components;
using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Notifications;
using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Pantry;
using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Services.Interfaces;
using System.Collections.Generic;

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Services.Implementaions
{
    public class PantryTrackingService : IPantryTrackingService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public PantryTrackingService(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<PantryItem> AddOrUpdateFromPurchaseAsync(
            string userId,
            Ingredient ingredient,
            decimal purchaseQuantity)
        {
            var existingItem = await _context.PantryItems
                .FirstOrDefaultAsync(p => p.UserId == userId && p.IngredientId == ingredient.Id);

            if (existingItem == null)
            {
                existingItem = new PantryItem
                {
                    UserId = userId,
                    IngredientId = ingredient.Id,
                    IngredientName = ingredient.Name,
                    CurrentUnit = ingredient.Unit
                };
                _context.PantryItems.Add(existingItem);
            }

            existingItem.CurrentQuantity += purchaseQuantity;
            await _context.SaveChangesAsync();

            return existingItem;
        }

        public async Task DeductFromInventoryAsync(
            string userId,
            IEnumerable<(Ingredient Ingredient, decimal Quantity)> deductions)
        {
            foreach (var (ingredient, quantity) in deductions)
            {
                var pantryItem = await _context.PantryItems
                    .FirstOrDefaultAsync(p => p.UserId == userId && p.IngredientId == ingredient.Id);

                if (pantryItem != null)
                {
                    pantryItem.CurrentQuantity -= quantity;

                    if (pantryItem.CurrentQuantity < pantryItem.LowStockThreshold)
                    {
                        await _notificationService.CreateNotificationAsync(
                            userId,
                            "Low Stock Alert",
                            $"Running low on {ingredient.Name}",
                            NotificationType.LowStock
                        );
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PantryItem>> GetLowStockItemsAsync(string userId)
        {
            return await _context.PantryItems
                .Where(p => p.UserId == userId && p.CurrentQuantity < p.LowStockThreshold)
                .ToListAsync();
        }

        public async Task<IEnumerable<PantryItem>> GetExpiringItemsAsync(
            string userId,
            int daysThreshold = 7)
        {
            var thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);
            return await _context.PantryItems
                .Where(p => p.UserId == userId && p.ExpirationDate <= thresholdDate)
                .ToListAsync();
        }
    }
}
