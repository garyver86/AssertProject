using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IAmenitiesRepository
    {
        Task<List<TpAmenitiesType>> GetActives();
    }
}
