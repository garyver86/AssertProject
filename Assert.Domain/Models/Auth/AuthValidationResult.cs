using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Models.Auth;

public class AuthValidationResult
{
    public bool IsValid { get; set; }
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public string? ErrorMessage { get; set; }
}
