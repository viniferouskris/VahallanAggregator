namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Notifications
{
    public class NotificationPreference
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool AutoConfirmMealPlanDeductions { get; set; }
        public decimal DefaultLowStockThreshold { get; set; }
        public int DefaultReplenishmentDays { get; set; }
    }
}
