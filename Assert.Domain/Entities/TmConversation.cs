using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TmConversation
{
    public long ConversationId { get; set; }

    public int UserIdOne { get; set; }

    public int UserIdTwo { get; set; }

    public int StatusId { get; set; }

    public DateTime? CreationDate { get; set; }

    public long? BookId { get; set; }

    public long? PriceCalculationId { get; set; }

    public long? ListingRentId { get; set; }

    public bool? UserOneFeatured { get; set; }

    public bool? UserOneArchived { get; set; }

    public bool? UserOneSilent { get; set; }

    public bool? UserTwoFeatured { get; set; }

    public bool? UserTwoArchived { get; set; }

    public bool? UserTwoSilent { get; set; }

    public DateTime? UserOneFeaturedDateTime { get; set; }

    public DateTime? UserOneArchivedDateTime { get; set; }

    public DateTime? UserOneSilentDateTime { get; set; }

    public DateTime? UserTwoFeaturedDateTime { get; set; }

    public DateTime? UserTwoArchivedDateTime { get; set; }

    public DateTime? UserTwoSilentDateTime { get; set; }

    public virtual TbBook? Book { get; set; }

    public virtual TlListingRent? ListingRent { get; set; }

    public virtual PayPriceCalculation? PriceCalculation { get; set; }

    public virtual TmConversationStatus Status { get; set; } = null!;

    public virtual ICollection<TmMessage> TmMessages { get; set; } = new List<TmMessage>();

    public virtual TuUser UserIdOneNavigation { get; set; } = null!;

    public virtual TuUser UserIdTwoNavigation { get; set; } = null!;
}
