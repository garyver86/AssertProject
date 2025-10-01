using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
                    .Include(us => us.CancellationUser)
                    .Include(lr => lr.ListingRent)
                    .ThenInclude(pr => pr.TpProperties)
                    .Include(pr => pr.ListingRent.TlListingPrices)
                    .Include(pr => pr.ListingRent.TlListingPhotos)
                    .Include(pr => pr.ListingRent.OwnerUser)
                    .Include(pr => pr.PayPriceCalculations)
                    .Include(pr => pr.CancellationUser)
                    .FirstOrDefaultAsync(b => b.BookId == bookId);

                if (book?.ListingRent?.TpProperties?.Count > 0)
                {
                    TpProperty prop = ((List<TpProperty>)book.ListingRent.TpProperties).FirstOrDefault();
                    ((List<TpProperty>)book.ListingRent.TpProperties).FirstOrDefault().TpPropertyAddresses = new List<TpPropertyAddress>
                    {
                        new TpPropertyAddress
                        {
                            Address1 = prop.Address1,
                            Address2 = prop.Address2,
                            CityId = prop.CityId,
                            CountyId = prop.CountyId,
                            ZipCode = prop.ZipCode,
                            StateId = prop.StateId,
                            City = new TCity
                            {
                                CityId = prop.CityId??0,
                                Name = prop.CityName,
                                CountyId = prop.CountyId??0,
                                County = new TCounty
                                {
                                    CountyId = prop.CountyId ?? 0,
                                    Name = prop.CountyName,
                                    StateId = prop.StateId??0,
                                    State = new TState
                                    {
                                        Name = prop.StateName,
                                        StateId = prop.StateId ?? 0,
                                        Country = new TCountry
                                        {
                                            Name = prop.CountryName,
                                            CountryId = prop.CountryId ?? 0
                                        }
                                    }
                                }
                            }
                        }
                    };
                }

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

        public async Task<List<TbBook>> GetByUserId(long userId, int[]? statuses)
        {
            try
            {
                //List<TbBook> books = await _dbContext.TbBooks
                //    .Where(b => b.UserIdRenter == userId && (statuses == null || !statuses.Any() || statuses.Contains(b.BookStatusId)))
                //    .Include(x => x.ListingRent).ThenInclude(lr => lr.OwnerUser)
                //    .Include(x => x.ListingRent.TlListingPhotos)
                //    .Include(x => x.ListingRent.ApprovalPolicyType)
                //    .Include(x => x.ListingRent.TpProperties)
                //    .Include(x => x.BookStatus)
                //    .ToListAsync();

                List<TbBook> books = await _dbContext.TbBooks
                    .Where(b => b.UserIdRenter == userId && (statuses == null || !statuses.Any() || statuses.Contains(b.BookStatusId)))
                    .Include(lr => lr.ListingRent.OwnerUser)
                    .Include(x => x.ListingRent.TlListingPhotos)
                    .Include(x => x.ListingRent.ApprovalPolicyType)
                    .Include(x => x.ListingRent.TpProperties)
                    .Select(x => new TbBook
                    {
                        AdditionalInfo = x.AdditionalInfo,
                        AmountFees = x.AmountFees,
                        AmountTotal = x.AmountTotal,
                        ApprovalDetails = x.ApprovalDetails,
                        BookId = x.BookId,
                        BookStatus = x.BookStatusId > 0 ? new TbBookStatus
                        {
                            BookStatusId = x.BookStatusId,
                            Code = x.BookStatus.Code,
                            Description = x.BookStatus.Description,
                            Name = x.BookStatus.Name
                        } : null,
                        BookStatusId = x.BookStatusId,
                        Cancellation = x.Cancellation,
                        CancellationEnd = x.CancellationEnd,
                        CancellationStart = x.CancellationStart,
                        //CancellationUser = x.CancellationUser,
                        CancellationUserId = x.CancellationUserId,
                        Checkin = x.Checkin,
                        Checkout = x.Checkout,
                        Currency = new TCurrency
                        {
                            Code = x.Currency.Code,
                            CountryCode = x.Currency.CountryCode,
                            CurrencyId = x.CurrencyId,
                            Name = x.Currency.Name,
                            Symbol = x.Currency.Symbol
                        },
                        CurrencyId = x.CurrencyId,
                        DaysToApproval = x.DaysToApproval,
                        DepositSec = x.DepositSec,
                        EndDate = x.EndDate,
                        ExpiredDateTime = x.ExpiredDateTime,
                        GuestCheckin = x.GuestCheckin,
                        GuestCheckout = x.GuestCheckout,
                        InitDate = x.InitDate,
                        IsApprobal = x.IsApprobal,
                        IsManualApprobal = x.IsManualApprobal,
                        LastNameRenter = x.LastNameRenter,
                        ListingRent = x.ListingRent,
                        ListingRentId = x.ListingRentId,
                        MaxCheckin = x.MaxCheckin,
                        MountPerNight = x.MountPerNight,
                        NameRenter = x.NameRenter,
                        PaymentCode = x.PaymentCode,
                        PaymentId = x.PaymentId,
                        PayPriceCalculations = x.PayPriceCalculations.Any() ? new PayPriceCalculation[]{
                            new PayPriceCalculation
                            {
                              Amount =  x.PayPriceCalculations.First().Amount,
                              BreakdownInfo = x.PayPriceCalculations.First().BreakdownInfo,
                              CalculationCode = x.PayPriceCalculations.First().CalculationCode,
                            }
                        } : null,
                        PickUpLocation = x.PickUpLocation,
                        Pk = x.Pk,
                        ReasonRefused = x.ReasonRefusedId > 0 ? new TReasonRefusedBook
                        {
                            ReasonRefusedCode = x.ReasonRefused.ReasonRefusedCode,
                            ReasonRefusedId = x.ReasonRefused.ReasonRefusedId,
                            ReasonRefusedName = x.ReasonRefused.ReasonRefusedName,
                            ReasonRefusedText = x.ReasonRefused.ReasonRefusedText
                        } : null,
                        ReasonRefusedId = x.ReasonRefusedId,
                        StartDate = x.StartDate,

                    })
                    .ToListAsync();

                //// Luego carga los BookStatus por separado si los necesitas
                //var bookStatusIds = books.Select(b => b.BookStatusId).Distinct().ToList();
                //var statusesDict = await _dbContext.TbBookStatuses
                //    .Where(s => bookStatusIds.Contains(s.BookStatusId))
                //    .ToDictionaryAsync(s => s.BookStatusId);

                //// Asignar los BookStatus a cada libro
                //foreach (var book in books)
                //{
                //    if (statusesDict.TryGetValue(book.BookStatusId, out var status))
                //    {
                //        status.TbBooks = null;
                //        book.BookStatus = status;
                //    }
                //}

                if (books != null)
                {
                    foreach (var book in books)
                    {
                        if (book?.ListingRent?.TpProperties?.Count > 0)
                        {
                            foreach (var prop in book.ListingRent.TpProperties)
                            {
                                prop.TpPropertyAddresses = new List<TpPropertyAddress>
                                {
                                    new TpPropertyAddress
                                    {
                                        Address1 = prop.Address1,
                                        Address2 = prop.Address2,
                                        CityId = prop.CityId,
                                        CountyId = prop.CountyId,
                                        ZipCode = prop.ZipCode,
                                        StateId = prop.StateId,
                                        City = new TCity
                                        {
                                            CityId = prop.CityId??0,
                                            Name = prop.CityName,
                                            CountyId = prop.CountyId??0,
                                            County = new TCounty
                                            {
                                                CountyId = prop.CountyId ?? 0,
                                                Name = prop.CountyName,
                                                StateId = prop.StateId??0,
                                                State = new TState
                                                {
                                                    Name = prop.StateName,
                                                    StateId = prop.StateId ?? 0,
                                                    Country = new TCountry
                                                    {
                                                        Name = prop.CountryName,
                                                        CountryId = prop.CountryId ?? 0
                                                    }
                                                }
                                            }
                                        }
                                    }
                                };
                            }
                        }
                    }
                }

                books = books.OrderByDescending(x => x.InitDate).ToList();
                return books ?? new List<TbBook>();
                //throw new KeyNotFoundException($"No existen reservas para el  usuario con ID {userId}.");
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { userId });
                throw new InfrastructureException(ex.Message);
            }
        }

        public async Task<List<TbBook>> GetByOwnerId(long userId, int[]? statuses)
        {
            try
            {
                List<TbBook> books = await _dbContext.TbBooks
                    .Where(b => b.ListingRent.OwnerUserId == userId && (statuses == null || !statuses.Any() || statuses.Contains(b.BookStatusId)))
                    .Include(lr => lr.ListingRent.OwnerUser)
                    .Include(x => x.ListingRent.TlListingPhotos)
                    .Include(x => x.ListingRent.ApprovalPolicyType)
                    .Include(x => x.ListingRent.TpProperties)
                    .Select(x => new TbBook
                    {
                        AdditionalInfo = x.AdditionalInfo,
                        AmountFees = x.AmountFees,
                        AmountTotal = x.AmountTotal,
                        ApprovalDetails = x.ApprovalDetails,
                        BookId = x.BookId,
                        BookStatus = x.BookStatusId > 0 ? new TbBookStatus
                        {
                            BookStatusId = x.BookStatusId,
                            Code = x.BookStatus.Code,
                            Description = x.BookStatus.Description,
                            Name = x.BookStatus.Name
                        } : null,
                        BookStatusId = x.BookStatusId,
                        Cancellation = x.Cancellation,
                        CancellationEnd = x.CancellationEnd,
                        CancellationStart = x.CancellationStart,
                        //CancellationUser = x.CancellationUser,
                        CancellationUserId = x.CancellationUserId,
                        Checkin = x.Checkin,
                        Checkout = x.Checkout,
                        Currency = new TCurrency
                        {
                            Code = x.Currency.Code,
                            CountryCode = x.Currency.CountryCode,
                            CurrencyId = x.CurrencyId,
                            Name = x.Currency.Name,
                            Symbol = x.Currency.Symbol
                        },
                        CurrencyId = x.CurrencyId,
                        DaysToApproval = x.DaysToApproval,
                        DepositSec = x.DepositSec,
                        EndDate = x.EndDate,
                        ExpiredDateTime = x.ExpiredDateTime,
                        GuestCheckin = x.GuestCheckin,
                        GuestCheckout = x.GuestCheckout,
                        InitDate = x.InitDate,
                        IsApprobal = x.IsApprobal,
                        IsManualApprobal = x.IsManualApprobal,
                        LastNameRenter = x.LastNameRenter,
                        ListingRent = x.ListingRent,
                        ListingRentId = x.ListingRentId,
                        MaxCheckin = x.MaxCheckin,
                        MountPerNight = x.MountPerNight,
                        NameRenter = x.NameRenter,
                        PaymentCode = x.PaymentCode,
                        PaymentId = x.PaymentId,
                        PayPriceCalculations = x.PayPriceCalculations.Any() ? new PayPriceCalculation[]{
                            new PayPriceCalculation
                            {
                              Amount =  x.PayPriceCalculations.First().Amount,
                              BreakdownInfo = x.PayPriceCalculations.First().BreakdownInfo,
                              CalculationCode = x.PayPriceCalculations.First().CalculationCode,
                            }
                        } : null,
                        PickUpLocation = x.PickUpLocation,
                        Pk = x.Pk,
                        ReasonRefused = x.ReasonRefusedId > 0 ? new TReasonRefusedBook
                        {
                            ReasonRefusedCode = x.ReasonRefused.ReasonRefusedCode,
                            ReasonRefusedId = x.ReasonRefused.ReasonRefusedId,
                            ReasonRefusedName = x.ReasonRefused.ReasonRefusedName,
                            ReasonRefusedText = x.ReasonRefused.ReasonRefusedText
                        } : null,
                        ReasonRefusedId = x.ReasonRefusedId,
                        StartDate = x.StartDate,

                    })
                    .ToListAsync();

                //// Luego carga los BookStatus por separado si los necesitas
                //var bookStatusIds = books.Select(b => b.BookStatusId).Distinct().ToList();
                //var statusesDict = await _dbContext.TbBookStatuses
                //    .Where(s => bookStatusIds.Contains(s.BookStatusId))
                //    .ToDictionaryAsync(s => s.BookStatusId);

                //// Asignar los BookStatus a cada libro
                //foreach (var book in books)
                //{
                //    if (statusesDict.TryGetValue(book.BookStatusId, out var status))
                //    {
                //        status.TbBooks = null;
                //        book.BookStatus = status;
                //    }
                //}

                if (books != null)
                {
                    foreach (var book in books)
                    {
                        if (book?.ListingRent?.TpProperties?.Count > 0)
                        {
                            foreach (var prop in book.ListingRent.TpProperties)
                            {
                                prop.TpPropertyAddresses = new List<TpPropertyAddress>
                                {
                                    new TpPropertyAddress
                                    {
                                        Address1 = prop.Address1,
                                        Address2 = prop.Address2,
                                        CityId = prop.CityId,
                                        CountyId = prop.CountyId,
                                        ZipCode = prop.ZipCode,
                                        StateId = prop.StateId,
                                        City = new TCity
                                        {
                                            CityId = prop.CityId??0,
                                            Name = prop.CityName,
                                            CountyId = prop.CountyId??0,
                                            County = new TCounty
                                            {
                                                CountyId = prop.CountyId ?? 0,
                                                Name = prop.CountyName,
                                                StateId = prop.StateId??0,
                                                State = new TState
                                                {
                                                    Name = prop.StateName,
                                                    StateId = prop.StateId ?? 0,
                                                    Country = new TCountry
                                                    {
                                                        Name = prop.CountryName,
                                                        CountryId = prop.CountryId ?? 0
                                                    }
                                                }
                                            }
                                        }
                                    }
                                };
                            }
                        }
                    }
                }

                books = books.OrderByDescending(x => x.InitDate).ToList();
                return books ?? new List<TbBook>();
                //throw new KeyNotFoundException($"No existen reservas para el  usuario con ID {userId}.");
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
                var user = await _dbContext.TuUsers.FirstOrDefaultAsync(x => x.UserId == incomingBook.UserIdRenter);
                if (user is null) throw new NotFoundException("El usuario no existe");
                if (user.Status == "UN")
                    throw new UnauthorizedException("El usuario se encuentra bloqueado, no puede realizar reservas.");
                if (user.Status == "IN")
                    throw new UnauthorizedException("El usuario se encuentra inactivo, no puede realizar reservas.");


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
                    if (incomingBook.Checkin.HasValue) existingBook.Checkin = incomingBook.Checkin;
                    if (incomingBook.Checkout.HasValue) existingBook.Checkout = incomingBook.Checkout;
                    if (incomingBook.CancellationStart.HasValue) existingBook.CancellationStart = incomingBook.CancellationStart;
                    if (incomingBook.CancellationEnd.HasValue) existingBook.CancellationEnd = incomingBook.CancellationEnd;
                }
                else //insert
                {
                    incomingBook.PayPriceCalculations = null;
                    incomingBook.TbBookChanges = null;
                    incomingBook.TbBookingInsurances = null;
                    incomingBook.TbBookInsuranceClaims = null;
                    incomingBook.TlListingReviews = null;
                    incomingBook.TmConversations = null;
                    incomingBook.TbBookPayments = null;
                    incomingBook.TbBookSnapshots = null;
                    incomingBook.TbBookSteps = null;
                    incomingBook.TiIssues = null;
                    incomingBook.TuUserReviews = null;
                    incomingBook.TlListingAvailabilities = null;
                    incomingBook.TlListingCalendars = null;
                    incomingBook.TlListingReviews = null;
                    _dbContext.TbBooks.Add(incomingBook);
                }

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
                    //.Where(b => !context.TlListingReviews.Any(r => r.Book != null && r.Book.BookId == b.BookId))
                    .Where(b => !context.TlListingReviews.Any(r => r.Book != null && r.Book.BookId == b.BookId) || context.TlListingReviews.Where(x => x.Book != null && x.Book.BookId == b.BookId && x.IsComplete != true).FirstOrDefault() != null)
                    .Include(x => x.ListingRent).ThenInclude(lr => lr.OwnerUser)
                    .Include(x => x.ListingRent.TlListingPhotos)
                    .Include(x => x.ListingRent.ApprovalPolicyType)
                    .Include(x => x.ListingRent.TpProperties)
                    .Include(x => x.ListingRent.OwnerUser)
                    .ToListAsync();

                if (booksWithoutReview != null)
                {
                    foreach (var book in booksWithoutReview)
                    {
                        if (book?.ListingRent?.TpProperties?.Count > 0)
                        {
                            foreach (var prop in book.ListingRent.TpProperties)
                            {
                                prop.TpPropertyAddresses = new List<TpPropertyAddress>
                                {
                                    new TpPropertyAddress
                                    {
                                        Address1 = prop.Address1,
                                        Address2 = prop.Address2,
                                        CityId = prop.CityId,
                                        CountyId = prop.CountyId,
                                        ZipCode = prop.ZipCode,
                                        StateId = prop.StateId,
                                        City = new TCity
                                        {
                                            CityId = prop.CityId??0,
                                            Name = prop.CityName,
                                            CountyId = prop.CountyId??0,
                                            County = new TCounty
                                            {
                                                CountyId = prop.CountyId ?? 0,
                                                Name = prop.CountyName,
                                                StateId = prop.StateId??0,
                                                State = new TState
                                                {
                                                    Name = prop.StateName,
                                                    StateId = prop.StateId ?? 0,
                                                    Country = new TCountry
                                                    {
                                                        Name = prop.CountryName,
                                                        CountryId = prop.CountryId ?? 0
                                                    }
                                                }
                                            }
                                        }
                                    }
                                };
                            }
                        }
                    }
                }
                booksWithoutReview = booksWithoutReview.OrderByDescending(x => x.InitDate).ToList();
                return booksWithoutReview;
            }
        }

        public async Task<List<TbBook>> GetPendingAcceptance(int userId)
        {
            using (var context = new InfraAssertDbContext(dbOptions))
            {
                // Recupera los TbBook del usuario que no tienen review asociado
                var booksWithoutAcceptation = await context.Set<TbBook>()
                    .Where(b => b.BookStatusId == 1 && b.ListingRent.OwnerUserId == userId)
                    .Include(x => x.ListingRent).ThenInclude(lr => lr.OwnerUser)
                    .Include(x => x.ListingRent.TlListingPhotos)
                    .Include(x => x.ListingRent.ApprovalPolicyType)
                    .Include(x => x.ListingRent.TpProperties)
                    .Include(x => x.ListingRent.OwnerUser)
                    .ToListAsync();

                if (booksWithoutAcceptation != null)
                {
                    foreach (var book in booksWithoutAcceptation)
                    {
                        if (book?.ListingRent?.TpProperties?.Count > 0)
                        {
                            foreach (var prop in book.ListingRent.TpProperties)
                            {
                                prop.TpPropertyAddresses = new List<TpPropertyAddress>
                                {
                                    new TpPropertyAddress
                                    {
                                        Address1 = prop.Address1,
                                        Address2 = prop.Address2,
                                        CityId = prop.CityId,
                                        CountyId = prop.CountyId,
                                        ZipCode = prop.ZipCode,
                                        StateId = prop.StateId,
                                        City = new TCity
                                        {
                                            CityId = prop.CityId??0,
                                            Name = prop.CityName,
                                            CountyId = prop.CountyId??0,
                                            County = new TCounty
                                            {
                                                CountyId = prop.CountyId ?? 0,
                                                Name = prop.CountyName,
                                                StateId = prop.StateId??0,
                                                State = new TState
                                                {
                                                    Name = prop.StateName,
                                                    StateId = prop.StateId ?? 0,
                                                    Country = new TCountry
                                                    {
                                                        Name = prop.CountryName,
                                                        CountryId = prop.CountryId ?? 0
                                                    }
                                                }
                                            }
                                        }
                                    }
                                };
                            }
                        }
                    }
                }
                //booksWithoutAcceptation = booksWithoutAcceptation.OrderByDescending(x => x.InitDate).ToList();
                return booksWithoutAcceptation;
            }
        }


        public async Task<List<TbBook>> GetPendingAcceptanceForRenter(int userId)
        {
            using (var context = new InfraAssertDbContext(dbOptions))
            {
                // Recupera los TbBook del usuario que no tienen review asociado
                var booksWithoutAcceptation = await context.Set<TbBook>()
                    .Where(b => b.BookStatusId == 1 && b.UserIdRenter == userId)
                    .Include(x => x.ListingRent).ThenInclude(lr => lr.OwnerUser)
                    .Include(x => x.ListingRent.TlListingPhotos)
                    .Include(x => x.ListingRent.ApprovalPolicyType)
                    .Include(x => x.ListingRent.TpProperties)
                    .Include(x => x.ListingRent.OwnerUser)
                    .ToListAsync();

                if (booksWithoutAcceptation != null)
                {
                    foreach (var book in booksWithoutAcceptation)
                    {
                        if (book?.ListingRent?.TpProperties?.Count > 0)
                        {
                            foreach (var prop in book.ListingRent.TpProperties)
                            {
                                prop.TpPropertyAddresses = new List<TpPropertyAddress>
                                {
                                    new TpPropertyAddress
                                    {
                                        Address1 = prop.Address1,
                                        Address2 = prop.Address2,
                                        CityId = prop.CityId,
                                        CountyId = prop.CountyId,
                                        ZipCode = prop.ZipCode,
                                        StateId = prop.StateId,
                                        City = new TCity
                                        {
                                            CityId = prop.CityId??0,
                                            Name = prop.CityName,
                                            CountyId = prop.CountyId??0,
                                            County = new TCounty
                                            {
                                                CountyId = prop.CountyId ?? 0,
                                                Name = prop.CountyName,
                                                StateId = prop.StateId??0,
                                                State = new TState
                                                {
                                                    Name = prop.StateName,
                                                    StateId = prop.StateId ?? 0,
                                                    Country = new TCountry
                                                    {
                                                        Name = prop.CountryName,
                                                        CountryId = prop.CountryId ?? 0
                                                    }
                                                }
                                            }
                                        }
                                    }
                                };
                            }
                        }
                    }
                }
                //booksWithoutAcceptation = booksWithoutAcceptation.OrderByDescending(x => x.InitDate).ToList();
                return booksWithoutAcceptation;
            }
        }


        public async Task<List<TbBook>> GetApprovedsWOInit(int userId)
        {
            using (var context = new InfraAssertDbContext(dbOptions))
            {
                // Recupera los TbBook del usuario que no tienen review asociado
                var booksWithoutAcceptation = await context.Set<TbBook>()
                    .Where(b => b.BookStatusId == 2 && b.ListingRent.OwnerUserId == userId &&
                        (b.Checkin != null && ((DateTime)b.Checkin).AddHours(+4) > DateTime.UtcNow))
                    .Include(x => x.ListingRent).ThenInclude(lr => lr.OwnerUser)
                    .Include(x => x.ListingRent.TlListingPhotos)
                    .Include(x => x.ListingRent.ApprovalPolicyType)
                    .Include(x => x.ListingRent.TpProperties)
                    .Include(x => x.ListingRent.OwnerUser)
                    .ToListAsync();

                if (booksWithoutAcceptation != null)
                {
                    foreach (var book in booksWithoutAcceptation)
                    {
                        if (book?.ListingRent?.TpProperties?.Count > 0)
                        {
                            foreach (var prop in book.ListingRent.TpProperties)
                            {
                                prop.TpPropertyAddresses = new List<TpPropertyAddress>
                                {
                                    new TpPropertyAddress
                                    {
                                        Address1 = prop.Address1,
                                        Address2 = prop.Address2,
                                        CityId = prop.CityId,
                                        CountyId = prop.CountyId,
                                        ZipCode = prop.ZipCode,
                                        StateId = prop.StateId,
                                        City = new TCity
                                        {
                                            CityId = prop.CityId??0,
                                            Name = prop.CityName,
                                            CountyId = prop.CountyId??0,
                                            County = new TCounty
                                            {
                                                CountyId = prop.CountyId ?? 0,
                                                Name = prop.CountyName,
                                                StateId = prop.StateId??0,
                                                State = new TState
                                                {
                                                    Name = prop.StateName,
                                                    StateId = prop.StateId ?? 0,
                                                    Country = new TCountry
                                                    {
                                                        Name = prop.CountryName,
                                                        CountryId = prop.CountryId ?? 0
                                                    }
                                                }
                                            }
                                        }
                                    }
                                };
                            }
                        }
                    }
                }
                //booksWithoutAcceptation = booksWithoutAcceptation.OrderByDescending(x => x.InitDate).ToList();
                return booksWithoutAcceptation;
            }
        }

        public async Task<List<TbBook>> GetCancelablesBookings(int userId)
        {
            using (var context = new InfraAssertDbContext(dbOptions))
            {
                // Recupera los TbBook del usuario que no tienen review asociado
                var booksWithoutAcceptation = await context.Set<TbBook>()
                    .Where(b => b.BookStatusId == 3 && b.ListingRent.OwnerUserId == userId &&
                    (b.Checkin != null && ((DateTime)b.Checkin).AddHours(+4) > DateTime.UtcNow) && //Devolver las reservas que todavia no pasaron el checkin
                    ((b.CancellationEnd != null && ((DateTime)b.CancellationEnd).AddHours(+4) > DateTime.UtcNow) || b.CancellationEnd == null) &&
                    ((b.CancellationStart != null && ((DateTime)b.CancellationStart).AddHours(+4) < DateTime.UtcNow) || b.CancellationStart == null))
                    .Include(x => x.ListingRent).ThenInclude(lr => lr.OwnerUser)
                    .Include(x => x.ListingRent.TlListingPhotos)
                    .Include(x => x.ListingRent.ApprovalPolicyType)
                    .Include(x => x.ListingRent.TpProperties)
                    .Include(x => x.ListingRent.OwnerUser)
                    .ToListAsync();

                if (booksWithoutAcceptation != null)
                {
                    foreach (var book in booksWithoutAcceptation)
                    {
                        if (book?.ListingRent?.TpProperties?.Count > 0)
                        {
                            foreach (var prop in book.ListingRent.TpProperties)
                            {
                                prop.TpPropertyAddresses = new List<TpPropertyAddress>
                                {
                                    new TpPropertyAddress
                                    {
                                        Address1 = prop.Address1,
                                        Address2 = prop.Address2,
                                        CityId = prop.CityId,
                                        CountyId = prop.CountyId,
                                        ZipCode = prop.ZipCode,
                                        StateId = prop.StateId,
                                        City = new TCity
                                        {
                                            CityId = prop.CityId??0,
                                            Name = prop.CityName,
                                            CountyId = prop.CountyId??0,
                                            County = new TCounty
                                            {
                                                CountyId = prop.CountyId ?? 0,
                                                Name = prop.CountyName,
                                                StateId = prop.StateId??0,
                                                State = new TState
                                                {
                                                    Name = prop.StateName,
                                                    StateId = prop.StateId ?? 0,
                                                    Country = new TCountry
                                                    {
                                                        Name = prop.CountryName,
                                                        CountryId = prop.CountryId ?? 0
                                                    }
                                                }
                                            }
                                        }
                                    }
                                };
                            }
                        }
                    }
                }
                //booksWithoutAcceptation = booksWithoutAcceptation.OrderByDescending(x => x.InitDate).ToList();
                return booksWithoutAcceptation;
            }
        }

        public async Task<TbBook> Cancel(int userId, long bookId)
        {
            var existingBook = await _dbContext.TbBooks.Include(x => x.ListingRent).Where(x => x.BookId == bookId).FirstOrDefaultAsync();
            if (existingBook == null)
                throw new NotFoundException($"No se encontro la reserva con ID {bookId}.");

            if (existingBook.ListingRent.OwnerUserId != userId && existingBook.UserIdRenter != userId)
                throw new UnauthorizedAccessException($"El usuario con ID {userId} no tiene permiso para cancelar esta reserva.");

            List<int> cancellableStatuses = new List<int> { 1, 2, 3 }; // Ejemplo: 1 = Prebook, 2 = Approved

            if (!cancellableStatuses.Contains(existingBook.BookStatusId))
                throw new InvalidOperationException($"La reserva con ID {bookId} no puede ser cancelada en su estado actual.");

            if (existingBook.CancellationEnd != null && existingBook.CancellationEnd?.AddHours(4) > DateTime.UtcNow &&
                (existingBook.CancellationStart == null || (existingBook.CancellationStart?.AddHours(4) < DateTime.UtcNow)))
            {
                existingBook.BookStatusId = 4;
                existingBook.CancellationUserId = userId;
                existingBook.Cancellation = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();

                return existingBook;
            }
            else if (existingBook.BookStatusId == 1)
            {
                existingBook.BookStatusId = 4;
                existingBook.CancellationUserId = userId;
                existingBook.Cancellation = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                return existingBook;
            }

            throw new InvalidOperationException($"La reserva con ID {bookId} no puede ser cancelada por las políticas de cancelación.");

        }


        public async Task<TbBook> AuthorizationResponse(int userId, long bookId, bool isApproval, int? reasonRefused)
        {
            var existingBook = await _dbContext.TbBooks.Include(x => x.ListingRent).Where(x => x.BookId == bookId).FirstOrDefaultAsync();
            if (existingBook == null)
                throw new NotFoundException($"No se encontro la reserva con ID {bookId}.");

            if (existingBook.ListingRent.OwnerUserId != userId)
                throw new UnauthorizedAccessException($"El usuario con ID {userId} no tiene permiso para autorizar/rechazar esta reserva.");

            List<int> cancellableStatuses = new List<int> { 1 }; // Ejemplo: 1 = Prebook

            if (!cancellableStatuses.Contains(existingBook.BookStatusId))
                throw new InvalidOperationException($"La reserva con ID {bookId} no puede ser aprobada/rechazada en su estado actual.");

            if (isApproval)
            {
                existingBook.BookStatusId = 2;
                existingBook.IsApprobal = true;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                existingBook.BookStatusId = 5;
                existingBook.IsApprobal = false;
                existingBook.ReasonRefusedId = reasonRefused;
                await _dbContext.SaveChangesAsync();
            }

            return existingBook;
        }

        public async Task CheckAndExpireReservation(DateTime expirationThreshold)
        {
            using (var dbContext = new InfraAssertDbContext(dbOptions))
            {
                var expiredReservations = await dbContext.TbBooks
                    .Where(r => r.BookStatusId == 1 &&
                               r.RequestDateTime <= expirationThreshold)
                    .Take(100) // Límite por batch para no sobrecargar
                    .ToListAsync();

                if (!expiredReservations.Any())
                {
                    return;
                }

                foreach (var reservation in expiredReservations)
                {
                    reservation.BookStatusId = 7;
                    reservation.ExpiredDateTime = DateTime.UtcNow;
                }

                await dbContext.SaveChangesAsync();
            }
        }

        public async Task CheckAndFinishReservation(DateTime expirationThreshold)
        {
            using (var dbContext = new InfraAssertDbContext(dbOptions))
            {
                var expiredReservations = await dbContext.TbBooks
                    .Where(r => r.BookStatusId == 3 &&
                               r.EndDate <= expirationThreshold)
                    .Take(100) // Límite por batch para no sobrecargar
                    .ToListAsync();

                if (!expiredReservations.Any())
                {
                    return;
                }

                foreach (var reservation in expiredReservations)
                {
                    reservation.BookStatusId = 6;
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
