using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TmConversationStatus
{
    public int ConversationStatusId { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<TmConversation> TmConversations { get; set; } = new List<TmConversation>();
}
