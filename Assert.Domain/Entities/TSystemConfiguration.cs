namespace Assert.Domain.Entities;

public partial class TSystemConfiguration
{
    public int SystemConfigurationId { get; set; }

    public string Name { get; set; } = null!;

    public string Value { get; set; } = null!;
}
