using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuEmergencyContact
{
    public int EmergencyContactId { get; set; }

    public string Name { get; set; } = null!;

    public string LstName { get; set; } = null!;

    public string Relationship { get; set; } = null!;

    public int? LanguageId { get; set; }

    public string? Email { get; set; }

    public string? PhoneCode { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual TLanguage? Language { get; set; }
}
