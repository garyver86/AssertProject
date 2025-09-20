using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Services
{
    public interface IBookService
    {
        Task<ReturnModel<(PayPriceCalculation, List<PriceBreakdownItem>)>> CalculatePrice(long listingRentId, DateTime startDate, DateTime endDate, int guestId,
            Dictionary<string, string> clientData, bool useTechnicalMessages);

        Task<ReturnModel<TbBook>> RegisterPaymentAndCreateBooking(PaymentRequest paymentRequest, int userId,
            Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<List<TbBook>> GetBooksWithoutReviewByUser(int userId);

        Task<ReturnModel<TbBook>> BookingRequestApproval(
            Guid CalculationCode,
            int userId,
            Dictionary<string, string> clientData,
            bool useTechnicalMessages);
        Task<TbBook> Cancel(int userId, long bookId);
        Task<List<TbBook>> GetPendingAcceptance(int userId);
        Task<List<TbBook>> GetPendingAcceptanceForRenter(int userId);
        Task<List<TbBook>> GetCancelablesBookings(int userId);
        Task<List<TbBook>> GetApprovedsWOInit(int userId);
        Task<TbBook> AuthorizationResponse(int userId, long bookId, bool isApproval, int? reasonRefused);
        Task<PayPriceCalculation> ConsultingResponse(int userId, long priceCalculationId, bool isApproval, int? reasonRefused);
    }
}
