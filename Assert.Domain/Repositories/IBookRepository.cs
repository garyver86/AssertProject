using Assert.Domain.Entities;

namespace Assert.Domain.Repositories;

public interface IBookRepository
{
    Task<List<TbBook>> GetByUserId(long userId, int[]? statuses);
    Task<List<TbBook>> GetByOwnerId(long userId, int[]? statuses);
    Task<TbBook> GetByIdAsync(long bookId);
    Task<long> UpsertBookAsync(TbBook book);
    Task<List<TbBook>> GetBooksWithoutReviewByUser(int userId);
    Task<TbBook> Cancel(int userId, long bookId);
    Task<List<TbBook>> GetPendingAcceptance(int userId);

    Task<List<TbBook>> GetCancelablesBookings(int userId);
    Task<List<TbBook>> GetApprovedsWOInit(int userId);
    Task<List<TbBook>> GetPendingAcceptanceForRenter(int userId);
    Task<TbBook> AuthorizationResponse(int userId, long bookId, bool isApproval, int? reasonRefused);
    Task CheckAndExpireReservation(DateTime expirationThreshold);
    Task CheckAndFinishReservation(DateTime expirationThreshold);
    Task SetReviewDateTime(long bookId);
    Task CancelOtherRequests(long listingRentId, DateTime startDate, DateTime endDate, long bookId);
    Task<List<TbBook>> GetPayedsByOwnerId(long userId);
}
