using Microsoft.EntityFrameworkCore;
using Vahallan_Ingredient_Aggregator.Data;
using Vahallan_Ingredient_Aggregator.Models.Components;
using Vahallan_Ingredient_Aggregator.Models.Notifications;
using Vahallan_Ingredient_Aggregator.Models.Pantry;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;
using System.Collections.Generic;

namespace Vahallan_Ingredient_Aggregator.Services.Implementaions
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
