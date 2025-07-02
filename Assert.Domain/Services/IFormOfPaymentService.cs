using Assert.Domain.Entities;
using Assert.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Services
{
    public interface IMethodOfPaymentService
    {
        Task<ReturnModel<PayMethodOfPayment>> GetByIdAsync(int id);
        Task<ReturnModel<List<PayMethodOfPayment>>> GetAllAsync(int countryId);
    }
}
