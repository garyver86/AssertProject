namespace Assert.Domain.Entities;

public partial class TuEmail
{
    public int EmailId { get; set; }

    public int? UserId { get; set; }

    public string? Email { get; set; }

    public bool? IsPrincipal { get; set; }

    public bool? IsRecover { get; set; }

    public string? Description { get; set; }

    public virtual TuUser? User { get; set; }
}
