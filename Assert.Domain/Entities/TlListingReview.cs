namespace Assert.Domain.Entities;

public partial class TlListingReview
{
    public long ListingReviewId { get; set; }

    public int UserId { get; set; }

    public DateTime? DateTimeReview { get; set; }

    public long ListingRentId { get; set; }

    public int Calification { get; set; }

    public string? Comment { get; set; }

    public long? BookId { get; set; }

    public virtual TbBook? Book { get; set; }

    public virtual TlListingRent ListingRent { get; set; } = null!;

    public virtual TuUser User { get; set; } = null!;
}
