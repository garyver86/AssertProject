using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Common.Metadata;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Infrastructure.Exceptions;
using Assert.Infrastructure.Persistence.SQLServer.AssertDB;
using AutoMapper;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assert.Application.Services
{
    public class AppBookService(
        IBookService _bookService,
        IBookRepository _bookRespository,
        IMapper _mapper,
        IErrorHandler _errorHandler,
        RequestMetadata _metadata,
        ISystemConfigurationRepository _systemConfigurationRepository,
        IHttpContextAccessor requestContext) : IAppBookService
    {
        public async Task<ReturnModelDTO<(PayPriceCalculationDTO, List<PriceBreakdownItemDTO>)>> CalculatePrice(
            long listingRentId, DateTime startDate, DateTime endDate,
            int guestId, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModelDTO<(PayPriceCalculationDTO, List<PriceBreakdownItemDTO>)> result = new ReturnModelDTO<(PayPriceCalculationDTO, List<PriceBreakdownItemDTO>)>();
            try
            {
                ReturnModel<(PayPriceCalculation, List<PriceBreakdownItem>)> returnModel = await _bookService.CalculatePrice(listingRentId, startDate, endDate, guestId, clientData, useTechnicalMessages);

                if (returnModel.StatusCode == ResultStatusCode.OK)
                {
                    result = new ReturnModelDTO<(PayPriceCalculationDTO, List<PriceBreakdownItemDTO>)>
                    {
                        Data = (_mapper.Map<PayPriceCalculationDTO>(returnModel.Data.Item1), _mapper.Map<List<PriceBreakdownItemDTO>>(returnModel.Data.Item2)),
                        HasError = false,
                        StatusCode = ResultStatusCode.OK
                    };
                }
                else
                {
                    result.StatusCode = returnModel.StatusCode;
                    result.HasError = true;
                    result.ResultError = _mapper.Map<ErrorCommonDTO>(returnModel.ResultError);
                }

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppBookService.CalculatePrice", ex, new { listingRentId, startDate, endDate, guestId, clientData }, useTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO<BookDTO>> GetBookByIdAsync(long bookId)
        {
            var book = await _bookRespository.GetByIdAsync(bookId);
            if (book is null)
                throw new NotFoundException($"La reserva con ID {bookId} no fue encontrada. Verifique e intente nuevamente.");

            var bookDto = _mapper.Map<BookDTO>(book);
            return new ReturnModelDTO<BookDTO>
            {
                Data = bookDto,
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        public async Task<ReturnModelDTO<List<BookDTO>>> GetBooksByUserIdAsync()
        {
            var books = await _bookRespository.GetByUserId(_metadata.UserId);

            if (!(books is { Count: > 0 }))
                throw new KeyNotFoundException($"No existen reservas para el usuario con ID {_metadata.UserId}.");

            string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
            _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");
            foreach (var book in books)
            {
                if (book.ListingRent?.TlListingPhotos?.Count > 0)
                {
                    foreach (var item in book.ListingRent?.TlListingPhotos)
                    {
                        item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                    }

                }
            }

            var bookDtos = _mapper.Map<List<BookDTO>>(books);

            return new ReturnModelDTO<List<BookDTO>>
            {
                Data = bookDtos,
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        public async Task<ReturnModelDTO<BookDTO>> SimulatePayment(PaymentModel request, int userId, Dictionary<string, string> requestInfo, bool useTechnicalMessages)
        {
            ReturnModelDTO<BookDTO> result = new ReturnModelDTO<BookDTO>();
            try
            {

                PaymentRequest req = new PaymentRequest
                {
                    Amount = request.Amount,
                    CalculationCode = request.CalculationCode,
                    CountryId = request.CountryId,
                    CurrencyCode = request.CurrencyCode,
                    MethodOfPaymentId = request.MethodOfPaymentId,
                    OrderCode = request.OrderCode,
                    PaymentData = request.PaymentData,
                    PaymentProviderId = request.PaymentProviderId,
                    Stan = request.Stan,
                    TransactionData = request.TransactionData
                };
                var _result = await _bookService.RegisterPaymentAndCreateBooking(req, userId, requestInfo, useTechnicalMessages);
                if (_result.StatusCode == ResultStatusCode.OK)
                {
                    result = new ReturnModelDTO<BookDTO>
                    {
                        Data = _mapper.Map<BookDTO>(_result.Data),
                        HasError = false,
                        StatusCode = ResultStatusCode.OK
                    };
                }
                else
                {
                    result.StatusCode = _result.StatusCode;
                    result.HasError = true;
                    result.ResultError = _mapper.Map<ErrorCommonDTO>(_result.ResultError);
                }

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppBookService.SimulatePayment", ex, new { request, userId, requestInfo, useTechnicalMessages }, useTechnicalMessages));
            }
            return result;
        }

        public async Task<ReturnModelDTO<long>> UpsertBookAsync(BookDTO incomingBook)
        {
            var book = _mapper.Map<TbBook>(incomingBook);
            var bookId = await _bookRespository.UpsertBookAsync(book);

            return new ReturnModelDTO<long>
            {
                Data = bookId,
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        public async Task<ReturnModelDTO<List<BookDTO>>> GetBooksWithoutReviewByUser(int userId)
        {
            try
            {
                var books = await _bookService.GetBooksWithoutReviewByUser(userId);

                string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");
                foreach (var book in books)
                {
                    if (book.ListingRent?.TlListingPhotos?.Count > 0)
                    {
                        foreach (var item in book.ListingRent?.TlListingPhotos)
                        {
                            item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                        }

                    }
                }

                var bookDtos = _mapper.Map<List<BookDTO>>(books);
                return new ReturnModelDTO<List<BookDTO>>
                {
                    Data = bookDtos,
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                ReturnModelDTO<List<BookDTO>> result = new ReturnModelDTO<List<BookDTO>>();
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppBookService.GetBooksWithoutReviewByUser", ex, new { userId }, true));
                return result;
            }
        }

        public async Task<ReturnModelDTO<BookDTO>> CancelBooking(int userId, long bookId)
        {
            try
            {
                var book = await _bookService.Cancel(userId, bookId);

                string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");

                if (book.ListingRent?.TlListingPhotos?.Count > 0)
                {
                    foreach (var item in book.ListingRent?.TlListingPhotos)
                    {
                        item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                    }

                }

                var bookDtos = _mapper.Map<BookDTO>(book);
                return new ReturnModelDTO<BookDTO>
                {
                    Data = bookDtos,
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                ReturnModelDTO<BookDTO> result = new ReturnModelDTO<BookDTO>();
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppBookService.CancelBooking", ex, new { userId }, true));
                return result;
            }
        }

        public async Task<ReturnModelDTO<List<BookDTO>>> GetPendingAcceptance(int userId)
        {
            try
            {
                var books = await _bookService.GetPendingAcceptance(userId);

                string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");

                foreach (var book in books)
                {
                    if (book.ListingRent?.TlListingPhotos?.Count > 0)
                    {
                        foreach (var item in book.ListingRent?.TlListingPhotos)
                        {
                            item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                        }

                    }
                }
                var bookDtos = _mapper.Map<List<BookDTO>>(books);
                return new ReturnModelDTO<List<BookDTO>>
                {
                    Data = bookDtos,
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                ReturnModelDTO<List<BookDTO>> result = new ReturnModelDTO<List<BookDTO>>();
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppBookService.GetPendingAcceptance", ex, new { userId }, true));
                return result;
            }
        }

        public async Task<ReturnModelDTO<List<BookDTO>>> GetPendingAcceptanceForRenter(int userId)
        {
            try
            {
                var books = await _bookService.GetPendingAcceptanceForRenter(userId);

                string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");

                foreach (var book in books)
                {
                    if (book.ListingRent?.TlListingPhotos?.Count > 0)
                    {
                        foreach (var item in book.ListingRent?.TlListingPhotos)
                        {
                            item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                        }

                    }
                }
                var bookDtos = _mapper.Map<List<BookDTO>>(books);
                return new ReturnModelDTO<List<BookDTO>>
                {
                    Data = bookDtos,
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                ReturnModelDTO<List<BookDTO>> result = new ReturnModelDTO<List<BookDTO>>();
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppBookService.GetPendingAcceptanceForRenter", ex, new { userId }, true));
                return result;
            }
        }

        public async Task<ReturnModelDTO<List<BookDTO>>> GetCancelablesBookings(int userId)
        {
            try
            {
                var books = await _bookService.GetCancelablesBookings(userId);

                string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");

                foreach (var book in books)
                {
                    if (book.ListingRent?.TlListingPhotos?.Count > 0)
                    {
                        foreach (var item in book.ListingRent?.TlListingPhotos)
                        {
                            item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                        }

                    }
                }
                var bookDtos = _mapper.Map<List<BookDTO>>(books);
                return new ReturnModelDTO<List<BookDTO>>
                {
                    Data = bookDtos,
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                ReturnModelDTO<List<BookDTO>> result = new ReturnModelDTO<List<BookDTO>>();
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppBookService.GetCancelablesBookings", ex, new { userId }, true));
                return result;
            }
        }

        public async Task<ReturnModelDTO<List<BookDTO>>> GetApprovedsWOInit(int userId)
        {
            try
            {
                var books = await _bookService.GetApprovedsWOInit(userId);

                string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");

                foreach (var book in books)
                {
                    if (book.ListingRent?.TlListingPhotos?.Count > 0)
                    {
                        foreach (var item in book.ListingRent?.TlListingPhotos)
                        {
                            item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                        }

                    }
                }
                var bookDtos = _mapper.Map<List<BookDTO>>(books);
                return new ReturnModelDTO<List<BookDTO>>
                {
                    Data = bookDtos,
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                ReturnModelDTO<List<BookDTO>> result = new ReturnModelDTO<List<BookDTO>>();
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppBookService.GetApprovedsWOInit", ex, new { userId }, true));
                return result;
            }
        }


        public async Task<ReturnModelDTO<BookDTO>> AuthorizationResponse(int userId, long bookId, bool isApproval)
        {
            try
            {
                var book = await _bookService.AuthorizationResponse(userId, bookId, isApproval);

                string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");

                if (book.ListingRent?.TlListingPhotos?.Count > 0)
                {
                    foreach (var item in book.ListingRent?.TlListingPhotos)
                    {
                        item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                    }

                }
                var bookDtos = _mapper.Map<BookDTO>(book);
                return new ReturnModelDTO<BookDTO>
                {
                    Data = bookDtos,
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                ReturnModelDTO<BookDTO> result = new ReturnModelDTO<BookDTO>();
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppBookService.AuthorizationResponse", ex, new { userId }, true));
                return result;
            }
        }
        public async Task<ReturnModelDTO<BookDTO>> BookingRequestApproval(Guid CalculationCode,
            int userId,
            Dictionary<string, string> clientData,
            bool useTechnicalMessages)
        {
            try
            {
                var books = await _bookService.BookingRequestApproval(CalculationCode,
                    userId,
                    clientData,
                    useTechnicalMessages);

                string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");

                if (books.Data?.ListingRent?.TlListingPhotos?.Count > 0)
                {
                    foreach (var item in books.Data.ListingRent?.TlListingPhotos)
                    {
                        item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                    }

                }

                var bookDtos = _mapper.Map<BookDTO>(books.Data);
                return new ReturnModelDTO<BookDTO>
                {
                    Data = bookDtos,
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                ReturnModelDTO<BookDTO> result = new ReturnModelDTO<BookDTO>();
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppBookService.BookingRequestApproval", ex, new { userId }, true));
                return result;
            }
        }

    }
}
