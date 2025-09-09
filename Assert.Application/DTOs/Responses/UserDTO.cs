namespace Assert.Application.DTOs.Responses
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? FavoriteName { get; set; }
        public string? PhotoLink { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhoneCode { get; set; }
        public EmergencyContactDTO? EmergencyContact { get; set; } 
        public List<DocumentDTO>? Documents { get; set; }
        public List<PhoneDTO>? Phones { get; set; }
        public List<ProfilePhotoDTO>? ProfilePhotos { get; set; }
        public DateTime? RegisterDate { get; set; }
        public int? RegisterDateDays { get; set; }
        public int? RegisterDateMonths { get; set; }
        public int? RegisterDateYears { get; set; }
        public string? CityName { get; set; }
    }
}