using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IConversationRepository
    {
        Task<TmConversation> Create(int renterId, int hostId);
        //Task<List<TmMessage>> Get(long conversationId, int page, int pageSize, string orderBy);
        Task<List<TmConversation>> Get(int userId);
        Task<TmConversation> GetConversation(long conversationId);
    }
}
