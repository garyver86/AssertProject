using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Common.Metadata;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Infrastructure.Exceptions;
using AutoMapper;

namespace Assert.Application.Services
{
    public class AppBookService(
        IBookService _bookService,
        IBookRepository _bookRespository,
        IMapper _mapper,
        IErrorHandler _errorHandler,
        RequestMetadata _metadata) : IAppBookService
    {
        public async Task<ReturnModelDTO<PayPriceCalculationDTO>> CalculatePrice(
            long listingRentId, DateTime startDate, DateTime endDate,
            int guestId, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModelDTO<PayPriceCalculationDTO> result = new ReturnModelDTO<PayPriceCalculationDTO>();
            try
            {
                ReturnModel<PayPriceCalculation> returnModel = await _bookService.CalculatePrice(listingRentId, startDate, endDate, guestId, clientData, useTechnicalMessages);

                if (returnModel.StatusCode == ResultStatusCode.OK)
                {
                    result = new ReturnModelDTO<PayPriceCalculationDTO>
                    {
                        Data = _mapper.Map<PayPriceCalculationDTO>(returnModel.Data),
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


    }
}
