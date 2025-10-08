using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TbBookCancellation
{
    public long BookCacellationId { get; set; }

    public long? BookId { get; set; }

    public long? ListingRentId { get; set; }

    public int? UserId { get; set; }

    public string? CancellationTypeCode { get; set; }

    public int? CancellationReasonId { get; set; }

    public string? MessageToGuest { get; set; }

    public string? MessageToHost { get; set; }

    public string? MessageToAssert { get; set; }

    public string? CustomMessage { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Status { get; set; }

    public virtual TbBook? Book { get; set; }

    public virtual TbBookCancellationReason? CancellationReason { get; set; }

    public virtual TlListingRent? ListingRent { get; set; }

    public virtual TuUser? User { get; set; }
}
