namespace Assert.Application.DTOs.Responses;

public class UserAccountClosedDTO
{
    public int UserAccountClosedId { get; set; }

    public int? UserId { get; set; }

    public int? UserSelectionOptionsId { get; set; }

    public DateTime? ClosingDate { get; set; }

    public DateTime? OpeningDate { get; set; }

    public string? Status { get; set; }

    public UserSelectionOptionDTO? UserSelectionOptions { get; set; }
}
