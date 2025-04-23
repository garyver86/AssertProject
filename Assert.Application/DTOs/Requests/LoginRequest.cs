using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Requests;

public class LoginRequest
{
    [Required]
    public string Platform { get; set; } = "none"; //apple - meta - google - local
    public string Token { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string UserName { get; set; } = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
