namespace Assert.Application.DTOs.Responses
{
    public class PhotoDTO
    {
        public string? PhotoLink { get; set; }

        public bool? IsPrincipal { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? Position { get; set; }
        public object SpaceType { get; set; }
        public object SpaceTypeCode { get; set; }
        public int? SpaceTypeId { get; set; }
        public long ListingPhotoId { get; set; }
    }
}