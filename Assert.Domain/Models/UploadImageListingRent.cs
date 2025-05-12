using Microsoft.AspNetCore.Http;

namespace Assert.Domain.Models
{
    public class UploadImageListingRent
    {
        public int? SpaceTypeId { get; set; }
        public string Description { get; set; }
        public bool IsMain { get; set; }
        public string FileName { get; set; }
    }
}
