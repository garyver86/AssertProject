using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses
{
    public class PayPriceCalculationCompleteDTO
    {
        public PayPriceCalculationDTO PriceCalculation { get; set; }
        public List<PriceBreakdownItemDTO> PriceBreakdowns { get; set; }
    }
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
        public DateTime? InitBook { get; set; }
        public DateTime? EndBook { get; set; }
        public long? ListingRentId { get; set; }
        public string? BreakdownInfo { get; set; }
        public int CalculationStatusId { get; set; }
        public bool? ConsultAccepted { get; set; }
        public DateTime? ConsultResponse { get; set; }
        public int? ReasonRefusedId { get; set; }
        public int? UserId { get; set; }
        //public virtual TbBook? Book { get; set; }
        public PriceCalculationStatusDTO CalculationStatus { get; set; } = null!;
        public ReasonRefusedConsultDTO? ReasonRefused { get; set; }

        //public virtual UserDTO User { get; set; }
        //public virtual PayMethodOfPayment? MethodOfPayment { get; set; }
        //public virtual PayProvider? PaymentProvider { get; set; }
        //public virtual PayTransaction? PaymentTransaction { get; set; }
    }
    public class PriceBreakdownItemDTO
    {
        public string Type { get; set; } // "BASE_PRICE", "DISCOUNT", "CLEANING_FEE", etc.
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal? Percentage { get; set; } // Para descuentos
        public DateTime? Date { get; set; } // Para cobros por día específico
    }
    public class PriceCalculationStatusDTO
    {
        public int PayPriceCalculationStatus1 { get; set; }
        public string PriceCalculationStatusCode { get; set; } = null!;
        public string PriceCalculationStatusDescription { get; set; } = null!;
    }
}
