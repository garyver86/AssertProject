using Assert.Application.DTOs;
using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Domain.Entities;
using Assert.Domain.Enums;
using Assert.Domain.Models;
using Assert.Domain.Models.Review;
using Assert.Domain.ValueObjects;
using Assert.Infrastructure.Utils;
using AutoMapper;

namespace Assert.Application.Mappings

{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            #region mapping enums
            CreateMap<string, Platform>().ConvertUsing<PlatformConverter>();
            #endregion

            CreateMap<DateOnly, DateTime>().ConvertUsing<DateOnlyToDateTimeConverter>();
            CreateMap<DateTime, DateOnly>().ConvertUsing<DateTimeToDateOnlyConverter>();

            CreateMap<ReturnModel, ReturnModelDTO>();

            CreateMap<ProcessDataRequest, ListingProcessDataModel>();
            CreateMap<AddressDTO, ProcessData_AddressModel>();
            CreateMap<ProcessData_Address, ProcessData_AddressModel>();
            CreateMap<ProcessData_Space, ProcessData_SpaceModel>();
            CreateMap<ProcessData_Photo, ProcessData_PhotoModel>();
            CreateMap<ProcessData_Discount, ProcessData_DiscountModel>();

            CreateMap<ListingProcessDataResultModel, ProcessDataResult>();

            CreateMap<string, ReturnModelDTO<string>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(_ => "200"))
                .ForMember(dest => dest.HasError, opt => opt.MapFrom(_ => false));

            CreateMap<CommonReview, CommonReviewDTO>();
            CreateMap<Domain.Models.Profile.Profile, ProfileDTO>();
            CreateMap<TuUser, AdditionalProfileDataDTO>()
            .ForMember(dest => dest.AdditionalProfileDataId,
                opt => opt.MapFrom(src => src.TuAdditionalProfiles.First().AdditionalProfileId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.WhatIDo,
                opt => opt.MapFrom(src => src.TuAdditionalProfiles.FirstOrDefault() != null
                    ? src.TuAdditionalProfiles.First().WhatIdo
                    : string.Empty))
            .ForMember(dest => dest.WantedToGo,
                opt => opt.MapFrom(src => src.TuAdditionalProfiles.FirstOrDefault() != null
                    ? src.TuAdditionalProfiles.First().WantedToGo
                    : string.Empty))
            .ForMember(dest => dest.Pets,
                opt => opt.MapFrom(src => src.TuAdditionalProfiles.FirstOrDefault() != null
                    ? src.TuAdditionalProfiles.First().Pets
                    : string.Empty))
            .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.DateOfBirth))
            .ForMember(dest => dest.IntroduceYourself,
                opt => opt.MapFrom(src => src.TuAdditionalProfiles.FirstOrDefault() != null
                    ? src.TuAdditionalProfiles.First().Additional
                    : string.Empty));

            CreateMap<ReturnModel<TlListingRent>, ReturnModelDTO<ListingRentDTO>>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

            CreateMap<ReturnModel<List<TlListingRent>>, ReturnModelDTO<List<ListingRentDTO>>>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

            CreateMap<ReturnModel<ListingProcessDataResultModel>, ReturnModelDTO<ProcessDataResult>>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

            CreateMap<Assert.Domain.Models.ListingProcessData_Parametrics, Assert.Application.DTOs.ListingProcessData_Parametrics>();
            CreateMap<Assert.Domain.Models.ListingProcessData_ListingData, Assert.Application.DTOs.ListingProcessData_ListingData>();
            CreateMap<PaginationMetadata, Assert.Application.DTOs.Responses.PaginationMetadataDTO>();



            CreateMap<ErrorCommon, ErrorCommonDTO>();
            CreateMap<TpSecurityItemType, SecurityItemDTO>();
            CreateMap<TDiscountTypeForTypePrice, DiscountDTO>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.PorcentageSuggest, opt => opt.MapFrom(src => src.PorcentageSuggest))
                .ForMember(dest => dest.Porcentage, opt => opt.MapFrom(src => src.PorcentageSuggest))
                .ForMember(dest => dest.DiscountTypeForTypePriceId, opt => opt.MapFrom(src => src.DiscountTypeForTypePriceId));


            CreateMap<TlListingDiscountForRate, DiscountDTO>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.DiscountTypeForTypePrice.Code))
                .ForMember(dest => dest.PorcentageSuggest, opt => opt.MapFrom(src => src.DiscountTypeForTypePrice.PorcentageSuggest))
                .ForMember(dest => dest.Days, opt => opt.MapFrom(src => src.DiscountTypeForTypePrice.Days))
                .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.DiscountTypeForTypePrice.Question))
                .ForMember(dest => dest.DiscountTypeForTypePriceId, opt => opt.MapFrom(src => src.DiscountTypeForTypePrice.DiscountTypeForTypePriceId));

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
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.TlListingPrices.FirstOrDefault() ?? new TlListingPrice()))
                .ForPath(dest => dest.Price.TlListingDiscountForRates, opt => opt.MapFrom(src => src.TlListingDiscountForRates))
                //.ForMember(dest => dest.Valoration, opt => opt.MapFrom(src => src.TlListingReviews.Select(y=>y.Calification).Average()));
                .ForMember(dest => dest.Valoration, opt => opt.MapFrom(src => UtilsMgr.CalculateAverageCalification(src.TlListingReviews)));


            CreateMap<TlListingFavorite, ListingFavoriteDTO>();
            CreateMap<TlListingFavoriteGroup, ListingFavoriteGroupDTO>();

            CreateMap<TlAccommodationType, AccomodationTypeDTO>();
            CreateMap<PayPriceCalculation, PayPriceCalculationDTO>();
            CreateMap<PayMethodOfPayment, PayMethodOfPaymentDTO>();
            CreateMap<TApprovalPolicyType, ApprovalPolicyDTO>();
            CreateMap<TCancelationPolicyType, CancelationPolicyDTO>();
            CreateMap<TpRuleType, RentRuleDTO>();
            CreateMap<TuEmergencyContact, EmergencyContactDTO>().ReverseMap();
            CreateMap<TuUser, UserDTO>().
                ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.TuEmails.ToList().FirstOrDefault(email => email.IsPrincipal ?? true) != null ? src.TuEmails.ToList().First(email => email.IsPrincipal ?? true).Email : null))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.TuPhones.ToList().FirstOrDefault(phone => phone.IsPrimary ?? true) != null ? src.TuPhones.ToList().First(phone => phone.IsPrimary ?? true).Number : null))
                .ForMember(dest => dest.PhoneCode, opt => opt.MapFrom(src => src.TuPhones.ToList().FirstOrDefault(phone => phone.IsPrimary ?? true) != null ? src.TuPhones.ToList().First(phone => phone.IsPrimary ?? true).CountryCode : null))
                .ForMember(dest => dest.EmergencyContact, opt => opt.MapFrom(src => src.TuEmergencyContacts.FirstOrDefault()))
                .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.TuDocuments))
                .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.TuPhones))
                .ForMember(dest => dest.ProfilePhotos, opt => opt.MapFrom(src => src.TuProfilePhotos));
            CreateMap<TpProperty, PropertyDTO>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.TpPropertyAddresses.FirstOrDefault()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.PropertySubtype));
            CreateMap<TlListingPrice, PriceDTO>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency != null ? src.Currency.Name : null))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.Currency != null ? src.Currency.Code : null));
            CreateMap<TlCheckInOutPolicy, CheckInOutPolicyDTO>();
            CreateMap<TlListingAmenity, AmenityDTO>()
                .ForMember(dest => dest.TypeCode, opt => opt.MapFrom(src => src.AmenitiesType != null ? src.AmenitiesType.Code : null))
                .ForMember(dest => dest.IconLink, opt => opt.MapFrom(src => src.AmenitiesType != null ? src.AmenitiesType.IconLink : null))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.AmenitiesType != null ? src.AmenitiesType.Name : null));
            CreateMap<TpAmenitiesType, AmenityDTO>()
                .ForMember(dest => dest.TypeCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.IconLink, opt => opt.MapFrom(src => src.IconLink))
                .ForMember(dest => dest.AmenitiesTypeId, opt => opt.MapFrom(src => src.AmenitiesTypeId));
            CreateMap<TlListingFeaturedAspect, FeaturedAspectDTO>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.FeaturesAspectType != null ? src.FeaturesAspectType.FeaturedAspectCode : null))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FeaturesAspectType != null ? src.FeaturesAspectType.FeaturedAspectName : null))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.FeaturesAspectType != null ? src.FeaturedAspectValue : null));
            CreateMap<TFeaturedAspectType, FeaturedAspectDTO>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.FeaturedAspectCode))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FeaturedAspectName))
                .ForMember(dest => dest.FeaturedAspectTypeId, opt => opt.MapFrom(src => src.FeaturedAspectType));
            CreateMap<TlListingPhoto, PhotoDTO>()
                .ForMember(dest => dest.SpaceType, opt => opt.MapFrom(src => src.SpaceType != null ? src.SpaceType.Name : null))
                .ForMember(dest => dest.SpaceTypeCode, opt => opt.MapFrom(src => src.SpaceType != null ? src.SpaceType.Code : null));
            CreateMap<TlListingRentRule, RentRuleDTO>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.RuleType != null ? src.RuleType.Code : null))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RuleType != null ? src.RuleType.Name : null))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.RuleType != null ? src.RuleType.Description : null))
                .ForMember(dest => dest.IconLink, opt => opt.MapFrom(src => src.RuleType != null ? src.RuleType.IconLink : null));
            CreateMap<TlListingReview, ReviewDTO>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User != null ? $"{src.User.Name} {src.User.LastName}" : null))
                .ForMember(dest => dest.UserProfilePhoto, opt => opt.MapFrom(src => src.User != null ? src.User.PhotoLink : null));
            CreateMap<TlListingSecurityItem, SecurityItemDTO>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.SecurityItemType != null ? src.SecurityItemType.Code : null))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SecurityItemType != null ? src.SecurityItemType.Name : null))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SecurityItemType != null ? src.SecurityItemType.Description : null))
                .ForMember(dest => dest.SecurityItemTypeId, opt => opt.MapFrom(src => src.SecurityItemTypeId))
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
            CreateMap<TpPropertyAddress, AddressDTO>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.cityId, opt => opt.MapFrom(src => src.CityId))
                .ForMember(dest => dest.CountyId, opt => opt.MapFrom(src => src.City != null ? src.City.CountyId : 0))
                .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => src.City != null && src.City.County != null ? src.City.County.StateId : 0));

            CreateMap<TmMessage, MessageDTO>();
            CreateMap<TmConversation, ConversationDTO>();
            CreateMap<TpPropertySubtype, PropertyTypeDTO>()
                .ForMember(dest => dest.SubTypeCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.SubTypeIcon, opt => opt.MapFrom(src => src.Icon))
                .ForMember(dest => dest.SubType, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.PropertyType != null ? src.PropertyType.Name : null))
                .ForMember(dest => dest.TypeCode, opt => opt.MapFrom(src => src.PropertyType != null ? src.PropertyType.Code : null));

            CreateMap<TlListingSpecialDatePrice, ListingSpecialDatePriceDTO>();
            CreateMap<TCity, CityDTO>()
                .ForMember(dest => dest.StateCode, opt => opt.MapFrom(src => src.County.State != null ? src.County.State.IataCode : null))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.County.State != null ? src.County.State.Name : null))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.County.State != null && src.County.State.Country != null ? src.County.State.Country.IataCode : null))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.County.State != null && src.County.State.Country != null ? src.County.State.Country.Name : null))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.County != null ? src.County.Name : null));
            CreateMap<UploadImageRequest, UploadImageListingRent>();

            CreateMap<TSpaceType, SpaceTypeDTO>();

            CreateMap<TLanguage, LanguageDTO>().ReverseMap();

            CreateMap<ListingReviewSummary, ListingReviewSummaryDTO>();

            CreateMap<TlListingCalendar, CalendarDayDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.BlockType, opt => opt.MapFrom(src => src.BlockType))
                .ForMember(dest => dest.BlockReason, opt => opt.MapFrom(src => src.BlockReason))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
                .ForMember(dest => dest.MinimumStay, opt => opt.MapFrom(src => src.MinimumStay))
                .ForMember(dest => dest.MaximumStay, opt => opt.MapFrom(src => src.MaximumStay))
                .ForMember(dest => dest.BlockTypeName, opt => opt.MapFrom(src => src.BlockTypeNavigation.BlockTypeName));

            CreateMap<(List<TlListingCalendar> CalendarDays, PaginationMetadata Pagination),
                     (IEnumerable<CalendarDayDto> CalendarDays, PaginationMetadata Pagination)>()
                .ForMember(dest => dest.CalendarDays, opt => opt.MapFrom(src => src.CalendarDays))
                .ForMember(dest => dest.Pagination, opt => opt.MapFrom(src => src.Pagination));

            CreateMap<(List<TlListingCalendar>, PaginationMetadata), CalendarResultPaginatedDTO>()
            .ForMember(dest => dest.CalendarDays, opt => opt.MapFrom(src => src.Item1))
            .ForMember(dest => dest.Pagination, opt => opt.MapFrom(src => src.Item2));
        }

        // Conversor personalizado de DateOnly a DateTime
        public class DateOnlyToDateTimeConverter : ITypeConverter<DateOnly, DateTime>
        {
            public DateTime Convert(DateOnly source, DateTime destination, ResolutionContext context)
            {
                return source.ToDateTime(TimeOnly.MinValue);
            }
        }

        // Conversor personalizado de DateTime a DateOnly
        public class DateTimeToDateOnlyConverter : ITypeConverter<DateTime, DateOnly>
        {
            public DateOnly Convert(DateTime source, DateOnly destination, ResolutionContext context)
            {
                return DateOnly.FromDateTime(source);
            }
        }
    }
}
