namespace Assert.Application.DTOs.Requests;

public class UpsertHostBookCancellationRequestDTO
{
    public int BookCancellationId { get; set; }
    public int BookId { get; set; }
    public int CancellationReasonId { get; set; }
    public string MessageTo { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
