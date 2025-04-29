using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IMessageRepository
    {
        Task<List<TmMessage>> Get(int conversationId, int page, int pageSize, string orderBy);
        Task<TmMessage> Send(int conversationId, int userId, string body, int messageType, long? bookId);
        Task<TmMessage> SetAsRead(int conversationId, List<long>? messagesId);
        Task<TmMessage> SetAsUnread(int conversationId, List<long>? messagesId);
        Task<int> GetUnreadCount(int userId);
    }
}
