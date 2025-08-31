namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Notifications
{
    public class NotificationSettings
    {
        public bool EnableEmailNotifications { get; set; } = false;
        public bool EnablePushNotifications { get; set; } = true;
        public int DefaultExpirationWarningDays { get; set; } = 7;
        public decimal DefaultLowStockThreshold { get; set; } = 20;
    }
}
