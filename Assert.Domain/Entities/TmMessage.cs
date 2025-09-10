using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TmMessage
{
    public long MessageId { get; set; }

    public int? UserId { get; set; }

    public string? Body { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ReadDate { get; set; }

    public int MessageTypeId { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool IsRead { get; set; }

    public long? ParentMessage { get; set; }

    public DateTime? DateExecution { get; set; }

    public long ConversationId { get; set; }

    public string? IpAddress { get; set; }

    public string? AdditionalData { get; set; }

    public int MessageStatusId { get; set; }

    public virtual TmConversation Conversation { get; set; } = null!;

    public virtual TmMessageStatus MessageStatus { get; set; } = null!;

    public virtual TmTypeMessage MessageType { get; set; } = null!;
}
