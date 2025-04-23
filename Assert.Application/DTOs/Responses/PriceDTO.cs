namespace Assert.Application.DTOs.Responses
{
    public class PriceDTO
    {
        public decimal? PriceNightly { get; set; }
        public decimal? SecurityDepositPrice { get; set; }
        public string? Currency { get; set; }
        public string? CurrencyCode { get; set; }
        public List<DiscountDTO> TlListingDiscountForRates { get; set; }
        public List<ListingSpecialDatePriceDTO> SpecialDatePrices { get; set; }
    }
}