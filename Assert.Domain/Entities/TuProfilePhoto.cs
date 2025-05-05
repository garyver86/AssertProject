namespace Assert.Domain.Entities;

public partial class TuProfilePhoto
{
    public int ProfilePhotoId { get; set; }

    public int UserId { get; set; }

    public string FileName { get; set; } = null!;

    public DateTime UpdateDate { get; set; }

    public bool IsPrincipal { get; set; }

    public int StatusId { get; set; }

    public virtual TuUser User { get; set; } = null!;
}
