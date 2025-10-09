using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Repositories
{
    public interface IComplaintRepository
    {
        Task<TbComplaint> GetByComplaintCodeAsync(string complaintCode);
        Task<TbComplaint> GetByBookingIdAsync(long bookingId);
        Task<List<TbComplaint>> GetByComplainantUserIdAsync(int userId);
        Task<List<TbComplaint>> GetByReportedHostIdAsync(int hostId);
        Task<List<TbComplaint>> GetFilteredAsync(ComplaintFilter filter);
        Task<(List<TbComplaint>, PaginationMetadata)> GetPaginatedAsync(ComplaintFilter filter);
        Task<bool> ExistsForBookingAsync(long bookingId);
        Task UpdateStatusAsync(int complaintId, string status, int? adminUserId = null);
        Task AssignComplaintAsync(int complaintId, int adminUserId);
        Task ResolveComplaintAsync(int complaintId, string resolutionType, string resolutionNotes, string internalNotes);
        Task AddAsync(TbComplaint entity);
        Task UpdateAsync(TbComplaint entity);
    }
}
