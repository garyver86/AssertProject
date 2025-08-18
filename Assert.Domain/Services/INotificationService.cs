using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Services
{
    public interface INotificationService
    {
        Task<TnNotification> GetNotificationAsync(int notificationId, int userId);
        Task<(List<TnNotification>, PaginationMetadata)> GetUserNotificationsAsync(int userId, int page, int pageSize);
        Task<int> GetUnreadCountAsync(int userId);
        Task MarkAsReadAsync(int notificationId, int userId);
        Task MarkAllAsReadAsync(int userId);
        Task<(List<TnNotification>, PaginationMetadata)> GetHistoricalNotificationsAsync(
            int userId,
            NotificationHistoryFilter filter);

        // Métodos para crear notificaciones específicas
        Task SendBookingRequestNotificationAsync(int hostId, int propertyId, int bookingId);
        Task SendBookingApprovedNotificationAsync(int renterId, int propertyId, int bookingId);
        Task SendReviewReminderNotificationAsync(int renterId, int propertyId, int bookingId);
    }
}
