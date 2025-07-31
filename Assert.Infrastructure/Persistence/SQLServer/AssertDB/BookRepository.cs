using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static System.Reflection.Metadata.BlobBuilder;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class BookRepository(
        InfraAssertDbContext _dbContext,
        IExceptionLoggerService _exceptionLoggerService, IServiceProvider serviceProvider)
        : IBookRepository
    {
        private readonly DbContextOptions<InfraAssertDbContext> dbOptions = serviceProvider.GetRequiredService<DbContextOptions<InfraAssertDbContext>>();

        public async Task<TbBook> GetByIdAsync(long bookId)
        {
            try
            {
                var book = await _dbContext.TbBooks
                    .FirstOrDefaultAsync(b => b.BookId == bookId);

                return book ??
                    throw new NotFoundException($"La reserva con ID {bookId} no fue encontrada. Verifique e intente nuevamente.");
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { bookId });
                throw new InfrastructureException(ex.Message);
            }
        }

        public async Task<List<TbBook>> GetByUserId(long userId)
        {
            try
            {
                var books = await _dbContext.TbBooks
                    .Where(b => b.UserIdRenter == userId)
                    .Include(x => x.ListingRent).ThenInclude(lr => lr.OwnerUser)
                    .Include(x => x.ListingRent.TlListingPhotos).ToListAsync();

                books = books.OrderByDescending(x => x.InitDate).ToList();
                return books ??
                    throw new KeyNotFoundException($"No existen reservas para el  usuaerio con ID {userId}.");
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { userId });
                throw new InfrastructureException(ex.Message);
            }
        }

        public async Task<long> UpsertBookAsync(TbBook incomingBook)
        {
            try
            {
                if (incomingBook.BookId > 0) //update
                {
                    var existingBook = await _dbContext.TbBooks.FindAsync(incomingBook.BookId);
                    if (existingBook == null)
                        throw new NotFoundException($"No se encontro la reserva con ID {incomingBook.BookId}.");

                    if (incomingBook.ListingRentId != default) existingBook.ListingRentId = incomingBook.ListingRentId;
                    if (incomingBook.UserIdRenter != default) existingBook.UserIdRenter = incomingBook.UserIdRenter;
                    if (incomingBook.StartDate != default) existingBook.StartDate = incomingBook.StartDate;
                    if (incomingBook.EndDate != default) existingBook.EndDate = incomingBook.EndDate;
                    if (incomingBook.AmountTotal != default) existingBook.AmountTotal = incomingBook.AmountTotal;
                    if (incomingBook.CurrencyId != default) existingBook.CurrencyId = incomingBook.CurrencyId;
                    if (incomingBook.MountPerNight.HasValue) existingBook.MountPerNight = incomingBook.MountPerNight;
                    if (incomingBook.AmountFees.HasValue) existingBook.AmountFees = incomingBook.AmountFees;
                    if (!string.IsNullOrWhiteSpace(incomingBook.NameRenter)) existingBook.NameRenter = incomingBook.NameRenter;
                    if (!string.IsNullOrWhiteSpace(incomingBook.LastNameRenter)) existingBook.LastNameRenter = incomingBook.LastNameRenter;
                    if (incomingBook.TermsAccepted.HasValue) existingBook.TermsAccepted = incomingBook.TermsAccepted;
                    if (!string.IsNullOrWhiteSpace(incomingBook.AdditionalInfo)) existingBook.AdditionalInfo = incomingBook.AdditionalInfo;
                    if (incomingBook.BookStatusId != default) existingBook.BookStatusId = incomingBook.BookStatusId;
                    if (incomingBook.IsApprobal.HasValue) existingBook.IsApprobal = incomingBook.IsApprobal;
                    if (!string.IsNullOrWhiteSpace(incomingBook.ApprovalDetails)) existingBook.ApprovalDetails = incomingBook.ApprovalDetails;
                    if (incomingBook.IsManualApprobal.HasValue) existingBook.IsManualApprobal = incomingBook.IsManualApprobal;
                    if (incomingBook.DaysToApproval.HasValue) existingBook.DaysToApproval = incomingBook.DaysToApproval;
                    if (incomingBook.InitDate.HasValue) existingBook.InitDate = incomingBook.InitDate;
                    if (!string.IsNullOrWhiteSpace(incomingBook.PaymentCode)) existingBook.PaymentCode = incomingBook.PaymentCode;
                    if (!string.IsNullOrWhiteSpace(incomingBook.Pk)) existingBook.Pk = incomingBook.Pk;
                    if (!string.IsNullOrWhiteSpace(incomingBook.PaymentId)) existingBook.PaymentId = incomingBook.PaymentId;
                    if (incomingBook.DepositSec.HasValue) existingBook.DepositSec = incomingBook.DepositSec;
                    if (!string.IsNullOrWhiteSpace(incomingBook.PickUpLocation)) existingBook.PickUpLocation = incomingBook.PickUpLocation;
                    if (incomingBook.VggFee.HasValue) existingBook.VggFee = incomingBook.VggFee;
                    if (incomingBook.VggFeePercent.HasValue) existingBook.VggFeePercent = incomingBook.VggFeePercent;
                }
                else //insert
                    _dbContext.TbBooks.Add(incomingBook);

                await _dbContext.SaveChangesAsync();
                return incomingBook.BookId;

            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { incomingBook });
                throw new InfrastructureException(ex.Message);
            }
        }

        public async Task<List<TbBook>> GetBooksWithoutReviewByUser(int userId)
        {
            using (var context = new InfraAssertDbContext(dbOptions))
            {
                // Recupera los TbBook del usuario que no tienen review asociado
                var booksWithoutReview = await context.Set<TbBook>()
                    .Where(b => b.UserIdRenter == userId)
                    .Where(b => !context.TlListingReviews.Any(r => r.Book != null && r.Book.BookId == b.BookId))
                    .Include(x => x.ListingRent).ThenInclude(lr => lr.OwnerUser)
                    .Include(x => x.ListingRent.TlListingPhotos)
                    .ToListAsync();
                booksWithoutReview = booksWithoutReview.OrderByDescending(x => x.InitDate).ToList();
                return booksWithoutReview;
            }
        }
    }
}
