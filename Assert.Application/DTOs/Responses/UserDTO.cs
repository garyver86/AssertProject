﻿namespace Assert.Application.DTOs.Responses
{
    public class UserDTO
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? PhotoLink { get; set; }
        public string? Gender { get; set; }
        public string Email { get; set; }
        public List<DocumentDTO> Documents { get; set; }
        public List<PhoneDTO> Phones { get; set; }
        public List<ProfilePhotoDTO> ProfilePhotos { get; set; }
    }
}