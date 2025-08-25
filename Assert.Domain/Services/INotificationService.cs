using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
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
        Task<TnNotification> GetNotificationAsync(long notificationId, int userId);
        Task<(List<TnNotification>, PaginationMetadata)> GetUserNotificationsAsync(int userId, int page, int pageSize);
        Task<int> GetUnreadCountAsync(int userId);
        Task MarkAsReadAsync(long notificationId, int userId);
        Task MarkAllAsReadAsync(int userId);
        Task<(List<TnNotification>, PaginationMetadata)> GetHistoricalNotificationsAsync(
            int userId,
            NotificationHistoryFilter filter);
        Task SendBookingRequestNotificationAsync(int hostId, int propertyId, int bookingId);
        Task SendBookingApprovedNotificationAsync(int renterId, int propertyId, int bookingId);
        Task SendReviewReminderNotificationAsync(int renterId, int propertyId, int bookingId, bool isForHost);
        Task SendNewMessageNotificationAsync(int userIdTo, string messageBody);
        Task SendBookingPaymentNotificationAsync(long priceCalculationId);
        Task SendBookingPaymentRejectedNotificationAsync(long priceCalculationId);
        Task SendBookingPaymentPendingNotificationAsync(int userId, long priceCalculationId);
        Task SendNewMessageNotificationAsync(int userIdOrigin, int userIdTo, long? bookId);
    }
}
