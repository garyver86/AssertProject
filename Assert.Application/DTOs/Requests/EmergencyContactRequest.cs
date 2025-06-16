namespace Assert.Application.DTOs.Requests;

public class EmergencyContactRequest
{
    public int EmergencyContactId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Relationship { get; set; } = string.Empty;

    public int LanguageId { get; set; }

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;
}
