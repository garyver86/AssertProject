using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.ValueObjects;

namespace Assert.Domain.Repositories
{
    public interface IConversationRepository
    {
        Task<TmConversation> Create(int renterId, int? hostId, long? bookId, long? priceCalculationId, long? listingId);
        //Task<List<TmMessage>> Get(long conversationId, int page, int pageSize, string orderBy);
        Task<List<TmConversation>> Get(int userId);
        Task<TmConversation> GetConversation(long conversationId);
        Task<(List<TmConversation> Conversations, PaginationMetadata pagination)> SearchConversations(ConversationFilter filter);
        Task<TmConversation> SetFeatured(long conversationId, int userid, bool isFeatured);
        Task<TmConversation> SetArchived(long conversationId, int userid, bool isArchived);
        Task<TmConversation> SetSilent(long conversationId, int userid, bool isSilent);
        Task<(List<TmConversation> Conversations, PaginationMetadata pagination)> SearchConversationsArchiveds(ConversationFilter filter);
        Task<List<TmConversation>> GetArchiveds(int userId);
        Task<TmConversation> SetUnread(long conversationId, int userid, bool isUnread);
    }
}
