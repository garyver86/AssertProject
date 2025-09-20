using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IReasonRefuseBookRepository
    {
        Task<List<TReasonRefusedBook>> GetActives();
    }
}
