using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;

namespace Assert.Application.Interfaces
{
    public interface IAppBookService
    {
        Task<ReturnModelDTO<PayPriceCalculationDTO>> CalculatePrice(long listingRentId, DateTime startDate, DateTime endDate, int guestId,
           Dictionary<string, string> clientData, bool useTechnicalMessages);

        Task<ReturnModelDTO<long>> UpsertBookAsync(BookDTO incomingBook);

        Task<ReturnModelDTO<BookDTO>> GetBookByIdAsync(long bookId);

        Task<ReturnModelDTO<List<BookDTO>>> GetBooksByUserIdAsync();
        Task<ReturnModelDTO<BookDTO>> SimulatePayment(PaymentModel request, int userId, Dictionary<string, string> requestInfo, bool useTechnicalMessages);
    }
}
