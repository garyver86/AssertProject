using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TmPredefinedMessage
{
    public int PredefinedMessageId { get; set; }

    public string? Title { get; set; }

    public string Code { get; set; } = null!;

    public string MessageBody { get; set; } = null!;

    public string? MessageActions { get; set; }
}
