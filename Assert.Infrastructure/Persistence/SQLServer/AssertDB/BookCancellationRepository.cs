using Assert.Domain.Common.Metadata;
using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB;

public class BookCancellationRepository
    (IBookStatusRepository _bookStatusRepository, 
    IExceptionLoggerService _exceptionLoggerService,
    InfraAssertDbContext _dbContext,
    RequestMetadata _metadata)
    : IBookCancellationRepository
{
    public async Task<string> UpsertHostBookCancellation(int bookCancellationId,
        int bookId, int cancellationReasonId, string messageTo, string message)
    {
        try
        {
            var book = await _dbContext.TbBooks.FirstOrDefaultAsync(b => b.BookId == bookId);
            if (book == null)
                throw new NotFoundException($"No existe la reserva con identificador: {bookId}");

            if(bookCancellationId == 0) //insert
            {
                await _dbContext.TbBookCancellations.AddAsync(new()
                {
                    BookId = bookId,
                    ListingRentId = book.ListingRentId,
                    UserId = _metadata.UserId,
                    CancellationTypeCode = "BYHOST",
                    CancellationReasonId = cancellationReasonId,
                    MessageToAssert = messageTo == "ASSERT" ? message : "",
                    MessageToGuest = messageTo == "GUEST" ? message : "",
                    MessageToHost = messageTo == "HOST" ? message : "",
                    CustomMessage = "",
                    CreatedAt = DateTime.UtcNow,
                    Status = "AC"
                });
            }
            else //update
            {
                var bookCancellation = await _dbContext.TbBookCancellations
                    .FirstOrDefaultAsync(bc => bc.BookCacellationId == bookCancellationId);

                if(bookCancellation == null)
                    throw new NotFoundException($"No existe cancelacion de reserva con identificador: {bookCancellationId}");

                bookCancellation.CancellationReasonId = cancellationReasonId;
                if (messageTo == "ASSERT")
                    bookCancellation.MessageToAssert = message;
                else if (messageTo == "GUEST")
                    bookCancellation.MessageToGuest = message;
                else if (messageTo == "HOST")
                    bookCancellation.MessageToHost = message;
                bookCancellation.CreatedAt = DateTime.UtcNow;
            }

            var statusNonApproved = await _bookStatusRepository.GetStatusByCode("non_approved");
            book.BookStatusId = statusNonApproved.BookStatusId;

            await _dbContext.SaveChangesAsync();

            return "SAVED";
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, new { _metadata.UserId });
            throw new InfrastructureException(ex.Message);
        }
    }

    public async Task<List<TbBookCancellation>> GetHostBookCancellations(long bookId)
    {
        try
        {
            var result = await _dbContext.TbBookCancellations
                .Include(bc => bc.CancellationReason)
                    .ThenInclude(c => c.CancellationGroup)
                .Where(bc => bc.BookId == bookId 
                    && bc.CancellationTypeCode == "BYHOST"
                    && bc.Status == "AC")
                .ToListAsync();

            if (result is null)
                throw new NotFoundException($"No existe reserva con Id: {bookId}");

            return result;
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, new { _metadata.UserId });
            throw new InfrastructureException(ex.Message);
        }
    }
}
