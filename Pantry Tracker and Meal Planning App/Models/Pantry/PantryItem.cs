namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Pantry
{
    public class PantryItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }

        public decimal CurrentQuantity { get; set; }
        public string CurrentUnit { get; set; }

        public decimal LastPurchaseQuantity { get; set; }
        public string LastPurchaseUnit { get; set; }
        public decimal? LastPurchasePrice { get; set; }
        public DateTime LastPurchaseDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public DateTime? ExpirationDate { get; set; }
        public int? ReplenishmentDays { get; set; }
        public decimal DailyUsageRate { get; set; }
        public decimal LowStockThreshold { get; set; }
    }
}
