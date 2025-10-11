namespace Assert.Application.DTOs.Responses;

public class BookCancellationReasonDTO
{
    public int CancellationReasonId { get; set; }

    public int? CancellationReasonParentId { get; set; }

    public int? CancellationGroupId { get; set; }

    public string? CancellationTypeCode { get; set; }

    public int? CancellationLevel { get; set; }

    public bool? IsEndStep { get; set; }

    public string? TitleGroup { get; set; }

    public string? Detail { get; set; }

    public string? MessageTo { get; set; }

    public string? Icon { get; set; }
}
