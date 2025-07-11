using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlListingFavoriteGroup
{
    public long FavoriteGroupListingId { get; set; }

    public int UserId { get; set; }

    public string FavoriteGroupName { get; set; } = null!;

    public DateTime? CreationDate { get; set; }

    public short GroupStatus { get; set; }

    public virtual ICollection<TlListingFavorite> TlListingFavorites { get; set; } = new List<TlListingFavorite>();

    public virtual TuUser User { get; set; } = null!;
}
