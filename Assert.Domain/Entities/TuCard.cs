namespace Assert.Domain.Entities;

public partial class TuCard
{
    public int CardId { get; set; }

    public int UserId { get; set; }

    public string CardNumber { get; set; } = null!;

    public string? CardToken { get; set; }

    public string? ProcesorCode { get; set; }

    public string? ProcesorName { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int StatusId { get; set; }

    public bool IsPrincipal { get; set; }

    public virtual TuUser User { get; set; } = null!;
}
