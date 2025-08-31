namespace Vahallan_Ingredient_Aggregator.Models.Notifications
{
    public class NotificationSettings
    {
        public bool EnableEmailNotifications { get; set; } = false;
        public bool EnablePushNotifications { get; set; } = true;
        public int DefaultExpirationWarningDays { get; set; } = 7;
        public decimal DefaultLowStockThreshold { get; set; } = 20;
    }
}
