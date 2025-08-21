using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using AutoMapper;

namespace Assert.Application.Services
{
    public class AppMessagingService : IAppMessagingService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly INotificationService _notificationService;

        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;
        public AppMessagingService(IConversationRepository conversationRepository, IMessageRepository messageRepository,
            IMapper mapper, IErrorHandler errorHandler, INotificationService notificationService)
        {
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
            _errorHandler = errorHandler;
            _notificationService = notificationService;
        }
        public async Task<ReturnModelDTO<ConversationDTO>> CreateConversation(int renterid, int hostId, int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO<ConversationDTO> result = new ReturnModelDTO<ConversationDTO>();
            try
            {
                if (renterid != userId && hostId != userId)
                {
                    result = new ReturnModelDTO<ConversationDTO>
                    {
                        StatusCode = ResultStatusCode.Unauthorized,
                        HasError = true,
                        ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetError(ResultStatusCode.Unauthorized, "El usuario actual no puede crear al converzación deseada.", true))
                    };
                }
                else
                {
                    var result_ = await _conversationRepository.Create(renterid, hostId);

                    result = new ReturnModelDTO<ConversationDTO>
                    {
                        StatusCode = ResultStatusCode.OK,
                        HasError = false,
                        Data = _mapper.Map<ConversationDTO>(result_)
                    };
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppMessagingService.CreateConversation", ex, new { renterid, hostId, userId, requestInfo }, false));
            }
            return result;
        }

        public async Task<ReturnModelDTO<List<MessageDTO>>> GetConversationMessages(int conversationId, int userId, int page, int pageSize, string orderBy, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO<List<MessageDTO>> result = new ReturnModelDTO<List<MessageDTO>>();
            try
            {
                List<TmMessage> result_ = await _messageRepository.Get(conversationId, page, pageSize, orderBy, userId);

                result = new ReturnModelDTO<List<MessageDTO>>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = _mapper.Map<List<MessageDTO>>(result_)
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppMessagingService.GetConversationMessages", ex, new { conversationId, page, pageSize, orderBy, userId, requestInfo }, false));
            }
            return result;
        }

        public async Task<ReturnModelDTO<List<ConversationDTO>>> GetConversations(int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO<List<ConversationDTO>> result = new ReturnModelDTO<List<ConversationDTO>>();
            try
            {
                List<TmConversation> result_ = await _conversationRepository.Get(userId);

                result = new ReturnModelDTO<List<ConversationDTO>>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = _mapper.Map<List<ConversationDTO>>(result_)
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppMessagingService.GetConversations", ex, new { userId, requestInfo }, false));
            }
            return result;
        }

        public async Task<ReturnModelDTO> GetUnreadCount(int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO result = new ReturnModelDTO();
            try
            {
                int result_ = await _messageRepository.GetUnreadCount(userId);

                result = new ReturnModelDTO
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = result_
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppMessagingService.GetUnreadCount", ex, new { userId, requestInfo }, false));
            }
            return result;
        }

        public async Task<ReturnModelDTO<MessageDTO>> SendMessage(int conversationId, int userId, string body, int messageTypeId, int? bookId, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO<MessageDTO> result = new ReturnModelDTO<MessageDTO>();
            try
            {
                TmMessage result_ = await _messageRepository.Send(conversationId, userId, body, messageTypeId, bookId);
                var conversation = await _conversationRepository.GetConversation(conversationId);
                await _notificationService.SendNewMessageNotificationAsync(conversation.UserIdOne == userId ? conversation.UserIdTwo : conversation.UserIdOne, body);

                result = new ReturnModelDTO<MessageDTO>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = _mapper.Map<MessageDTO>(result_)
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppMessagingService.SendMessage", ex, new { userId, requestInfo }, false));
            }
            return result;
        }

        public async Task<ReturnModelDTO> SetAsRead(int conversationId, int userId, List<long>? messageIds, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO result = new ReturnModelDTO();
            try
            {
                var result_ = await _messageRepository.SetAsRead(conversationId, messageIds);

                result = new ReturnModelDTO
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppMessagingService.SetAsRead", ex, new { conversationId, messageIds, userId, requestInfo }, false));
            }
            return result;
        }

        public async Task<ReturnModelDTO> SetAsUnread(int conversationId, int userId, List<long> messageIds, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO result = new ReturnModelDTO();
            try
            {
                var result_ = await _messageRepository.SetAsUnread(conversationId, messageIds);

                result = new ReturnModelDTO
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppMessagingService.SetAsRead", ex, new { conversationId, messageIds, userId, requestInfo }, false));
            }
            return result;
        }
    }
}
