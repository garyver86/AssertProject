using Assert.Application.DTOs;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using AutoMapper;

namespace Assert.Application.Services

{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<ReturnModel, ReturnModelDTO>();

            CreateMap<ProcessDataRequest, ListingProcessDataModel>();
            CreateMap<AddressDTO, ProcessData_AddressModel>();
            CreateMap<ProcessData_Address, ProcessData_AddressModel>();
            CreateMap<ProcessData_Space, ProcessData_SpaceModel>();
            CreateMap<ProcessData_Photo, ProcessData_PhotoModel>();
            CreateMap<ProcessData_Discount, ProcessData_DiscountModel>();

            CreateMap<ListingProcessDataResultModel, ProcessDataResult>();




            CreateMap<ReturnModel<TlListingRent>, ReturnModelDTO<ListingRentDTO>>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

            CreateMap<ReturnModel<List<TlListingRent>>, ReturnModelDTO<List<ListingRentDTO>>>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

            CreateMap<ReturnModel<ListingProcessDataResultModel>, ReturnModelDTO<ProcessDataResult>>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

            CreateMap<Assert.Domain.Models.ListingProcessData_Parametrics, Assert.Application.DTOs.ListingProcessData_Parametrics>();
            CreateMap<Assert.Domain.Models.ListingProcessData_ListingData, Assert.Application.DTOs.ListingProcessData_ListingData>();



            CreateMap<ErrorCommon, ErrorCommonDTO>();
            CreateMap<TlListingRent, ListingRentDTO>()
                .ForMember(dest => dest.ApprovalPolicy, opt => opt.MapFrom(src => src.ApprovalPolicyType))
                .ForMember(dest => dest.ApprovalPolicy, opt => opt.MapFrom(src => src.ApprovalPolicyType))
                .ForMember(dest => dest.CancelationPolicy, opt => opt.MapFrom(src => src.CancelationPolicyType))
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.OwnerUser))
                .ForMember(dest => dest.Property, opt => opt.MapFrom(src => src.TpProperties.FirstOrDefault()))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.TlListingPrices.FirstOrDefault()))
                .ForMember(dest => dest.CheckInOutPolicies, opt => opt.MapFrom(src => src.TlCheckInOutPolicies))
                .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.TlListingAmenities))
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.TlListingPhotos))
                .ForMember(dest => dest.RentRules, opt => opt.MapFrom(src => src.TlListingRentRules))
                .ForMember(dest => dest.SecurityItems, opt => opt.MapFrom(src => src.TlListingSecurityItems))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.TlListingReviews))
                .ForMember(dest => dest.Spaces, opt => opt.MapFrom(src => src.TlListingSpaces))
                .ForMember(dest => dest.StayPresences, opt => opt.MapFrom(src => src.TlStayPresences));
            CreateMap<TlAccommodationType, AccomodationTypeDTO>();
            CreateMap<TApprovalPolicyType, ApprovalPolicyDTO>();
            CreateMap<TCancelationPolicyType, CancelationPolicyDTO>();
            CreateMap<TuUser, UserDTO>().
                ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.TuEmails.ToList().FirstOrDefault(email => email.IsPrincipal ?? true) != null ? src.TuEmails.ToList().First(email => email.IsPrincipal ?? true).Email : null))
                .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.TuDocuments))
                .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.TuPhones))
                .ForMember(dest => dest.ProfilePhotos, opt => opt.MapFrom(src => src.TuProfilePhotos));
            CreateMap<TpProperty, PropertyDTO>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.TpPropertyAddresses.FirstOrDefault()));
            CreateMap<TlListingPrice, PriceDTO>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency != null ? src.Currency.Name : null))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.Currency != null ? src.Currency.Code : null));
            CreateMap<TlCheckInOutPolicy, CheckInOutPolicyDTO>();
            CreateMap<TlListingAmenity, AmenityDTO>()
                .ForMember(dest => dest.TypeCode, opt => opt.MapFrom(src => src.AmenitiesType != null ? src.AmenitiesType.Code : null))
                .ForMember(dest => dest.IconLink, opt => opt.MapFrom(src => src.AmenitiesType != null ? src.AmenitiesType.IconLink : null))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.AmenitiesType != null ? src.AmenitiesType.Name : null));
            CreateMap< TpAmenitiesType, AmenityDTO >()
                .ForMember(dest => dest.TypeCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.IconLink, opt => opt.MapFrom(src => src.IconLink))
                .ForMember(dest => dest.AmenitiesTypeId, opt => opt.MapFrom(src => src.AmenitiesTypeId));
            CreateMap<TlListingFeaturedAspect, FeaturedAspectDTO>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.FeaturesAspectType != null ? src.FeaturesAspectType.FeaturedAspectCode : null))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FeaturesAspectType != null ? src.FeaturesAspectType.FeaturedAspectName : null))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.FeaturesAspectType != null ? src.FeaturedAspectValue : null));
            CreateMap< TFeaturedAspectType,FeaturedAspectDTO > ()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.FeaturedAspectCode))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FeaturedAspectName))
                .ForMember(dest => dest.FeaturedAspectTypeId, opt => opt.MapFrom(src => src.FeaturedAspectType));
            CreateMap<TlListingPhoto, PhotoDTO>();
            CreateMap<TlListingRentRule, RentRuleDTO>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.RuleType != null ? src.RuleType.Code : null))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RuleType != null ? src.RuleType.Name : null))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.RuleType != null ? src.RuleType.Description : null))
                .ForMember(dest => dest.IconLink, opt => opt.MapFrom(src => src.RuleType != null ? src.RuleType.IconLink : null));
            CreateMap<TlListingReview, ReviewDTO>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User != null ? $"{src.User.Name} {src.User.LastName}" : null));
            CreateMap<TlListingSecurityItem, SecurityItemDTO>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.SecurityItemType != null ? src.SecurityItemType.Code : null))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SecurityItemType != null ? src.SecurityItemType.Name : null))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SecurityItemType != null ? src.SecurityItemType.Description : null))
                .ForMember(dest => dest.IconLink, opt => opt.MapFrom(src => src.SecurityItemType != null ? src.SecurityItemType.IconLink : null));
            CreateMap<TlListingSpace, SpaceDTO>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.SpaceType != null ? src.SpaceType.Code : null))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SpaceType != null ? src.SpaceType.Name : null))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SpaceType != null ? src.SpaceType.Description : null))
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.SpaceType != null ? src.SpaceType.Icon : null));
            CreateMap<TlStayPresence, StayPresenceDTO>();
            CreateMap<TuDocument, DocumentDTO>();
            CreateMap<TuPhone, PhoneDTO>();
            CreateMap<TuProfilePhoto, ProfilePhotoDTO>();
            CreateMap<TpPropertyAddress, AddressDTO>();
            CreateMap<TpPropertySubtype, PropertyTypeDTO>()
                .ForMember(dest => dest.SubTypeCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.SubTypeIcon, opt => opt.MapFrom(src => src.Icon))
                .ForMember(dest => dest.SubType, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.PropertyType != null ? src.PropertyType.Name : null))
                .ForMember(dest => dest.TypeCode, opt => opt.MapFrom(src => src.PropertyType != null ? src.PropertyType.Code : null));
            CreateMap<TDiscountTypeForTypePrice, DiscountDTO>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.PorcentageSuggest, opt => opt.MapFrom(src => src.PorcentageSuggest))
                .ForMember(dest => dest.Porcentage, opt => opt.MapFrom(src => src.PorcentageSuggest))
                .ForMember(dest => dest.DiscountTypeForTypePriceId, opt => opt.MapFrom(src => src.DiscountTypeForTypePriceId));
            CreateMap<TlListingSpecialDatePrice, ListingSpecialDatePriceDTO>();
            CreateMap<TCity, CityDTO>()
                .ForMember(dest => dest.StateCode, opt => opt.MapFrom(src => src.County.State != null ? src.County.State.IataCode : null))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.County.State != null ? src.County.State.Name : null))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.County.State != null && src.County.State.Country != null ? src.County.State.Country.IataCode : null))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.County.State != null && src.County.State.Country != null ? src.Name : null))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.County.State != null && src.County.State.Country != null ? src.County.State.Country.Name : null));
        }
    }
}
