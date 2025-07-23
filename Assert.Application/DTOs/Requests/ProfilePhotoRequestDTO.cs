using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Requests;

public class ProfilePhotoRequestDTO
{
    [Required]
    [FromForm]
    public IFormFile ProfilePhoto { get; set; }

}
