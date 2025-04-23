namespace Assert.Application.DTOs.Responses
{
    public class ListingSpecialDatePriceDTO
    {
        public DateTime? InitDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Currency { get; set; }
        public string CurrencyCode { get; set; }

        public decimal? PriceNightly { get; set; }

        public decimal? SecurityDepositPrice { get; set; }

        public string? SpecialDateName { get; set; }

        public string? SpecialDateDescription { get; set; }
    }
}