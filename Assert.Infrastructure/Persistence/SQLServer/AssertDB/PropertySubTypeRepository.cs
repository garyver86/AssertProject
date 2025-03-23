using Assert.Domain.Entities;
using Assert.Domain.Repositories;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class PropertySubTypeRepository : IPropertySubTypeRepository
    {
        public Task<TpPropertySubtype> Get(int? subtypeId)
        {
            throw new NotImplementedException();
        }

        public Task<TpPropertySubtype> GetActive(int? subtypeId)
        {
            throw new NotImplementedException();
        }
    }
}
