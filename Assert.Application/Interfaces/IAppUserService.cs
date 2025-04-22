using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Assert.Application.DTOs;
using Assert.Domain.Models;
using Assert.Infrastructure.Security;

namespace Assert.Domain.Services;

public interface IAppUserService
{
    
    Task<ReturnModelDTO> LoginAndEnrollment(string platform, string token,
            string user, string password);
}
