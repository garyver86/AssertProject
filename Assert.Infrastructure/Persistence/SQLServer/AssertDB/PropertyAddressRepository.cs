using Assert.Domain.Entities;
using Assert.Domain.Repositories;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class PropertyAddressRepository : IPropertyAddressRepository
    {
        public Task<TpPropertyAddress> Add(TpPropertyAddress addresInput)
        {
            throw new NotImplementedException();
        }
    }
}
