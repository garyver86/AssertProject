namespace Assert.Domain.Entities;

public partial class TbBookStepType
{
    public int BookStepTypeId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string DataView { get; set; } = null!;

    public string TableData { get; set; } = null!;

    public string? PreviousStepCode { get; set; }

    public virtual ICollection<TbBookStep> TbBookSteps { get; set; } = new List<TbBookStep>();
}
