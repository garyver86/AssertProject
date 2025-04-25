namespace Assert.Domain.Entities;

public partial class TbBookInsuranceClaim
{
    public int ClaimId { get; set; }

    public long BookingId { get; set; }

    public int UserId { get; set; }

    public string ClaimDescription { get; set; } = null!;

    public DateTime? ClaimDate { get; set; }

    public int? ClaimStatusId { get; set; }

    public decimal? Amount { get; set; }

    public virtual TbBook Booking { get; set; } = null!;

    public virtual TuUser User { get; set; } = null!;
}
