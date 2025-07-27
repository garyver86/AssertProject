using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Models
{
    public class PaymentRequest
    {
        public Guid CalculationCode { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string OrderCode { get; set; }
        public string Stan { get; set; }
        public int MethodOfPaymentId { get; set; }
        public int PaymentProviderId { get; set; }
        public int CountryId { get; set; }
        public string PaymentData { get; set; }
        public string TransactionData { get; set; }
    }
}
