using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingStepViewRepository : IListingStepViewRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly IErrorHandler _errorHandler;
        private readonly DbContextOptions<InfraAssertDbContext> dbOptions;

        public ListingStepViewRepository(InfraAssertDbContext infraAssertDbContext, IErrorHandler errorHandler, IServiceProvider serviceProvider)
        {
            _context = infraAssertDbContext;
            _errorHandler = errorHandler;
            dbOptions = serviceProvider.GetRequiredService<DbContextOptions<InfraAssertDbContext>>();
        }
        public async Task<TlListingStepsView> Get(List<long> listingSteps, int viewTypeId)
        {
            var result = await _context.TlListingStepsViews.FirstOrDefaultAsync(x => x.ViewTypeId == viewTypeId && listingSteps.Contains(x.ListingStepsId ?? 0));
            return result;
        }

        public async Task<TlListingStepsView> Get(int listngStepsViewId)
        {
            var result = await _context.TlListingStepsViews.FirstOrDefaultAsync(x => x.ListngStepsViewId == listngStepsViewId);
            return result;
        }

        public async Task<TlListingStepsView> Get(long listingRentId, string nextViewCode)
        {
            var result = await _context.TlListingStepsViews.FirstOrDefaultAsync(x => x.ViewType.Code == nextViewCode && x.ListingSteps.ListingRentId == listingRentId);
            return result;
        }

        public async Task<ReturnModel> IsAllViewsEndeds(long listingRentId)
        {
            var result = await _context.TlListingStepsViews.
                Include(x => x.ViewType).Where(x => x.ListingSteps.ListingRentId == listingRentId && !(x.IsEnded ?? false)).ToListAsync();
            if (result.Count > 0)
            {
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.Conflict,
                    HasError = true,
                    ResultError = _errorHandler.GetError(ResultStatusCode.Conflict, $"Las vistas {string.Join(", ", result.Select(x => x.ViewType.Code))} no se encuentran finalizadas.", false)
                };
            }
            return new ReturnModel
            {
                StatusCode = ResultStatusCode.OK,
                HasError = false
            };
        }

        public async Task SetEnded(int listngStepsViewId, bool isEnded)
        {
            using (var dbcontext = new InfraAssertDbContext(dbOptions))
            {
                var result = await dbcontext.TlListingStepsViews.FirstOrDefaultAsync(x => x.ListngStepsViewId == listngStepsViewId);
                result.IsEnded = isEnded;
                await _context.SaveChangesAsync();
            }
        }
        public async Task SetEnded(long listingRentId, int viewTypeId, bool isEnded)
        {
            using (var dbcontext = new InfraAssertDbContext(dbOptions))
            {
                var result = await dbcontext.TlListingStepsViews.FirstOrDefaultAsync(x => x.ViewTypeId == viewTypeId && x.ListingSteps.ListingRentId == listingRentId);
                if (result != null)
                {
                    result.IsEnded = isEnded;
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
