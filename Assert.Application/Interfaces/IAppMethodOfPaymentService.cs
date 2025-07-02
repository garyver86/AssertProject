using Assert.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.Interfaces
{
    public interface IAppMethodOfPaymentService
    {
        Task<ReturnModelDTO<PayMethodOfPaymentDTO>> GetByIdAsync(int id);
        Task<ReturnModelDTO<List<PayMethodOfPaymentDTO>>> GetAllAsync(int countryId);
    }
}
