using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Services
{
    public interface IComplaintService
    {
        Task<ReturnModel<TbComplaint>> GetByComplaintCodeAsync(string complaintCode);
        Task<ReturnModel<TbComplaint>> GetByBookingIdAsync(long bookingId);
        Task<ReturnModel<List<TbComplaint>>> GetByComplainantUserIdAsync(int userId);
        Task<ReturnModel<List<TbComplaint>>> GetByReportedHostIdAsync(int hostId);
        Task<ReturnModel<List<TbComplaint>>> GetFilteredAsync(ComplaintFilter filter);
        Task<ReturnModel<(List<TbComplaint>, PaginationMetadata)>> GetPaginatedAsync(ComplaintFilter filter);
        Task<bool> ExistsForBookingAsync(long bookingId);
        Task UpdateStatusAsync(int complaintId, string status, int? adminUserId = null);
        Task AssignComplaintAsync(int complaintId, int adminUserId);
        Task ResolveComplaintAsync(int complaintId, string resolutionType, string resolutionNotes, string internalNotes);
        Task AddAsync(TbComplaint entity);
        Task UpdateAsync(TbComplaint entity);
    }
}
