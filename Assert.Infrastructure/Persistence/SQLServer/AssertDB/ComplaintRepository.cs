using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly DbContextOptions<InfraAssertDbContext> dbOptions;
        public ComplaintRepository(IServiceProvider serviceProvider, InfraAssertDbContext context)
        {
            dbOptions = serviceProvider.GetRequiredService<DbContextOptions<InfraAssertDbContext>>();
            _context = context;
        }
        public async Task AddAsync(TbComplaint entity)
        {
            using (var context = new InfraAssertDbContext(dbOptions))
            {
                await _context.TbComplaints.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AssignComplaintAsync(int complaintId, int adminUserId)
        {
            using (var context = new InfraAssertDbContext(dbOptions))
            {
                var complaint = await _context.TbComplaints.FirstOrDefaultAsync(x => x.ComplaintId == complaintId);
                if (complaint != null)
                {
                    complaint.AssignedAdminId = adminUserId;
                    complaint.AssignedDate = DateTime.UtcNow;
                    complaint.ComplaintStatus = "UNDER_REVIEW";
                    complaint.ComplaintStatusId = 2; // Assuming 2 corresponds to "UNDER_REVIEW"

                    _context.TbComplaints.Update(complaint);
                }
            }
        }

        public async Task<bool> ExistsForBookingAsync(long bookingId)
        {
            return await _context.TbComplaints.AnyAsync(c => c.BookingId == bookingId);
        }

        public async Task<TbComplaint> GetByBookingIdAsync(long bookingId)
        {
            return await _context.TbComplaints
                .Include(x => x.ComplaintReason)
                .Include(x => x.ComplaintStatusNavigation)
                .Include(x => x.TbComplaintEvidences)
                .FirstOrDefaultAsync(cr => cr.BookingId == bookingId);
        }

        public async Task<List<TbComplaint>> GetByComplainantUserIdAsync(int userId)
        {
            return await _context.TbComplaints
                .Include(x => x.ComplaintReason)
                .Include(x => x.ComplaintStatusNavigation)
                .Include(x => x.TbComplaintEvidences)
                .Where(cr => cr.ComplainantUserId == userId).ToListAsync();
        }

        public async Task<TbComplaint> GetByComplaintCodeAsync(string complaintCode)
        {
            return await _context.TbComplaints
                .Include(x => x.ComplaintReason)
                .Include(x => x.ComplaintStatusNavigation)
                .Include(x => x.TbComplaintEvidences)
                .FirstOrDefaultAsync(cr => cr.ComplainCode == complaintCode);
        }

        public async Task<List<TbComplaint>> GetByReportedHostIdAsync(int hostId)
        {
            return await _context.TbComplaints
                .Include(x => x.ComplaintReason)
                .Include(x => x.ComplaintStatusNavigation)
                .Include(x => x.TbComplaintEvidences)
                .Where(cr => cr.ReportedHostId == hostId).ToListAsync();
        }

        public async Task<List<TbComplaint>> GetFilteredAsync(ComplaintFilter filter)
        {
            var query = _context.TbComplaints
                .Include(c => c.ComplaintReason)
                .Include(x => x.ComplaintStatusNavigation)
                .Include(x => x.TbComplaintEvidences)
            .AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(c => c.ComplaintStatus == filter.Status);

            if (filter.ReportedHostId.HasValue)
                query = query.Where(c => c.ReportedHostId == filter.ReportedHostId.Value);

            if (filter.ComplainantUserId.HasValue)
                query = query.Where(c => c.ComplainantUserId == filter.ComplainantUserId.Value);

            if (!string.IsNullOrEmpty(filter.Priority))
                query = query.Where(c => c.ComplaintPriority == filter.Priority);

            if (filter.StartDate.HasValue)
                query = query.Where(c => c.ComplaintDate >= filter.StartDate.Value);

            if (filter.EndDate.HasValue)
                query = query.Where(c => c.ComplaintDate <= filter.EndDate.Value);


            if (filter.UserId.HasValue)
                query = query.Where(c => c.ComplainantUserId <= filter.UserId.Value);


            if (filter.HostId.HasValue)
                query = query.Where(c => c.ReportedHostId <= filter.HostId.Value);

            // Aplicar paginación
            var complaints = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return complaints;
        }

        public async Task<(List<TbComplaint>, PaginationMetadata)> GetPaginatedAsync(ComplaintFilter filter)
        {
            var query = _context.TbComplaints
                .Include(c => c.ComplaintReason)
                .Include(x => x.ComplaintStatus)
                .Include(x => x.TbComplaintEvidences)
            .AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(c => c.ComplaintStatus == filter.Status);

            if (filter.ReportedHostId.HasValue)
                query = query.Where(c => c.ReportedHostId == filter.ReportedHostId.Value);

            if (filter.ComplainantUserId.HasValue)
                query = query.Where(c => c.ComplainantUserId == filter.ComplainantUserId.Value);

            if (!string.IsNullOrEmpty(filter.Priority))
                query = query.Where(c => c.ComplaintPriority == filter.Priority);

            if (filter.StartDate.HasValue)
                query = query.Where(c => c.ComplaintDate >= filter.StartDate.Value);

            if (filter.EndDate.HasValue)
                query = query.Where(c => c.ComplaintDate <= filter.EndDate.Value);


            if (filter.UserId.HasValue)
                query = query.Where(c => c.ComplainantUserId <= filter.UserId.Value);


            if (filter.HostId.HasValue)
                query = query.Where(c => c.ReportedHostId <= filter.HostId.Value);

            // Obtener total antes de paginación
            var totalCount = await query.CountAsync();

            // Aplicar paginación
            var complaints = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var pagination = new PaginationMetadata
            {
                CurrentPage = filter.Page,
                PageSize = filter.PageSize,
                TotalItemCount = totalCount,
                TotalPageCount = (int)Math.Ceiling((double)totalCount / filter.PageSize)
            };

            return (complaints, pagination);
        }

        public async Task ResolveComplaintAsync(int complaintId, string resolutionType, string resolutionNotes, string internalNotes)
        {
            using (var context = new InfraAssertDbContext(dbOptions))
            {
                var complaint = await context.TbComplaints.FirstOrDefaultAsync(x => x.ComplaintId == complaintId);
                if (complaint != null)
                {
                    complaint.ResolutionType = resolutionType;
                    complaint.ResolutionNotes = resolutionNotes;
                    complaint.InternalNotes = internalNotes;
                    complaint.ResolutionDate = DateTime.UtcNow;
                    complaint.ComplaintStatus = "RESOLVED";
                    complaint.ComplaintStatusId = 3; // Assuming 3 corresponds to "RESOLVED"

                    context.TbComplaints.Update(complaint);
                }
            }
        }

        public async Task UpdateAsync(TbComplaint entity)
        {
            using (var context = new InfraAssertDbContext(dbOptions))
            {
                TbComplaint _entity = new TbComplaint
                {
                    AssignedAdminId = entity.AssignedAdminId,
                    AssignedDate = entity.AssignedDate,
                    BookingId = entity.BookingId,
                    ComplainantUserId = entity.ComplainantUserId,
                    ComplainCode = entity.ComplainCode,
                    ComplaintDate = entity.ComplaintDate,
                    ComplaintId = entity.ComplaintId,
                    ComplaintPriority = entity.ComplaintPriority,
                    ComplaintReasonId = entity.ComplaintReasonId,
                    ComplaintStatusId = entity.ComplaintStatusId,
                    ComplaintStatus = entity.ComplaintStatus,
                    CreatedAt = entity.CreatedAt,
                    FreeTextDescription = entity.FreeTextDescription,
                    InternalNotes = entity.InternalNotes,
                    IpAddress = entity.IpAddress,
                    ReportedHostId = entity.ReportedHostId,
                    ResolutionDate = entity.ResolutionDate,
                    ResolutionNotes = entity.ResolutionNotes,
                    ResolutionType = entity.ResolutionType,
                    UpdatedAt = entity.UpdatedAt,
                    UserAgent = entity.UserAgent
                };
                _context.TbComplaints.Update(entity);
            }
        }

        public async Task UpdateStatusAsync(int complaintId, string status, int? adminUserId = null)
        {
            using (var context = new InfraAssertDbContext(dbOptions))
            {
                var complaint = await context.TbComplaints.FirstOrDefaultAsync(x => x.ComplaintId == complaintId);
                if (complaint != null)
                {
                    complaint.ComplaintStatus = status;

                    if (adminUserId.HasValue)
                        complaint.AssignedAdminId = adminUserId.Value;

                    context.Update(complaint);
                }
            }
        }
    }
}
