using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.Interfaces
{
    public interface IAppCompliantService
    {
        Task<ReturnModelDTO<ComplaintDTO>> GetByComplaintCodeAsync(string complaintCode);
        Task<ReturnModelDTO<ComplaintDTO>> GetByBookingIdAsync(int bookingId);
        Task<ReturnModelDTO<List<ComplaintDTO>>> GetByComplainantUserIdAsync(int userId);
        Task<ReturnModelDTO<List<ComplaintDTO>>> GetByReportedHostIdAsync(int hostId);
        Task<ReturnModelDTO<List<ComplaintDTO>>> GetFilteredAsync(ComplaintFilterDto filter);
        Task<ReturnModelDTO<(List<ComplaintDTO>, PaginationMetadataDTO)>> GetPaginatedAsync(ComplaintFilterDto filter);
        Task<bool> ExistsForBookingAsync(int bookingId);
        Task UpdateStatusAsync(int complaintId, string status, int? adminUserId = null);
        Task AssignComplaintAsync(int complaintId, int adminUserId);
        Task ResolveComplaintAsync(int complaintId, CompliantResolutionRequest resolutionInfo);
        Task<ReturnModelDTO<ComplaintDTO>> AddAsync(ComplaintRequestDTO entity);
        Task UpdateAsync(ComplaintRequestDTO entity);
    }
}
