using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface INotificationRepository
    {
        Task<TnNotification> GetByIdAsync(long notificationId);
        Task<List<TnNotification>> GetUserNotificationsAsync(int userId, int page, int pageSize);
        Task<int> GetUnreadCountAsync(int userId);
        Task<List<TnNotification>> GetUnreadNotificationsAsync(int userId);
        Task MarkAsReadAsync(long notificationId);
        Task MarkAllAsReadAsync(int userId);
        Task<TnNotification> CreateNotificationAsync(TnNotification notification);
        Task AddNotificationActionAsync(TnNotificationAction action);

        Task<List<TnNotification>> GetHistoricalNotificationsAsync(
       int userId,
       DateTime? fromDate,
       DateTime? toDate,
       string typeFilter,
       int page,
       int pageSize);

        Task<int> GetHistoricalNotificationsCountAsync(
            int userId,
            DateTime? fromDate,
            DateTime? toDate,
            string typeFilter);
    }
}
