using Assert.Domain.Entities;
using Assert.Domain.Repositories;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class AccommodationTypeRepository : IAccommodationTypeRepository
    {
        public Task<TlAccommodationType> GetActive(int? accomodationId)
        {
            throw new NotImplementedException();
        }
    }
}
