namespace Assert.Application.DTOs.Responses
{
    public class PriceDTO
    {
        public decimal? PriceNightly { get; set; }
        public decimal? SecurityDepositPrice { get; set; }
        public string? Currency { get; set; }
        public string? CurrencyCode { get; set; }
        public long ListingPriceId { get; set; }
        public long ListingRentId { get; set; }
        public int? ListingPriceOfferId { get; set; }
        public int? TimeUnitId { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? WeekendNightlyPrice { get; set; }
        public List<DiscountDTO> TlListingDiscountForRates { get; set; }
        public List<ListingSpecialDatePriceDTO> SpecialDatePrices { get; set; }
    }
}