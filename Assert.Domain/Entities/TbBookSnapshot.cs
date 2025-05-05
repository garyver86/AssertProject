namespace Assert.Domain.Entities;

public partial class TbBookSnapshot
{
    public long BookSnapshotId { get; set; }

    public long ListingRentId { get; set; }

    public long ListingId { get; set; }

    public string ListingName { get; set; } = null!;

    public int OwnerId { get; set; }

    public int RenterId { get; set; }

    public DateTime? BookDate { get; set; }

    public long PropertyId { get; set; }

    public decimal Amount { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public decimal AmountDiscount { get; set; }

    public decimal AmountNightly { get; set; }

    public decimal AmountNightlyDiscount { get; set; }

    public string DiscountDetails { get; set; } = null!;

    public decimal? DepositSegure { get; set; }

    public int Miles { get; set; }

    public int ManufacturerId { get; set; }

    public string Manufacturer { get; set; } = null!;

    public int ModelId { get; set; }

    public string Model { get; set; } = null!;

    public int Year { get; set; }

    public string GeneratorDescription { get; set; } = null!;

    public string TypeApprovalDescription { get; set; } = null!;

    public string MilesPolicyDescription { get; set; } = null!;

    public string? Policies { get; set; }

    public int DaysOfRent { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int CityId { get; set; }

    public string FormOfPayment { get; set; } = null!;

    public string? FopDetails { get; set; }

    public string IpAddress { get; set; } = null!;

    public long BookId { get; set; }

    public int? TotalDays { get; set; }

    public decimal? DiscountRate { get; set; }

    public decimal? SubtotalAmount { get; set; }

    public decimal? SubtotalAmountDiscount { get; set; }

    public decimal? TotalAmountDiscount { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual TbBook Book { get; set; } = null!;
}
