namespace Assert.Application.DTOs.Requests;

public class UpsertPreparationDayRequestDTO
{
    public long ListingRentId { get; set; }
    public int PreparationDayValue { get; set; }
}

public class PricingRequestDTO
{
    public long ListingRentId { get; set; }
    public decimal pricing { get; set; }
    public decimal weekendPricing { get; set; }
}
