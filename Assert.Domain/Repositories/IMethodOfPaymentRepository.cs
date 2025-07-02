using Assert.Domain.Entities;
using Assert.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Repositories
{
    public interface IMethodOfPaymentRepository
    {
        Task<PayMethodOfPayment> GetByIdAsync(int id);
        Task<List<PayMethodOfPayment>> GetAllAsync(int countryId);
    }
}
