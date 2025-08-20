using Assert.Application.DTOs.Responses;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.ValueObjects;

namespace Assert.Application.Interfaces.Notifications
{
    public interface IAppNotificationService
    {
        Task<ReturnModelDTO<(List<NotificationDTO>, PaginationMetadataDTO)>> GetHistoricalNotificationsAsync(int userId, NotificationHistoryFilter filter, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO<NotificationDTO>> GetNotificationAsync(long notificationId, int userId, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO<int>> GetUnreadCountAsync(int userId, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO<(List<NotificationDTO>, PaginationMetadataDTO)>> GetUserNotificationsAsync(int userId, int page, int pageSize, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task MarkAllAsReadAsync(int userId, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task MarkAsReadAsync(long notificationId, int userId, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task SendNewMessageNotificationAsync(string messageBody, int userId, Dictionary<string, string> clientData, bool useTechnicalMessages);
    }
}
