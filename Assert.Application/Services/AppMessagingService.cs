using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.ValueObjects;
using Assert.Infrastructure.Persistence.SQLServer.AssertDB;
using AutoMapper;
using Microsoft.VisualBasic;

namespace Assert.Application.Services
{
    public class AppMessagingService : IAppMessagingService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly INotificationService _notificationService;
        private readonly IMessagePredefinedRepository _messagePredefinedRepository;

        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;
        public AppMessagingService(IConversationRepository conversationRepository, IMessageRepository messageRepository,
            IMapper mapper, IErrorHandler errorHandler, INotificationService notificationService, IMessagePredefinedRepository messagePredefinedRepository)
        {
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
            _errorHandler = errorHandler;
            _notificationService = notificationService;
            _messagePredefinedRepository = messagePredefinedRepository;
        }
        public async Task<ReturnModelDTO<ConversationDTO>> CreateConversation(int renterid, int hostId, int userId, long? bookId, long? priceCalculationId, long? listingId, Dictionary<string, string> requestInfo)
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
                    var result_ = await _conversationRepository.Create(renterid, hostId, bookId, priceCalculationId, listingId);

                    result = new ReturnModelDTO<ConversationDTO>
                    {
                        StatusCode = ResultStatusCode.OK,
                        HasError = false,
                        Data = _mapper.Map<ConversationDTO>(result_)
                    };

                    if (result_.PriceCalculation?.CalculationStatusId == 4)
                    {
                        var messagePredefined = await _messagePredefinedRepository.GetByCode("CON_WORES");
                        if (messagePredefined != null)
                        {
                            string messageHost = string.Format(messagePredefined.MessageBody, result_.ListingRent?.MaxGuests > 1 ? result_.ListingRent?.MaxGuests.ToString() + " huespedes" : result_.ListingRent?.MaxGuests.ToString() + " huesped", result_.PriceCalculation?.InitBook?.ToString("dd/MM/yyyy") + " al " + result_.PriceCalculation?.EndBook?.ToString("dd/MM/yyyy"));
                            await SendMessage(result_.ConversationId, null, messageHost, 4, requestInfo, hostId);


                            string messageRenter = string.Format(messagePredefined.MessageBodyDest ?? messagePredefined.MessageBody, result_.ListingRent?.MaxGuests > 1 ? result_.ListingRent?.MaxGuests.ToString() + " huespedes" : result_.ListingRent?.MaxGuests.ToString() + " huesped", result_.PriceCalculation?.InitBook?.ToString("dd/MM/yyyy") + " al " + result_.PriceCalculation?.EndBook?.ToString("dd/MM/yyyy"));
                            await SendMessage(result_.ConversationId, null, messageRenter, 4, requestInfo, renterid);
                        }
                    }
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

        public async Task<ReturnModelDTO<ConversationDTO>> GetConversation(long conversationId, int userId, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO<ConversationDTO> result = new ReturnModelDTO<ConversationDTO>();
            try
            {
                TmConversation result_ = await _conversationRepository.GetConversation(conversationId);

                if (result_ != null && result_.UserIdOne != userId && result_.UserIdTwo != userId)
                {
                    throw new Exception("No cuenta con los permisos para recuperar esta converzación.");
                }

                ConversationDTO conversation = _mapper.Map<ConversationDTO>(result_);

                if (conversation != null && userId == conversation.UserIdOne)
                {
                    conversation.Archived = result_.UserOneArchived;
                    conversation.Silent = result_.UserOneSilent;
                    conversation.Featured = result_.UserOneFeatured;
                    conversation.ArchivedDateTime = result_.UserOneArchivedDateTime;
                    conversation.FeaturedDateTime = result_.UserOneFeaturedDateTime;
                    conversation.SilentDateTime = result_.UserOneSilentDateTime;
                }
                else if (conversation != null && userId == conversation.UserIdTwo)
                {
                    conversation.Archived = result_.UserTwoArchived;
                    conversation.Silent = result_.UserTwoSilent;
                    conversation.Featured = result_.UserTwoFeatured;
                    conversation.FeaturedDateTime = result_.UserTwoFeaturedDateTime;
                    conversation.ArchivedDateTime = result_.UserTwoArchivedDateTime;
                    conversation.SilentDateTime = result_.UserTwoSilentDateTime;
                }

                result = new ReturnModelDTO<ConversationDTO>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = conversation
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

                List<ConversationDTO> conversations = _mapper.Map<List<ConversationDTO>>(result_);

                if (conversations.Count > 0)
                {
                    foreach (var conversation in conversations)
                    {
                        var conv = result_.FirstOrDefault(c => c.ConversationId == conversation.ConversationId);
                        if (conversation != null && userId == conversation.UserIdOne)
                        {
                            conversation.Archived = conv.UserOneArchived;
                            conversation.Silent = conv.UserOneSilent;
                            conversation.Featured = conv.UserOneFeatured;
                            conversation.ArchivedDateTime = conv.UserOneArchivedDateTime;
                            conversation.FeaturedDateTime = conv.UserOneFeaturedDateTime;
                            conversation.SilentDateTime = conv.UserOneSilentDateTime;
                        }
                        else if (conversation != null && userId == conversation.UserIdTwo)
                        {
                            conversation.Archived = conv.UserTwoArchived;
                            conversation.Silent = conv.UserTwoSilent;
                            conversation.Featured = conv.UserTwoFeatured;
                            conversation.FeaturedDateTime = conv.UserTwoFeaturedDateTime;
                            conversation.ArchivedDateTime = conv.UserTwoArchivedDateTime;
                            conversation.SilentDateTime = conv.UserTwoSilentDateTime;
                        }
                    }
                }
                result = new ReturnModelDTO<List<ConversationDTO>>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = conversations
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


        public async Task<(ReturnModelDTO<List<ConversationDTO>>, PaginationMetadataDTO)> SearchConversations(ConversationFilterDTO filter, Dictionary<string, string> requestInfo)
        {
            ReturnModelDTO<List<ConversationDTO>> result = new ReturnModelDTO<List<ConversationDTO>>();
            PaginationMetadataDTO pagination = null;
            try
            {
                ConversationFilter _filter = _mapper.Map<ConversationFilter>(filter);
                (List<TmConversation>, PaginationMetadata) result_ = await _conversationRepository.SearchConversations(_filter);

                List<ConversationDTO> conversations = _mapper.Map<List<ConversationDTO>>(result_.Item1);

                if (conversations.Count > 0)
                {
                    foreach (var conversation in conversations)
                    {
                        var conv = result_.Item1.FirstOrDefault(c => c.ConversationId == conversation.ConversationId);
                        if (conversation != null && filter.UserId == conversation.UserIdOne)
                        {
                            conversation.Archived = conv.UserOneArchived;
                            conversation.Silent = conv.UserOneSilent;
                            conversation.Featured = conv.UserOneFeatured;
                            conversation.ArchivedDateTime = conv.UserOneArchivedDateTime;
                            conversation.FeaturedDateTime = conv.UserOneFeaturedDateTime;
                            conversation.SilentDateTime = conv.UserOneSilentDateTime;
                        }
                        else if (conversation != null && filter.UserId == conversation.UserIdTwo)
                        {
                            conversation.Archived = conv.UserTwoArchived;
                            conversation.Silent = conv.UserTwoSilent;
                            conversation.Featured = conv.UserTwoFeatured;
                            conversation.FeaturedDateTime = conv.UserTwoFeaturedDateTime;
                            conversation.ArchivedDateTime = conv.UserTwoArchivedDateTime;
                            conversation.SilentDateTime = conv.UserTwoSilentDateTime;
                        }
                    }
                }
                result = new ReturnModelDTO<List<ConversationDTO>>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = conversations
                };
                pagination = _mapper.Map<PaginationMetadataDTO>(result_.Item2);
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppMessagingService.SearchConversations", ex, new { filter, requestInfo }, false));
            }
            return (result, pagination);
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

        public async Task<ReturnModelDTO<MessageDTO>> SendMessage(long conversationId, int? userId, string body, int messageTypeId, Dictionary<string, string> requestInfo, int? onlyForUserId)
        {
            ReturnModelDTO<MessageDTO> result = new ReturnModelDTO<MessageDTO>();
            try
            {
                TmMessage result_ = await _messageRepository.Send(conversationId, userId, body, messageTypeId, onlyForUserId);
                var conversation = await _conversationRepository.GetConversation(conversationId);

                bool sendNotification = true;
                if(
                    ((conversation.UserOneSilent??false) && (conversation.UserTwoSilent ?? false)) ||
                    ((conversation.UserOneSilent ?? false) && conversation.UserIdOne == userId) ||
                    ((conversation.UserTwoSilent ?? false) && conversation.UserIdTwo == userId) ||
                    ((conversation.UserOneArchived ?? false) && (conversation.UserTwoArchived ?? false)) ||
                    ((conversation.UserOneArchived ?? false) && conversation.UserIdOne == userId) ||
                    ((conversation.UserTwoArchived ?? false) && conversation.UserIdTwo == userId)
                 )
                {
                    sendNotification = false;
                }
                if (sendNotification)
                {
                    await _notificationService.SendNewMessageNotificationAsync(onlyForUserId ?? (conversation.UserIdOne == userId ? conversation.UserIdTwo : conversation.UserIdOne), body);
                }
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

        public async Task<ReturnModelDTO<ConversationDTO>> SetFeatured(long conversationId, int userid, bool isFeatured)
        {
            ReturnModelDTO<ConversationDTO> result = new ReturnModelDTO<ConversationDTO>();
            try
            {
                TmConversation result_ = await _conversationRepository.SetFeatured(conversationId, userid, isFeatured);

                ConversationDTO conversation = _mapper.Map<ConversationDTO>(result_);

                if (conversation != null && userid == conversation.UserIdOne)
                {
                    conversation.Archived = result_.UserOneArchived;
                    conversation.Silent = result_.UserOneSilent;
                    conversation.Featured = result_.UserOneFeatured;
                    conversation.ArchivedDateTime = result_.UserOneArchivedDateTime;
                    conversation.FeaturedDateTime = result_.UserOneFeaturedDateTime;
                    conversation.SilentDateTime = result_.UserOneSilentDateTime;
                }
                else if (conversation != null && userid == conversation.UserIdTwo)
                {
                    conversation.Archived = result_.UserTwoArchived;
                    conversation.Silent = result_.UserTwoSilent;
                    conversation.Featured = result_.UserTwoFeatured;
                    conversation.FeaturedDateTime = result_.UserTwoFeaturedDateTime;
                    conversation.ArchivedDateTime = result_.UserTwoArchivedDateTime;
                    conversation.SilentDateTime = result_.UserTwoSilentDateTime;
                }

                result = new ReturnModelDTO<ConversationDTO>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = conversation
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppMessagingService.SetFeatured", ex, new { conversationId, userid, isFeatured }, false));
            }
            return result;
        }

        public async Task<ReturnModelDTO<ConversationDTO>> SetArchived(long conversationId, int userid, bool isArchived)
        {
            ReturnModelDTO<ConversationDTO> result = new ReturnModelDTO<ConversationDTO>();
            try
            {
                TmConversation result_ = await _conversationRepository.SetArchived(conversationId, userid, isArchived);

                ConversationDTO conversation = _mapper.Map<ConversationDTO>(result_);

                if (conversation != null && userid == conversation.UserIdOne)
                {
                    conversation.Archived = result_.UserOneArchived;
                    conversation.Silent = result_.UserOneSilent;
                    conversation.Featured = result_.UserOneFeatured;
                    conversation.ArchivedDateTime = result_.UserOneArchivedDateTime;
                    conversation.FeaturedDateTime = result_.UserOneFeaturedDateTime;
                    conversation.SilentDateTime = result_.UserOneSilentDateTime;
                }
                else if (conversation != null && userid == conversation.UserIdTwo)
                {
                    conversation.Archived = result_.UserTwoArchived;
                    conversation.Silent = result_.UserTwoSilent;
                    conversation.Featured = result_.UserTwoFeatured;
                    conversation.FeaturedDateTime = result_.UserTwoFeaturedDateTime;
                    conversation.ArchivedDateTime = result_.UserTwoArchivedDateTime;
                    conversation.SilentDateTime = result_.UserTwoSilentDateTime;
                }

                result = new ReturnModelDTO<ConversationDTO>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = conversation
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppMessagingService.SetArchived", ex, new { conversationId, userid, isArchived }, false));
            }
            return result;
        }

        public async Task<ReturnModelDTO<ConversationDTO>> SetSilent(long conversationId, int userid, bool isSilent)
        {
            ReturnModelDTO<ConversationDTO> result = new ReturnModelDTO<ConversationDTO>();
            try
            {
                TmConversation result_ = await _conversationRepository.SetSilent(conversationId, userid, isSilent);

                ConversationDTO conversation = _mapper.Map<ConversationDTO>(result_);

                if (conversation != null && userid == conversation.UserIdOne)
                {
                    conversation.Archived = result_.UserOneArchived;
                    conversation.Silent = result_.UserOneSilent;
                    conversation.Featured = result_.UserOneFeatured;
                    conversation.ArchivedDateTime = result_.UserOneArchivedDateTime;
                    conversation.FeaturedDateTime = result_.UserOneFeaturedDateTime;
                    conversation.SilentDateTime = result_.UserOneSilentDateTime;
                }
                else if (conversation != null && userid == conversation.UserIdTwo)
                {
                    conversation.Archived = result_.UserTwoArchived;
                    conversation.Silent = result_.UserTwoSilent;
                    conversation.Featured = result_.UserTwoFeatured;
                    conversation.FeaturedDateTime = result_.UserTwoFeaturedDateTime;
                    conversation.ArchivedDateTime = result_.UserTwoArchivedDateTime;
                    conversation.SilentDateTime = result_.UserTwoSilentDateTime;
                }

                result = new ReturnModelDTO<ConversationDTO>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = conversation
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppMessagingService.SetSilent", ex, new { conversationId, userid, isSilent }, false));
            }
            return result;
        }
    }
}
