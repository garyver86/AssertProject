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
        dto.LiveAt = MapLiveAt(additionalProfile?.TuAdditionalProfileLiveAts.FirstOrDefault());
        return dto;
    }

    private LiveAtDTO MapLiveAt(TuAdditionalProfileLiveAt? liveAt)
    {
        if(liveAt is null) 
            return new LiveAtDTO();

        return new LiveAtDTO
        {
            CityId = liveAt.StateId!.Value,
            CityName = liveAt.CityName ?? "",
            Location = liveAt.Location ?? "",
            LiveAtShow = liveAt.State?.Name is not null ?
                $"{liveAt.State.Name} - {liveAt.State?.Country?.Name ?? ""}" : "---",
        };
    }

    private List<LanguageDTO> MapLanguages(ICollection<TuAdditionalProfileLanguage>? profileLanguages)
    {
        if (profileLanguages is null) return new List<LanguageDTO>();

        return profileLanguages
            .Where(pl => pl.Language != null)
            .Select(pl => _mapper.Map<LanguageDTO>(pl.Language)) 
            .ToList();
    }
}
