using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces.Notifications;
using Assert.Domain.Models;
using Assert.Domain.Services;
using AutoMapper;

namespace Assert.Application.Services.Notifications
{
    public class AppNotificationService : IAppNotificationService
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;
        public AppNotificationService(INotificationService notificationService, IMapper mapper, IErrorHandler errorHandler)
        {
            _notificationService = notificationService;
            _mapper = mapper;
            _errorHandler = errorHandler;
        }
        public async Task<ReturnModelDTO<(List<NotificationDTO>, PaginationMetadataDTO)>> GetHistoricalNotificationsAsync(int userId, NotificationHistoryFilter filter, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModelDTO<(List<NotificationDTO>, PaginationMetadataDTO)> result = new ReturnModelDTO<(List<NotificationDTO>, PaginationMetadataDTO)>();
            try
            {
                var historical = await _notificationService.GetHistoricalNotificationsAsync(userId, filter);
                result = new ReturnModelDTO<(List<NotificationDTO>, PaginationMetadataDTO)>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = (historical.Item1.Select(n => _mapper.Map<NotificationDTO>(n)).ToList(), _mapper.Map<PaginationMetadataDTO>(historical.Item2))
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppNotificationService.GetHistoricalNotificationsAsync", ex, new { userId, filter, clientData }, useTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO<NotificationDTO>> GetNotificationAsync(long notificationId, int userId, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModelDTO<NotificationDTO> result = new ReturnModelDTO<NotificationDTO>();
            try
            {
                var detail = await _notificationService.GetNotificationAsync(notificationId, userId);
                result = new ReturnModelDTO<NotificationDTO>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = _mapper.Map<NotificationDTO>(detail)
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppNotificationService.GetNotificationAsync", ex, new { userId, notificationId, clientData }, useTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO<int>> GetUnreadCountAsync(int userId, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModelDTO<int> result = new ReturnModelDTO<int>();
            try
            {
                var detail = await _notificationService.GetUnreadCountAsync(userId);
                result = new ReturnModelDTO<int>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = detail
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppNotificationService.GetUnreadCountAsync", ex, new { userId, clientData }, useTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO<(List<NotificationDTO>, PaginationMetadataDTO)>> GetUserNotificationsAsync(int userId, int page, int pageSize, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModelDTO<(List<NotificationDTO>, PaginationMetadataDTO)> result = new ReturnModelDTO<(List<NotificationDTO>, PaginationMetadataDTO)>();
            try
            {
                var historical = await _notificationService.GetUserNotificationsAsync(userId, page, pageSize);
                result = new ReturnModelDTO<(List<NotificationDTO>, PaginationMetadataDTO)>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = false ? (historical.Item1.Select(n => _mapper.Map<NotificationDTO>(n)).ToList(), _mapper.Map<PaginationMetadataDTO>(historical.Item2)) : (new List<NotificationDTO>(), new PaginationMetadataDTO())
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppNotificationService.GetUserNotificationsAsync", ex, new { userId, page, pageSize, clientData }, useTechnicalMessages));
            }
            return result;
        }

        public async Task MarkAllAsReadAsync(int userId, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            try
            {
                await _notificationService.MarkAllAsReadAsync(userId);
                return;
            }
            catch (Exception ex)
            {
                throw new Exception("Error marking all notifications as read", ex);
            }
        }

        public async Task MarkAsReadAsync(long notificationId, int userId, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            try
            {
                await _notificationService.MarkAsReadAsync(notificationId, userId);
                return;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error marking {notificationId} as read", ex);
            }
        }

        public async Task SendNewMessageNotificationAsync(string messageBody, int userId, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            try
            {
                await _notificationService.SendNewMessageNotificationAsync(userId, messageBody);
                return;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending notification to {userId}.", ex);
            }
        }
    }
}
