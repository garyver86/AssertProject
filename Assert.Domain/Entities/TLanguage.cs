namespace Assert.Domain.Entities;

public partial class TLanguage
{
    public int LanguageId { get; set; }

    public string? Code { get; set; }

    public string? Detail { get; set; }

    public virtual ICollection<TuEmergencyContact> TuEmergencyContacts { get; set; } = new List<TuEmergencyContact>();
}
