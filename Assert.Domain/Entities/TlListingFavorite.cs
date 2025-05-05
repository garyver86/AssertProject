namespace Assert.Domain.Entities;

public partial class TlListingFavorite
{
    public long FavoriteListingId { get; set; }

    public long ListingRentId { get; set; }

    public int UserId { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual TlListingRent ListingRent { get; set; } = null!;

    public virtual TuUser User { get; set; } = null!;
}
