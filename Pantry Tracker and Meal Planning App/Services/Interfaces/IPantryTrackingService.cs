using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Components;
using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Pantry;

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Services.Interfaces
{
    public interface IPantryTrackingService
    {
        Task<PantryItem> AddOrUpdateFromPurchaseAsync(string userId, Ingredient ingredient, decimal purchaseQuantity);
        Task DeductFromInventoryAsync(string userId, IEnumerable<(Ingredient Ingredient, decimal Quantity)> deductions);
        Task<IEnumerable<PantryItem>> GetLowStockItemsAsync(string userId);
        Task<IEnumerable<PantryItem>> GetExpiringItemsAsync(string userId, int daysThreshold = 7);
    }
}
