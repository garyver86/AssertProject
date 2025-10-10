using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ComplaintReasonRepository : IComplaintReasonRepository
    {
        private readonly InfraAssertDbContext _context;
        public ComplaintReasonRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        //public async Task<List<TComplaintReason>> GetAll()
        //{
        //    List<TComplaintReason> complaintReasons = await _context.TComplaintReasons.Where(x => x.IsActive == true || x.IsActive == null).ToListAsync();
        //    return complaintReasons;
        //}

        public async Task<List<TComplaintReason>> GetAll(/*string option = "ALL", int? parentId = null, bool includeInactive = false*/)
        {
            string option = "ALL";
            int? parentId = null;
            bool includeInactive = false;
            var parameters = new[]
            {
                new SqlParameter("@Option", SqlDbType.NVarChar) { Value = (object)option ?? DBNull.Value },
                new SqlParameter("@ParentId", SqlDbType.Int) { Value = (object)parentId ?? DBNull.Value },
                new SqlParameter("@IncludeInactive", SqlDbType.Bit) { Value = includeInactive }
            };

            var reasons = await _context.Set<TComplaintReason>()
                .FromSqlRaw("EXEC [dbo].[GetComplaintReasons] @Option, @ParentId, @IncludeInactive", parameters)
                .AsNoTracking()
                .ToListAsync();

            return reasons;
        }

        public async Task<List<ComplaintReasonHierarchyDto>> GetComplaintReasonsHierarchyAsync(int? parentId = null, bool includeInactive = false)
        {
            var parameters = new[]
            {
                new SqlParameter("@IncludeInactive", includeInactive)
            };

            var results = await _context.Database
            .SqlQueryRaw<ComplaintReasonHierarchyDto>(
                "EXEC [dbo].[GetComplaintReasonsHierarchySPA] @IncludeInactive = {0}",
                includeInactive)
            .ToListAsync();

            return results;
        }

        // Método 5: Obtener por ID con hijos usando LINQ
        public async Task<TComplaintReason?> GetReasonWithChildrenAsync(int id, bool includeInactive = false)
        {
            var query = _context.TComplaintReasons
                .Where(cr => cr.ComplaintReasonId == id);

            if (!includeInactive)
            {
                query = query.Where(cr => cr.IsActive == true);
            }

            return await query
                .Include(cr => cr.InverseParent.Where(c => includeInactive || c.IsActive == true))
                .FirstOrDefaultAsync();
        }

        // Método 3: Obtener usando LINQ con Include (para pocos niveles)
        public async Task<IEnumerable<TComplaintReason>> GetReasonsWithChildrenAsync(int? parentId = null, bool includeInactive = false)
        {
            var query = _context.TComplaintReasons.AsQueryable();

            if (!includeInactive)
            {
                query = query.Where(cr => cr.IsActive == true);
            }

            if (parentId.HasValue)
            {
                query = query.Where(cr => cr.ParentId == parentId);
            }
            else
            {
                query = query.Where(cr => cr.ParentId == null);
            }

            return await query
                .Include(cr => cr.InverseParent.Where(c => includeInactive || c.IsActive == true))
                .OrderBy(cr => cr.ComplaintReasonCode)
                .ToListAsync();
        }
    }

}
