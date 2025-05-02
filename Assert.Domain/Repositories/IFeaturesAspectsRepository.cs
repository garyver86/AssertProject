using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IFeaturesAspectsRepository
    {
        Task<List<TFeaturedAspectType>> GetActives();
    }
}
