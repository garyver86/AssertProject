namespace Assert.Domain.Entities;

public partial class TErrorParam
{
    public int ErrorParamId { get; set; }

    public string Code { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Message { get; set; }

    public string? TechnicalMessage { get; set; }

    public string? Title { get; set; }
}
