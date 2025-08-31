using Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Notifications;

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Services.Interfaces
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
