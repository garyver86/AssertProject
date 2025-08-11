namespace Assert.Application.DTOs.Responses;

public class ProfileDTO
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FavoriteName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public int GuestHostingsTotal { get; set; }
    public int HostHostingsTotal { get; set; }
    public double GuestReviewCalification { get; set; }
    public double HostReviewCalification { get; set; }
    public int YearsInAssert { get; set; }
    public int ListingRentCount { get; set; }
    public string TimeInAssert { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public int CountReviewsGuest { get; set; }
    public int CountReviewsHost{ get; set; }
    public List<CommonReviewDTO> GuestReviews { get; set; }
    public List<CommonReviewDTO> HostReviews { get; set; }
}
