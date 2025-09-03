namespace Assert.Application.DTOs.Responses
{
    public class ListingRentDTO
    {
        public long ListingRentId { get; set; }
        public string Status { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? MinimunRentalPerDay { get; set; }
        public int? ApprovalRequestDays { get; set; }
        public string? PhoneNumber { get; set; }
        public int OwnerUserId { get; set; }
        public int? ListingStatusId { get; set; }
        public int? StepsCount { get; set; }
        public int? CancelationPolicyTypeId { get; set; }
        public int? ApprovalPolicyTypeId { get; set; }
        public int? AccomodationTypeId { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? MaxGuests { get; set; }
        public bool? AllDoorsLocked { get; set; }
        public int? Beds { get; set; }
        public bool? ExternalCameras { get; set; }
        public bool? PresenceOfWeapons { get; set; }
        public bool? NoiseDesibelesMonitor { get; set; }
        public DateTime? ListingRentConfirmationDate { get; set; }
        public AccomodationTypeDTO? AccomodationType { get; set; }
        public ApprovalPolicyDTO? ApprovalPolicy { get; set; }
        public CancellationPolicyDTO? CancelationPolicy { get; set; }
        public UserDTO Owner { get; set; }
        public PropertyDTO Property { get; set; }
        public PriceDTO Price { get; set; }
        public List<CheckInOutPolicyDTO> CheckInOutPolicies { get; set; }
        public List<AmenityDTO> Amenities { get; set; }
        public List<FeaturedAspectDTO> FeaturedAspects { get; set; }
        public List<PhotoDTO> Photos { get; set; }
        public List<RentRuleDTO> RentRules { get; set; }
        public List<ReviewDTO> Reviews { get; set; }
        public List<SecurityItemDTO> SecurityItems { get; set; }
        public List<SpaceDTO> Spaces { get; set; }
        public List<StayPresenceDTO> StayPresences { get; set; }
        //public List<BookDTO> Books { get; set; }
        public decimal? Valoration { get; set; }
        public DateTime? TotalRents { get; set; }
        public int MyProperty { get; set; }
        public bool? isFavorite { get; set; }
        public int? PrivateBathroom { get; set; }
        public int? PrivateBathroomLodging { get; set; }
        public int? SharedBathroom { get; set; }
        public decimal? AvgReviews { get; set; }
        public int? MinimunStay { get; set; }
        public int? MaximumStay { get; set; }
        public int? MinimumNotice { get; set; }
        public TimeOnly? MinimumNoticeHour { get; set; }
        public int? PreparationDays { get; set; }
        public int? AvailabilityWindowMonth { get; set; }
        public string? CheckInDays { get; set; }
        public string? CheckOutDays { get; set; }
    }
    public class ListingRentResumeDTO
    {
        public long ListingRentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public PropertyDTO Property { get; set; }
        public PriceDTO Price { get; set; }
    }
}
