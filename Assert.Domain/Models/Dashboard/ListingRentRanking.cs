namespace Assert.Domain.Models.Dashboard;

public class ListingRentRanking
{
    public long ListingRentId { get; set; }
    public string ListingRentTitle { get; set; } = string.Empty;
    public string ListingRentDescription { get; set; } = string.Empty;
    public string PhotoLink { get; set; } = string.Empty;
    public decimal TotalRent { get; set; }
    public decimal AdditionalFees { get; set; } = 0;
    public decimal Discounts { get; set; } = 0;
    public decimal PlatformFee { get; set; } = 0;
    public decimal Income { get; set; }
    public int ReservationCount { get; set; }
    public decimal OccupancyRate { get; set; } // % entre 0 y 100
    public int OccupiedDays { get; set; }
}
