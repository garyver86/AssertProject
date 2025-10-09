using Assert.Domain.Entities;
using Assert.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Repositories
{
    public interface IPayPriceCalculationRepository
    {
        Task<ReturnModel<PayPriceCalculation>> Create(PayPriceCalculation payPriceCalculation);
        Task<PayPriceCalculation> GetByCode(Guid calculationCode);
        Task<PayPriceCalculation> GetById(long id);
        Task<ReturnModel> SetAsPayed(Guid calculationCode, int paymentProviderId, int methodOfPaymentId,
            long PaymentTransactionId);
        Task<PayPriceCalculation> ConsultingResponse(int userId, long priceCalculationId, bool isApproval, int? reasonRefused);
        Task SetBookId(long priceCalculationId, long bookId);
    }
}
