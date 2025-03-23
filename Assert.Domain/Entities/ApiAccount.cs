namespace Assert.Domain.Entities;

public partial class ApiAccount
{
    public int ApiAccounId { get; set; }

    public string ApiKey { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsEnabled { get; set; }

    public DateTime? TemporaryBlockTo { get; set; }

    public bool? IsBlocked { get; set; }

    public int? WrongAccess { get; set; }
}
