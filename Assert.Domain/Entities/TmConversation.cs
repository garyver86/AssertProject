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

    public virtual TbBook? Book { get; set; }

    public virtual TlListingRent? ListingRent { get; set; }

    public virtual PayPriceCalculation? PriceCalculation { get; set; }

    public virtual TmConversationStatus Status { get; set; } = null!;

    public virtual ICollection<TmMessage> TmMessages { get; set; } = new List<TmMessage>();

    public virtual TuUser UserIdOneNavigation { get; set; } = null!;

    public virtual TuUser UserIdTwoNavigation { get; set; } = null!;
}
