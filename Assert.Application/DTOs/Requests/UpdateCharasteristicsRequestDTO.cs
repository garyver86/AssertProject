namespace Assert.Application.DTOs.Requests;

public class UpdateCharasteristicsRequestDTO
{
    //public int ListingRentId { get; set; }
    public List<int> FeaturedAmenities { get; set; } = new();
    public List<int> FeatureAspects { get; set; } = new();
    public List<int> SecurityItems { get; set; } = new();
}
