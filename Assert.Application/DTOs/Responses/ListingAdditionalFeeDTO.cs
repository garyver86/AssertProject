namespace Assert.Application.DTOs.Responses;

public class ListingAdditionalFeeDTO
{
    public long ListingAdditionalFeeId { get; set; }

    public long ListingRentId { get; set; }

    public int AdditionalFeeId { get; set; }

    public decimal AmountFee { get; set; }
}
