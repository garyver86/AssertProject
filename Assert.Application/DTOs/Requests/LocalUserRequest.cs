using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Requests;

public class LocalUserRequest
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;

    public string OtpCode { get; set; } = string.Empty;

}
