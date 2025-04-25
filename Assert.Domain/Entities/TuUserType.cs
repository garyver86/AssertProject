namespace Assert.Domain.Entities;

public partial class TuUserType
{
    public int UserTypeId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TuUserRole> TuUserRoles { get; set; } = new List<TuUserRole>();
}
