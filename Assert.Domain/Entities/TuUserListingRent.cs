namespace Assert.Domain.Entities;

public partial class TuUserListingRent
{
    public int UserListingRentId { get; set; }

    public int? UserId { get; set; }

    public long? ListingRentId { get; set; }

    public string? Code { get; set; }

    public DateTime? DateTimeTransaction { get; set; }

    public int? UserListingRentStatusId { get; set; }

    public int? UserListingStatusId { get; set; }

    public virtual TlListingRent? ListingRent { get; set; }

    public virtual ICollection<TuUserListingRentPayment> TuUserListingRentPayments { get; set; } = new List<TuUserListingRentPayment>();

    public virtual TuUser? User { get; set; }

    public virtual TuUserListingStatus? UserListingStatus { get; set; }
}
