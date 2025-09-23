using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IMessageRepository
    {
        Task<List<TmMessage>> Get(long conversationId, int page, int pageSize, string orderBy, int userId);
        Task<TmMessage> Send(long conversationId, int? userId, string body, int messageType, int? onlyForUserId);
        Task<TmMessage> SetAsRead(long conversationId, List<long>? messagesId);
        Task<TmMessage> SetAsUnread(long conversationId, List<long>? messagesId);
        Task<int> GetUnreadCount(int userId);
    }
}
