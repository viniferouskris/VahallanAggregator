using Microsoft.EntityFrameworkCore;
using Vahallan_Ingredient_Aggregator.Data;
using Vahallan_Ingredient_Aggregator.Models.Notifications;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;

namespace Vahallan_Ingredient_Aggregator.Services.Implementaions
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateNotificationAsync(string userId, string title, string message, NotificationType type)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.ReadAt.HasValue)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.ReadAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ProcessExpirationAlertsAsync()
        {
            var warningDays = 7;
            var thresholdDate = DateTime.UtcNow.AddDays(warningDays);

            var expiringItems = await _context.PantryItems
                .Where(p => p.ExpirationDate <= thresholdDate)
                .ToListAsync();

            foreach (var item in expiringItems)
            {
                await CreateNotificationAsync(
                    item.UserId,
                    "Expiration Warning",
                    $"{item.IngredientName} will expire on {item.ExpirationDate:d}",
                    NotificationType.ExpirationWarning
                );
            }
        }

        public async Task ProcessLowStockAlertsAsync()
        {
            var lowStockItems = await _context.PantryItems
                .Where(p => p.CurrentQuantity < p.LowStockThreshold)
                .ToListAsync();

            foreach (var item in lowStockItems)
            {
                await CreateNotificationAsync(
                    item.UserId,
                    "Low Stock Alert",
                    $"Running low on {item.IngredientName}",
                    NotificationType.LowStock
                );
            }
        }
    }

}
