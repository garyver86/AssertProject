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

    public virtual TmConversationStatus Status { get; set; } = null!;

    public virtual ICollection<TmMessage> TmMessages { get; set; } = new List<TmMessage>();
}
