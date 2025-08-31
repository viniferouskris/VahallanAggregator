using Vahallan_Ingredient_Aggregator.Models.Notifications;

namespace Vahallan_Ingredient_Aggregator.Services.Interfaces
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(string userId, string title, string message, NotificationType type);
        Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(string userId);
        Task MarkAsReadAsync(int notificationId);
        Task ProcessExpirationAlertsAsync();
        Task ProcessLowStockAlertsAsync();
    }
}
