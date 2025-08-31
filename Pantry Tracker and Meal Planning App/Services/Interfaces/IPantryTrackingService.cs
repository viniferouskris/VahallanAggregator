using Vahallan_Ingredient_Aggregator.Models.Components;
using Vahallan_Ingredient_Aggregator.Models.Pantry;

namespace Vahallan_Ingredient_Aggregator.Services.Interfaces
{
    public interface IPantryTrackingService
    {
        Task<PantryItem> AddOrUpdateFromPurchaseAsync(string userId, Ingredient ingredient, decimal purchaseQuantity);
        Task DeductFromInventoryAsync(string userId, IEnumerable<(Ingredient Ingredient, decimal Quantity)> deductions);
        Task<IEnumerable<PantryItem>> GetLowStockItemsAsync(string userId);
        Task<IEnumerable<PantryItem>> GetExpiringItemsAsync(string userId, int daysThreshold = 7);
    }
}
