using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses
{
    public class PayPriceCalculationDTO
    {
        public long PriceCalculationId { get; set; }

        public Guid? CalculationCode { get; set; }

        public long? BookId { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; } = null!;

        public string? CalculationDetails { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string CalculationStatue { get; set; } = null!;

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }

        public int? MethodOfPaymentId { get; set; }

        public int? PaymentProviderId { get; set; }

        public long? PaymentTransactionId { get; set; }
    }
}
