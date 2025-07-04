using Assert.Application.DTOs.Responses;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.Interfaces
{
    public interface IAppBookService
    {
        Task<ReturnModelDTO<PayPriceCalculationDTO>> CalculatePrice(long listingRentId, DateTime startDate, DateTime endDate, int guestId,
           Dictionary<string, string> clientData, bool useTechnicalMessages);
    }
}
