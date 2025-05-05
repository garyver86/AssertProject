namespace Assert.Domain.Entities;

public partial class TAdditionalSuggestion
{
    public long AdditionalSuggestionId { get; set; }

    public string? Code { get; set; }

    public decimal? MilesPerDay { get; set; }

    public decimal? AdditionalMilePrice { get; set; }

    public decimal? GeneratorHourPerDay { get; set; }

    public decimal? GeneratorAdditionalHourPrice { get; set; }

    public int? MinimunRentalPerDay { get; set; }
}
