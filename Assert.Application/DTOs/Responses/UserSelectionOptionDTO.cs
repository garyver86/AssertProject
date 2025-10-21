namespace Assert.Application.DTOs.Responses;

public class UserSelectionOptionDTO
{
    public int UserSelectionOptionsId { get; set; }

    public int? UserGroupTypeId { get; set; }

    public string? ToUse { get; set; }

    public string? OptionDetail { get; set; }

    public string? Status { get; set; }

    public UserGroupTypeDTO? UserGroupType { get; set; }
}
