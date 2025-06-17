using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses;

public class EmergencyContactDTO
{
    public int EmergencyContactId { get; set; }

    public int? UserId { get; set; }

    public string Name { get; set; } = null!;

    public string LstName { get; set; } = null!;

    public string Relationship { get; set; } = null!;

    public int? LanguageId { get; set; }

    public string? Email { get; set; }

    public string? PhoneCode { get; set; }

    public string? PhoneNumber { get; set; }
}
