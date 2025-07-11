using Assert.Application.DTOs.Responses;
using Assert.Domain.Entities;
using AutoMapper;

namespace Assert.Application.Mappings;

public class CustomProfileMapper(IMapper _mapper)
{
    public AdditionalProfileDataDTO MapUserToAdditionalProfile(TuUser user)
    {
        var dto = _mapper.Map<AdditionalProfileDataDTO>(user);

        var additionalProfile = user.TuAdditionalProfiles.FirstOrDefault();

        dto.AdditionalProfileDataId = additionalProfile?.AdditionalProfileId ?? 0;
        dto.Languages = MapLanguages(additionalProfile?.TuAdditionalProfileLanguages);
        
        return dto;
    }

    private List<LanguageDTO> MapLanguages(ICollection<TuAdditionalProfileLanguage> profileLanguages)
    {
        if (profileLanguages == null) return new List<LanguageDTO>();

        return profileLanguages
            .Where(pl => pl.Language != null)
            .Select(pl => _mapper.Map<LanguageDTO>(pl.Language)) 
            .ToList();
    }
}
