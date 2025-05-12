using Microsoft.AspNetCore.Http;

namespace Assert.Application.DTOs.Requests
{
    public class UploadImageRequest
    {
        
        public int? SpaceTypeId { get; set; }
        public string Description { get; set; }
        public bool? IsMain { get; set; }
        public string FileName { get; set; }
    }
}
