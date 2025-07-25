using Assert.Domain.Entities;
using Assert.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Services
{
    public interface IBookService
    {
        Task<ReturnModel<PayPriceCalculation>> CalculatePrice(long listingRentId, DateTime startDate, DateTime endDate, int guestId,
            Dictionary<string, string> clientData, bool useTechnicalMessages);

        Task<ReturnModel<TbBook>> RegisterPaymentAndCreateBooking(PaymentRequest paymentRequest, int userId,
            Dictionary<string, string> clientData, bool useTechnicalMessages);
    }
}
