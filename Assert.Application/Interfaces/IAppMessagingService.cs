using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;

namespace Assert.Application.Interfaces
{
    public interface IAppMessagingService
    {
        Task<ReturnModelDTO<ConversationDTO>> CreateConversation(int renterid, int hostId, int userId, long? bookId, long? priceCalculationId, long? listingId, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO<ConversationDTO>> GetConversation(long conversationId, int userId, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO<List<MessageDTO>>> GetConversationMessages(int conversationId, int userId, int page, int pageSize, string orderBy, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO<List<ConversationDTO>>> GetConversations(int userId, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO> GetUnreadCount(int userId, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO<MessageDTO>> SendMessage(int conversationId, int userId, string body, int messageTypeId, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO> SetAsRead(int conversationId, int userId, List<long> messageIds, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO> SetAsUnread(int conversationId, int userId, List<long> messageIds, Dictionary<string, string> requestInfo);
        Task<(ReturnModelDTO<List<ConversationDTO>>, PaginationMetadataDTO)> SearchConversations(ConversationFilterDTO filter, Dictionary<string, string> requestInfo);
    }
}
