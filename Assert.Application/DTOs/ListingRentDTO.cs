namespace Assert.Application.DTOs
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
        public AccomodationTypeDTO? AccomodationType { get; set; }
        public ApprovalPolicyDTO? ApprovalPolicy { get; set; }
        public CancelationPolicyDTO? CancelationPolicy { get; set; }
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


    }
}
