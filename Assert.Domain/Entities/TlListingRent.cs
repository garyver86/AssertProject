using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlListingRent
{
    public long ListingRentId { get; set; }

    public int OwnerUserId { get; set; }

    public int? ListingStatusId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? StepsCount { get; set; }

    public decimal? MinimunRentalPerDay { get; set; }

    public int? CancelationPolicyTypeId { get; set; }

    public int? ApprovalPolicyTypeId { get; set; }

    public int? ApprovalRequestDays { get; set; }

    public int? AccomodationTypeId { get; set; }

    public int? Bedrooms { get; set; }

    public int? Bathrooms { get; set; }

    public int? MaxGuests { get; set; }

    public bool? AllDoorsLocked { get; set; }

    public int? Beds { get; set; }

    public DateTime? ListingRentConfirmationDate { get; set; }

    public bool? ExternalCameras { get; set; }

    public bool? PresenceOfWeapons { get; set; }

    public bool? NoiseDesibelesMonitor { get; set; }

    public int? MinimunStay { get; set; }

    public int? MaximumStay { get; set; }

    public int? MinimumNotice { get; set; }

    public TimeOnly? MinimumNoticeHour { get; set; }

    public int? PreparationDays { get; set; }

    public int? AvailabilityWindowMonth { get; set; }

    public string? CheckInDays { get; set; }

    public string? CheckOutDays { get; set; }

    public int? PrivateBathroom { get; set; }

    public int? PrivateBathroomLodging { get; set; }

    public int? SharedBathroom { get; set; }

    public virtual TlAccommodationType? AccomodationType { get; set; }

    public virtual TApprovalPolicyType? ApprovalPolicyType { get; set; }

    public virtual TCancelationPolicyType? CancelationPolicyType { get; set; }

    public virtual TlListingStatus? ListingStatus { get; set; }

    public virtual TuUser OwnerUser { get; set; } = null!;

    public virtual ICollection<TbBook> TbBooks { get; set; } = new List<TbBook>();

    public virtual ICollection<TlCheckInOutPolicy> TlCheckInOutPolicies { get; set; } = new List<TlCheckInOutPolicy>();

    public virtual ICollection<TlExternalReference> TlExternalReferences { get; set; } = new List<TlExternalReference>();

    public virtual ICollection<TlGenerateRate> TlGenerateRates { get; set; } = new List<TlGenerateRate>();

    public virtual ICollection<TlListingAdditionalFee> TlListingAdditionalFees { get; set; } = new List<TlListingAdditionalFee>();

    public virtual ICollection<TlListingAmenity> TlListingAmenities { get; set; } = new List<TlListingAmenity>();

    public virtual ICollection<TlListingAvailability> TlListingAvailabilities { get; set; } = new List<TlListingAvailability>();

    public virtual ICollection<TlListingCalendar> TlListingCalendars { get; set; } = new List<TlListingCalendar>();

    public virtual ICollection<TlListingDiscountForRate> TlListingDiscountForRates { get; set; } = new List<TlListingDiscountForRate>();

    public virtual ICollection<TlListingFavorite> TlListingFavorites { get; set; } = new List<TlListingFavorite>();

    public virtual ICollection<TlListingFeaturedAspect> TlListingFeaturedAspects { get; set; } = new List<TlListingFeaturedAspect>();

    public virtual ICollection<TlListingPhoto> TlListingPhotos { get; set; } = new List<TlListingPhoto>();

    public virtual ICollection<TlListingPrice> TlListingPrices { get; set; } = new List<TlListingPrice>();

    public virtual ICollection<TlListingRentChange> TlListingRentChanges { get; set; } = new List<TlListingRentChange>();

    public virtual ICollection<TlListingRentRule> TlListingRentRules { get; set; } = new List<TlListingRentRule>();

    public virtual ICollection<TlListingReview> TlListingReviews { get; set; } = new List<TlListingReview>();

    public virtual ICollection<TlListingSecurityItem> TlListingSecurityItems { get; set; } = new List<TlListingSecurityItem>();

    public virtual ICollection<TlListingSpace> TlListingSpaces { get; set; } = new List<TlListingSpace>();

    public virtual ICollection<TlListingSpecialDatePrice> TlListingSpecialDatePrices { get; set; } = new List<TlListingSpecialDatePrice>();

    public virtual ICollection<TlListingStep> TlListingSteps { get; set; } = new List<TlListingStep>();

    public virtual ICollection<TlListingViewHistory> TlListingViewHistories { get; set; } = new List<TlListingViewHistory>();

    public virtual ICollection<TlStayPresence> TlStayPresences { get; set; } = new List<TlStayPresence>();

    public virtual ICollection<TpProperty> TpProperties { get; set; } = new List<TpProperty>();

    public virtual ICollection<TuUserListingRent> TuUserListingRents { get; set; } = new List<TuUserListingRent>();

    public virtual ICollection<TuUserReview> TuUserReviews { get; set; } = new List<TuUserReview>();
}
