using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;

namespace Assert.Application.Interfaces
{
    public interface IAppBookService
    {
        Task<ReturnModelDTO<(PayPriceCalculationDTO, List<PriceBreakdownItemDTO>)>> CalculatePrice(long listingRentId, DateTime startDate, DateTime endDate, int guestId,
           Dictionary<string, string> clientData, bool useTechnicalMessages);

        Task<ReturnModelDTO<long>> UpsertBookAsync(BookDTO incomingBook);

        Task<ReturnModelDTO<BookDTO>> GetBookByIdAsync(long bookId);

        Task<ReturnModelDTO<List<BookDTO>>> GetBooksByUserIdAsync();
        Task<ReturnModelDTO<BookDTO>> SimulatePayment(PaymentModel request, int userId, Dictionary<string, string> requestInfo, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<BookDTO>>> GetBooksWithoutReviewByUser(int userId);
        Task<ReturnModelDTO<BookDTO>> BookingRequestApproval(Guid CalculationCode,
            int userId,
            Dictionary<string, string> clientData,
            bool useTechnicalMessages);
        Task<ReturnModelDTO<BookDTO>> CancelBooking(int userId, long bookId);
        Task<ReturnModelDTO<List<BookDTO>>> GetPendingAcceptance(int userId);
        Task<ReturnModelDTO<List<BookDTO>>> GetCancelablesBookings(int userId);
        Task<ReturnModelDTO<List<BookDTO>>> GetApprovedsWOInit(int userId);
        Task<ReturnModelDTO<List<BookDTO>>> GetPendingAcceptanceForRenter(int userId);
        Task<ReturnModelDTO<BookDTO>> AuthorizationResponse(int userId, long bookId, bool isApproval, int? reasonRefused);
    }
}
