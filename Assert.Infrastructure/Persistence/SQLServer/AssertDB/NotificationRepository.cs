using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly InfraAssertDbContext _context;

        public NotificationRepository(InfraAssertDbContext context)
        {
            _context = context;
        }

        public async Task<TnNotification> GetByIdAsync(long notificationId)
        {
            return await _context.TnNotifications
                .Include(n => n.Type)
                .Include(n => n.TnNotificationActions)
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<List<TnNotification>> GetUserNotificationsAsync(int userId, int page, int pageSize)
        {
            return await _context.TnNotifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(n => n.Type)
                .Include(n => n.TnNotificationActions)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.TnNotifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task MarkAsReadAsync(long notificationId)
        {
            var notification = await _context.TnNotifications
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var unreadNotifications = await _context.TnNotifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<TnNotification> CreateNotificationAsync(TnNotification notification)
        {
            _context.TnNotifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task AddNotificationActionAsync(TnNotificationAction action)
        {
            _context.TnNotificationActions.Add(action);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TnNotification>> GetHistoricalNotificationsAsync(
        int userId,
        DateTime? fromDate,
        DateTime? toDate,
        string typeFilter,
        int page,
        int pageSize)
        {
            var query = _context.TnNotifications
                .Where(n => n.UserId == userId)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(n => n.CreatedAt >= fromDate);

            if (toDate.HasValue)
                query = query.Where(n => n.CreatedAt <= toDate);

            if (!string.IsNullOrEmpty(typeFilter))
                query = query.Where(n => n.Type.Nname == typeFilter);

            return await query
                .OrderByDescending(n => n.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(n => n.Type)
                .Include(n => n.TnNotificationActions)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> GetHistoricalNotificationsCountAsync(
            int userId,
            DateTime? fromDate,
            DateTime? toDate,
            string typeFilter)
        {
            var query = _context.TnNotifications
                .Where(n => n.UserId == userId)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(n => n.CreatedAt >= fromDate);

            if (toDate.HasValue)
                query = query.Where(n => n.CreatedAt <= toDate);

            if (!string.IsNullOrEmpty(typeFilter))
                query = query.Where(n => n.Type.Nname == typeFilter);

            return await query.CountAsync();
        }


        public Task<List<TnNotification>> GetUnreadNotificationsAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
