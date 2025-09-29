namespace Assert.Application.DTOs.Requests;

public class UpsertAdditionalFeeRequestDTO
{
    public int ListingRentId { get; set; }
    public List<int> AdditionalFeeIds { get; set; } = new();
    public List<decimal> AdditionalFeeValues { get; set; } = new();
}
