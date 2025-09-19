namespace Assert.Application.DTOs.Requests;

public class ForgotPasswordRequest
{
    public string UserName { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string OtpCode { get; set; } = string.Empty;
}
