using Assert.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.Interfaces
{
    public interface IAppMessagingService
    {
        Task<ReturnModelDTO<ConversationDTO>> CreateConversation(int renterid, int hostId, int userId, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO<List<MessageDTO>>> GetConversationMessages(int conversationId, int userId, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO<List<ConversationDTO>>> GetConversations(int userId, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO> GetUnreadCount(int userId, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO> SendMessage(int conversationId1, int userId, int conversationId2, object body, object messageTypeId, object bookId, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO> SetAsRead(int conversationId1, int userId, int conversationId2, int[]? messageIds, Dictionary<string, string> requestInfo);
        Task<ReturnModelDTO> SetAsUnread(int conversationId1, int userId, int conversationId2, int[]? messageIds, Dictionary<string, string> requestInfo);
    }
}
