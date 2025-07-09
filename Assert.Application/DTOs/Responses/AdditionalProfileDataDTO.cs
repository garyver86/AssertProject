using Assert.Domain.Models;

namespace Assert.Application.DTOs.Responses;

public class AdditionalProfileDataDTO
{
    public int AdditionalProfileDataId { get; set; }
    public int UserId { get; set; }
    public string WhatIDo { get; set; } = string.Empty;
    public string WantedToGo { get; set; } = string.Empty;
    public string Pets { get; set; } = string.Empty;
    public DateTime? Birthday { get; set; }
    public List<LanguageDTO> Languages { get; set; } = new();
    public LiveAtDTO LiveAt { get; set; } = new();
    public string IntroduceYourself { get; set; } = string.Empty;
}
