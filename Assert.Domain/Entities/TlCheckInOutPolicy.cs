namespace Assert.Domain.Entities;

public partial class TlCheckInOutPolicy
{
    public int PolicyId { get; set; }

    public long ListingRentid { get; set; }

    public TimeOnly? CheckInTime { get; set; }

    public TimeOnly? CheckOutTime { get; set; }

    public bool? FlexibleCheckIn { get; set; }

    public bool? FlexibleCheckOut { get; set; }

    public decimal? LateCheckInFee { get; set; }

    public decimal? LateCheckOutFee { get; set; }

    public string? Instructions { get; set; }

    public virtual TlListingRent ListingRent { get; set; } = null!;
}
