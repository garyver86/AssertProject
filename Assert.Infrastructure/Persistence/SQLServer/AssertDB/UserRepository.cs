using Assert.Domain.Common.Metadata;
using Assert.Domain.Entities;
using Assert.Domain.Enums;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Models;
using Assert.Domain.Models.Profile;
using Assert.Domain.Models.Review;
using Assert.Domain.Repositories;
using Assert.Domain.Utils;
using Assert.Domain.ValueObjects;
using Assert.Infrastructure.Exceptions;
using Assert.Infrastructure.Utils;
using Assert.Shared.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Net.Http.Headers;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class UserRepository(IExceptionLoggerService _exceptionLoggerService,
        RequestMetadata _metadata,
        InfraAssertDbContext _dbContext,
        ILogger<UserRepository> _logger, IServiceProvider serviceProvider)
        : IUserRepository
    {
        private readonly DbContextOptions<InfraAssertDbContext> dbOptions = serviceProvider.GetRequiredService<DbContextOptions<InfraAssertDbContext>>();

        public async Task<ReturnModel> Login(string username, string password)
        {
            try
            {
                string pass = UtilsMgr.GetHash512(password);

                string query = $"EXECUTE dbo.SPA_Loguin @username, @password, @ip";

                var param = new List<SqlParameter>
                    {
                        new SqlParameter("@username", username),
                        new SqlParameter("@password", pass),
                        new SqlParameter("@ip", _metadata.IpAddress)
                    };

                var result_login = _dbContext.Database.SqlQueryRaw<LoginModelResult>(
                    query, param.ToArray()).ToList().FirstOrDefault();
                if (result_login.CodeResult == 1)
                {
                    return new ReturnModel
                    {
                        StatusCode = ResultStatusCode.OK,
                        //Data = result_login.MessageResult,
                        Data = new ProviderUser
                        {
                            UserId = result_login.Identifier.ToString(),
                            Email = result_login.userName,
                            Name = result_login.givenName,
                            LastName = result_login.lastName
                        },
                        ResultError = new ErrorCommon
                        {
                            Message = result_login.Identifier.ToString(),
                            Code = result_login.Roles
                        }
                    };
                }
                else
                    throw new UnauthorizedException(result_login.MessageResult);
            }
            catch (Exception ex) when (ex is not UnauthorizedException)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { username });

                throw new InfrastructureException(ex.Message);
            }
        }

        public ReturnModel ChangeStatusUser(int userId, int id, string status)
        {
            TuUser? user = _dbContext.TuUsers.Where(x => x.UserId == id).FirstOrDefault();
            if (user != null)
            {
                user.Status = status == "AC" ? "IN" : "AC";

                _dbContext.SaveChanges();
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.OK
                };

            }
            else
            {

                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.NotFound,
                    ResultError = new ErrorCommon
                    {
                        Message = "El registro no ha sido encontrado."
                    }
                };
            }
        }


        public async Task<ReturnModel> BlockAsHost(int userId, int id)
        {
            TuUser? user = _dbContext.TuUsers.Where(x => x.UserId == id).FirstOrDefault();
            if (user != null)
            {
                user.BlockAsHost = true;

                await _dbContext.SaveChangesAsync();
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.OK
                };

            }
            else
            {

                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.NotFound,
                    ResultError = new ErrorCommon
                    {
                        Message = "El registro no ha sido encontrado."
                    }
                };
            }
        }

        public async Task<ReturnModel> UnblockAsHost(int userId, int id)
        {
            TuUser? user = _dbContext.TuUsers.Where(x => x.UserId == id).FirstOrDefault();
            if (user != null)
            {
                user.BlockAsHost = false;

                await _dbContext.SaveChangesAsync();
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.OK
                };

            }
            else
            {

                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.NotFound,
                    ResultError = new ErrorCommon
                    {
                        Message = "El registro no ha sido encontrado."
                    }
                };
            }
        }
        public async Task<ReturnModel<bool>> ExistLocalUser(string userName)
        {
            try
            {
                //var platformObj = await _dbContext.TuPlatforms.Where(x => x.Code.ToLower() == "local").FirstAsync();

                var user = await _dbContext.TuUsers
                    .Where(x => x.UserName!.ToUpper() == userName.ToUpper()
                    && x.Status == "AC")
                    .FirstOrDefaultAsync();

                var existUser = user != null;

                return new ReturnModel<bool>
                {
                    Data = existUser,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { userName });
                throw new InfrastructureException(ex.Message);
            }
        }

        public async Task<ReturnModel> ValidateUserName(string userName,
            bool validateStatusActive, Platform platform)
        {
            try
            {
                var user = await _dbContext.TuUsers
                    .Where(x => x.UserName!.ToUpper() == userName.ToUpper() && x.Status == "AC")
                    .Include(x => x.TuAccounts)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return new ReturnModel
                    {
                        StatusCode = ResultStatusCode.NotFound,
                        ResultError = new ErrorCommon { Message = "El usuario no existe en registros." }
                    };
                }

                string plataformStr = platform.ToString().ToLower();
                var platformObj = await _dbContext.TuPlatforms.Where(x => x.Code.ToLower() == plataformStr).FirstAsync();

                if (user.PlatformId != platformObj.PlatformId)
                {
                    var plaformFrom = await _dbContext.TuPlatforms.Where(x => x.PlatformId == user.PlatformId).FirstAsync();

                    _logger.LogError($"{_metadata.CorrelationId} | User registered in: {platformObj.Name!}. CorrelationID: {_metadata.CorrelationId}", platformObj);
                    throw new InfrastructureException($"El usuario se encuentra registrado en plataforma: {plaformFrom.Code!.ToUpper()}. No puede realizar Login por plataforma: {platform.ToString().ToUpper()}");
                }

                if (!validateStatusActive)
                {
                    return new ReturnModel
                    {
                        Data = user,
                        StatusCode = ResultStatusCode.OK
                    };
                }

                var lastAccount = user.TuAccounts.OrderByDescending(a => a.AccountId).FirstOrDefault();
                if (lastAccount == null)
                {
                    return new ReturnModel
                    {
                        StatusCode = ResultStatusCode.NoContent,
                        ResultError = new ErrorCommon { Message = "No se encontró cuenta asociada al usuario." },
                        Data = user.UserId
                    };
                }

                if (lastAccount.Status != "AC")
                    throw new UnauthorizedException("El usuario no se encuentra activo.");

                if (lastAccount.IsBlocked == true)
                    throw new UnauthorizedException("El usuario se encuentra bloqueado.");

                return new ReturnModel
                {
                    Data = user,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex) when (!(ex is UnauthorizedException
                                      || ex is InfrastructureException))
            {
                _logger.LogError($"{_metadata.CorrelationId} | Exception while validate user: {userName}", new { userName });
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { userName });
                throw new InfrastructureException(ex.Message);
            }
        }

        public async Task<ReturnModel<TuUser>> Get(int id)
        {
            TuUser user = _dbContext.TuUsers.Where(x => x.UserId == id).ToList().LastOrDefault();
            if (user != null)
            {
                return await Task.Run(() => new ReturnModel<TuUser>
                {
                    Data = user,
                    StatusCode = ResultStatusCode.OK
                });
            }
            else
            {
                return new ReturnModel<TuUser>
                {
                    StatusCode = ResultStatusCode.NotFound,
                    ResultError = new ErrorCommon { Message = "El usuario no fue encontrado." }
                };
            }
        }

        public async Task<List<TuUser>> GetAll()
        {
            List<TuUser> users;

            users = _dbContext.TuUsers.ToList();
            return users;
        }

        public async Task<TuUser> GetById(int id)
        {
            TuUser? user = _dbContext.TuUsers.FirstOrDefault(item => item.UserId == id);
            return user;
        }

        public async Task<int> Create(string userName, Platform platform,
            string name, string lastName, int genderTypeId,
            DateTime? dateOfBirth, string photoLink,
            int accountTypeId, string socialId, int? timeZoneId, string phoneNumber = "")
        {
            string plataformStr = platform.ToString().ToLower();
            var platformId = await _dbContext.TuPlatforms.Where(x => x.Code.ToLower() == plataformStr)
                .Select(x => x.PlatformId).FirstOrDefaultAsync();

            if (genderTypeId == 0)
                genderTypeId = await _dbContext.TuGenderTypes.Where(x => x.Code == "nr")
                    .Select(x => x.GenderTypeId).FirstOrDefaultAsync();

            var currentDate = DateTime.UtcNow;

            var newUser = new TuUser
            {
                UserName = userName,
                PlatformId = platformId,
                Name = name,
                LastName = lastName,
                GenderTypeId = genderTypeId,
                DateOfBirth = dateOfBirth.HasValue ? DateOnly.FromDateTime(dateOfBirth.Value)
                    : null,
                TitleTypeId = null,
                UserStatusTypeId = 1,
                RegisterDate = currentDate,
                UpdateDate = currentDate,
                PhotoLink = photoLink,
                Status = "AC",
                AccountType = accountTypeId,
                SocialId = socialId,
                TimeZoneId = timeZoneId
            };

            try
            {
                await _dbContext.TuUsers.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();

                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    var phoneNumberObject = phoneNumber.SplitCountryCode();
                    await _dbContext.TuPhones.AddAsync(new TuPhone
                    {
                        UserId = newUser.UserId,
                        CountryCode = phoneNumberObject.CountryCode,
                        AreaCode = "",
                        Number = phoneNumberObject.PhoneNumber,
                        IsPrimary = true,
                        IsMobile = true,
                        Status = 1
                    });
                    await _dbContext.SaveChangesAsync();
                }

                return newUser.UserId;
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { newUser });
                throw new InfrastructureException(ex.Message);
            }
        }

        public async Task<ReturnModel<(List<TlListingRent>, PaginationMetadata)>> SearchHostAsync(SearchFilters filters, int pageNumber, int pageSize, long userId)
        {
            throw new NotImplementedException("This method is not implemented yet.");
            //using (var context = new InfraAssertDbContext(dbOptions))
            //{
            //    var skipAmount = (pageNumber - 1) * pageSize;
            //    var query = context.TuUsers.AsQueryable();

            //    query = query.Where(x => x.TuUserRoles.Any(y => y.UserTypeId == 2));

            //    if (filters.CityId > 0)
            //    {
            //        query = query.Where(p => p.TpProperties.FirstOrDefault().CityId == filters.CityId);
            //    }

            //    if (filters.CountyId > 0)
            //    {
            //        query = query.Where(p => p.TpProperties.FirstOrDefault().CountyId == filters.CountyId);
            //    }

            //    if (filters.StateId > 0)
            //    {
            //        query = query.Where(p => p.TpProperties.FirstOrDefault().StateId == filters.StateId);
            //    }

            //    if (filters.CountryId > 0)
            //    {
            //        query = query.Where(p => p.TpProperties.FirstOrDefault().CountryId == filters.CountryId);
            //    }

            //    if (filters.MinPrice.HasValue && !filters.MaxPrice.HasValue)
            //    {
            //        query = query.Where(p => p.TlListingPrices.FirstOrDefault().PriceNightly >= filters.MinPrice.Value);
            //    }

            //    if (filters.MaxPrice.HasValue && !filters.MinPrice.HasValue)
            //    {
            //        query = query.Where(p => p.TlListingPrices.FirstOrDefault().PriceNightly <= filters.MaxPrice.Value);
            //    }

            //    if (filters.MaxPrice.HasValue && filters.MinPrice.HasValue)
            //    {
            //        query = query.Where(p => filters.MinPrice.Value <= p.TlListingPrices.FirstOrDefault().PriceNightly && p.TlListingPrices.FirstOrDefault().PriceNightly <= filters.MaxPrice.Value);
            //    }

            //    if (filters.Guests.HasValue)
            //    {
            //        query = query.Where(p => p.MaxGuests >= filters.Guests.Value);
            //    }

            //    if (filters.Bedrooms.HasValue)
            //    {
            //        query = query.Where(p => p.Bedrooms >= filters.Bedrooms.Value);
            //    }
            //    if (filters.Bathrooms.HasValue)
            //    {
            //        query = query.Where(p => p.Bathrooms >= filters.Bathrooms.Value);
            //    }

            //    if (filters.Beds.HasValue)
            //    {
            //        query = query.Where(p => p.Beds >= filters.Beds.Value);
            //    }

            //    if (filters.PropertyTypeId > 0)
            //    {
            //        query = query.Where(p => p.TpProperties.FirstOrDefault().PropertySubtypeId == filters.PropertyTypeId);
            //    }

            //    if (filters.AmenityIds != null && filters.AmenityIds.Any())
            //    {
            //        query = query.Where(listing => filters.AmenityIds.All(amenityId =>
            //           listing.TlListingAmenities.Any(listingAmenity => listingAmenity.AmenitiesTypeId == amenityId)));

            //    }

            //    if (filters.CheckInDate.HasValue && filters.CheckOutDate.HasValue)
            //    {
            //        query = query.Where(p => !p.TbBooks.Any(b =>
            //            (b.StartDate <= filters.CheckOutDate.Value && b.EndDate >= filters.CheckInDate.Value)));
            //    }

            //    if (filters?.Rules?.AllowedPets != null)
            //    {
            //        query = query.Where(p => p.TlListingRentRules.Any(b => b.RuleTypeId == 1 &&
            //          (filters.Rules.AllowedPets == b.Value)));
            //    }

            //    List<double> boundingBox = null;
            //    if (filters.Latitude.HasValue && filters.Longitude.HasValue && filters.Radius.HasValue)
            //    {
            //        boundingBox = GeoUtils.CalculateBoundingBox(filters.Latitude.Value, filters.Longitude.Value, filters.Radius ?? 0);

            //        var minLat = boundingBox[0];
            //        var maxLat = boundingBox[1];
            //        var minLon = boundingBox[2];
            //        var maxLon = boundingBox[3];

            //        var referenceLat = filters.Latitude.Value;
            //        var referenceLon = filters.Longitude.Value;
            //        var radius = filters.Radius.Value;

            //        query = query.Where(p =>
            //            p.TpProperties.FirstOrDefault().Latitude >= minLat && p.TpProperties.FirstOrDefault().Latitude <= maxLat &&
            //            p.TpProperties.FirstOrDefault().Longitude >= minLon && p.TpProperties.FirstOrDefault().Longitude <= maxLon);
            //    }

            //    query = query.Include(x => x.ListingStatus)
            //            .Include(x => x.ListingStatus)
            //            .Include(x => x.OwnerUser)
            //            .Include(x => x.TlListingAmenities)
            //                .ThenInclude(y => y.AmenitiesType)
            //            .Include(x => x.TlListingFeaturedAspects)
            //                .ThenInclude(y => y.FeaturesAspectType)
            //            .Include(x => x.TpProperties)
            //                //    .ThenInclude(y => y.TpPropertyAddresses)
            //                //    .ThenInclude(y => y.City)
            //                //    .ThenInclude(y => y.County)
            //                //    .ThenInclude(y => y.State)
            //                //    .ThenInclude(y => y.Country)
            //                //.Include(x => x.TpProperties)
            //                //    .ThenInclude(y => y.TpPropertyAddresses)
            //                //    .ThenInclude(y => y.County)
            //                //    .ThenInclude(y => y.State)
            //                //    .ThenInclude(y => y.Country)
            //                //.Include(x => x.TpProperties)
            //                //    .ThenInclude(y => y.TpPropertyAddresses)
            //                //    .ThenInclude(y => y.State)
            //                //    .ThenInclude(y => y.Country)
            //                //.Include(x => x.TpProperties)
            //                .ThenInclude(y => y.PropertySubtype)
            //                    .ThenInclude(y => y.PropertyType)
            //            .Include(x => x.TlListingPhotos)
            //            .Include(x => x.TlListingPrices)
            //            .Include(x => x.TlListingRentRules)
            //            .AsNoTracking();

            //    var properties = await query
            //       .Skip(skipAmount)
            //       .Take(pageSize)
            //       .ToListAsync();

            //    List<TlListingRent> result = new List<TlListingRent>();
            //    if (boundingBox != null)
            //    {
            //        foreach (var prop in properties)
            //        {
            //            bool cumple = false;
            //            foreach (var p in prop.TpProperties)
            //            {
            //                double distance = GeoUtils.CalculateDistanceMeters(filters.Latitude ?? 0, filters.Longitude ?? 0, p.Latitude ?? 0, p.Longitude ?? 0);
            //                if (distance <= filters.Radius)
            //                {
            //                    p.DistanceMeters = distance;
            //                    cumple = true;
            //                }
            //            }
            //            if (cumple)
            //            {
            //                result.Add(prop);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        result = properties;
            //    }

            //    if (result != null)
            //    {
            //        foreach (var listing in result)
            //        {
            //            listing.isFavorite = favoritesList.Contains(listing.ListingRentId);
            //            foreach (var prop in listing.TpProperties)
            //            {
            //                prop.TpPropertyAddresses = new List<TpPropertyAddress>
            //            {
            //                new TpPropertyAddress
            //                {
            //                    Address1 = prop.Address1,
            //                    Address2 = prop.Address2,
            //                    CityId = prop.CityId,
            //                    CountyId = prop.CountyId,
            //                    ZipCode = prop.ZipCode,
            //                    StateId = prop.StateId,
            //                    City = new TCity
            //                    {
            //                        CityId = prop.CityId??0,
            //                        Name = prop.CityName,
            //                        CountyId = prop.CountyId??0,
            //                        County = new TCounty
            //                        {
            //                            CountyId = prop.CountyId ?? 0,
            //                            Name = prop.CountyName,
            //                            StateId = prop.StateId??0,
            //                            State = new TState
            //                            {
            //                                Name = prop.StateName,
            //                                StateId = prop.StateId ?? 0,
            //                                Country = new TCountry
            //                                {
            //                                    Name = prop.CountryName,
            //                                    CountryId = prop.CountryId ?? 0
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            };
            //            }
            //        }
            //    }
            //    PaginationMetadata pagination = new PaginationMetadata
            //    {
            //        CurrentPage = pageNumber,
            //        PageSize = pageSize,
            //        TotalItemCount = await query.CountAsync(),
            //        TotalPageCount = (int)Math.Ceiling((double)await query.CountAsync() / pageSize)
            //    };

            //    return new ReturnModel<(List<TlListingRent>, PaginationMetadata)>
            //    {
            //        StatusCode = ResultStatusCode.OK,
            //        Data = (result, pagination)
            //    };
            //}
        }

        public async Task EnableHostRol(int userId)
        {
            try
            {
                using (var context = new InfraAssertDbContext(dbOptions))
                {
                    TuUserRole userRole = await context.TuUserRoles
                        .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.UserTypeId == 2);
                    if (userRole == null)
                    {
                        context.TuUserRoles.Add(new TuUserRole
                        {
                            UserId = userId,
                            UserTypeId = 2, // Host role
                            IsActive = true
                        });
                        await context.SaveChangesAsync();
                    }

                }

            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { userId });
                throw new InfrastructureException(ex.Message);
            }
        }

        public async Task<string> UpdatePersonalInformation(int userId,
            string name, string lastName, string favoriteName,
            string email, string phone)
        {
            try
            {
                TuUser? user = await _dbContext.TuUsers
                    .Include(u => u.TuEmails)
                    .Include(u => u.TuPhones)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                    throw new NotFoundException("El usuario no fue encontrado. No es posible actualizar informacion.");

                if (!string.IsNullOrEmpty(name)) user.Name = name;
                if (!string.IsNullOrEmpty(lastName)) user.LastName = lastName;
                if (!string.IsNullOrEmpty(favoriteName)) user.FavoriteName = favoriteName;
                if (!string.IsNullOrEmpty(email))
                {
                    var userEmail = user.TuEmails.FirstOrDefault();
                    if (userEmail != null)
                    {
                        userEmail.Email = email;
                        userEmail.IsPrincipal = true;
                        userEmail.IsRecover = true;
                        userEmail.Description = "";
                    }
                    else
                    {
                        user.TuEmails.Add(new TuEmail
                        {
                            UserId = user.UserId,
                            Email = email,
                            IsPrincipal = true,
                            IsRecover = true,
                            Description = ""
                        });
                    }
                }
                if (!string.IsNullOrEmpty(phone))
                {
                    var phoneParts = phone.SplitCountryCode();
                    var userPhone = user.TuPhones.FirstOrDefault();
                    if (userPhone != null)
                    {
                        userPhone.CountryCode = phoneParts.CountryCode.Replace("+", "");
                        userPhone.AreaCode = "";
                        userPhone.Number = phoneParts.PhoneNumber;
                        userPhone.IsPrimary = true;
                        userPhone.IsMobile = true;
                        userPhone.Status = 1;
                    }
                    else
                    {
                        user.TuPhones.Add(new TuPhone
                        {
                            UserId = user.UserId,
                            CountryCode = phoneParts.CountryCode.Replace("+", ""),
                            AreaCode = "",
                            Number = phoneParts.PhoneNumber,
                            IsPrimary = true,
                            IsMobile = true,
                            Status = 1
                        });
                    }
                }

                await _dbContext.SaveChangesAsync();

                return "SUCCESS";
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { userId, name, lastName, favoriteName, email, phone });
                throw new InfrastructureException(ex.Message);
            }
        }

        public async Task<TuUser> GetPersonalInformationById(int userId)
        {
            var user = await _dbContext.TuUsers
                    .Where(x => x.UserId == userId)
                    .Include(x => x.TuEmails)
                    .Include(x => x.TuPhones)
                    .Include(x => x.TuEmergencyContacts)
                    .FirstOrDefaultAsync();

            return user ??
                throw new NotFoundException("El usuario no fue encontrado. No es posible obtener informacion personal.");
        }

        public async Task<int> Update(TuUser user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<ReturnModel<List<TuUserRole>>> GetRoles(int userId, bool getOnlyActives)
        {
            List<TuUserRole> roles = null;
            List<int> roles_enableds = null;
            roles_enableds = _dbContext.TuUserRoles.Where(x => x.UserId == userId && (!getOnlyActives || (x.IsActive ?? false))).Select(x => x.UserRoleId).ToList();
            if (roles_enableds?.Count > 0)
            {
                roles = _dbContext.TuUserRoles.Where(x => roles_enableds.Contains(x.UserRoleId) && (!getOnlyActives || (x.IsActive ?? false))).ToList();
            }
            else
            {
                return new ReturnModel<List<TuUserRole>> { StatusCode = ResultStatusCode.NotFound };
            }
            if (roles?.Count > 0)
            {
                return new ReturnModel<List<TuUserRole>> { Data = roles, StatusCode = ResultStatusCode.OK };
            }
            else
            {
                return new ReturnModel<List<TuUserRole>> { StatusCode = ResultStatusCode.NotFound };
            }
        }

        #region profile & reviews
        public async Task<Profile> GetAllProfile()
        {
            try
            {
                var userWithData = await _dbContext.TuUsers
                    .AsNoTracking()
                    .Where(u => u.UserId == _metadata.UserId)
                    .Select(u => new
                    {
                        User = u,
                        Roles = u.TuUserRoles
                            .Select(ur => ur.UserType != null ? ur.UserType.Name : null)
                            .Where(name => name != null)
                            .ToList(),

                        GuestBooksCount = u.TbBooks
                            .Count(b => b.UserIdRenter == u.UserId &&
                                      b.BookStatus != null &&
                                      (b.BookStatus.Code == "approved" || b.BookStatus.Code == "completed")),

                        HostRentalCount = u.TlListingRents
                            .SelectMany(l => l.TbBooks)
                            .Count(b => b.BookStatus != null &&
                                      (b.BookStatus.Code == "approved" || b.BookStatus.Code == "completed")),

                        HostReviewCalification = u.TlListingRents
                            .SelectMany(l => l.TlListingReviews)
                            .Where(r => r.Calification != null)
                            .Average(r => (double?)r.Calification) ?? 0,

                        GuestReviewCalification = u.TuUserReviewUsers
                            .Where(r => r.Calification != null)
                            .Average(r => (double?)r.Calification) ?? 0,

                        HasGuestReviews = u.TuUserReviewUsers.Any(),
                        HasHostReviews = u.TlListingRents.Any(l => l.TlListingReviews.Any())
                    })
                    .FirstOrDefaultAsync();

                if (userWithData is null)
                {
                    _logger.LogError($"{_metadata.CorrelationId} | There is not user with ID: {_metadata.User} and name: {_metadata.User}");
                    throw new NotFoundException($"No existe usuario con ID: {_metadata.UserId}");
                }

                var guestReviews = userWithData.HasGuestReviews ?
                    await _dbContext.TuUserReviews
                        .Where(r => r.UserId == _metadata.UserId)
                        .OrderByDescending(r => r.DateTimeReview)
                        .Take(2)
                        .ToListAsync() : new List<TuUserReview>();

                var hostReviews = userWithData.HasHostReviews ?
                    await _dbContext.TlListingReviews
                        .Where(r => r.ListingRent.OwnerUserId == _metadata.UserId)
                        .OrderByDescending(r => r.DateTimeReview)
                        .Take(2)
                        .ToListAsync() : new List<TlListingReview>();

                var profile = new Profile
                {
                    UserId = userWithData.User.UserId,
                    Name = userWithData.User.Name ?? string.Empty,
                    LastName = userWithData.User.LastName ?? string.Empty,
                    FavoriteName = userWithData.User.FavoriteName ?? string.Empty,
                    Roles = userWithData.Roles ?? new List<string>(),
                    GuestHostingsTotal = userWithData.GuestBooksCount,
                    HostHostingsTotal = userWithData.HostRentalCount,
                    HostReviewCalification = userWithData.HostReviewCalification,
                    GuestReviewCalification = userWithData.GuestReviewCalification,
                    YearsInAssert = (DateTime.UtcNow - userWithData.User.RegisterDate).Value.Days / 365,
                    TimeInAssert = FormatTimeInCompany(userWithData.User.RegisterDate),
                    Avatar = userWithData.User.PhotoLink ?? string.Empty,
                    CountReviewsGuest = userWithData.User.TuUserReviewUsers.Count(),
                    CountReviewsHost = userWithData.User.TlListingRents.SelectMany(l => l.TlListingReviews).Count(),
                    GuestReviews = guestReviews
                        .Select(r => new CommonReview
                        {
                            ReviewId = r.UserReviewId,
                            ListingRentId = r.ListingRentId ?? 0,
                            BookId = r.BookId ?? 0,
                            UserIdReviewer = r.UserIdReviewer,
                            DateTimeReview = r.DateTimeReview ?? DateTime.Now,
                            ReviewerName = FormatReviewerName(r.UserIdReviewerNavigation),
                            ReviewerLocation = string.Empty,
                            ReviewDateName = r.DateTimeReview?.ToString("dd/MM/yyyy") ?? string.Empty,
                            StayDuration = (r.Book?.EndDate - r.Book?.StartDate)?.Days ?? 0,
                            Rating = r.Calification,
                            ReviewText = r.Comment ?? string.Empty,
                            Avatar = r.UserIdReviewerNavigation.PhotoLink ?? string.Empty
                        }).ToList(),
                    HostReviews = hostReviews
                        .Select(r => new CommonReview
                        {
                            ReviewId = r.ListingReviewId,
                            ListingRentId = r.ListingReviewId,
                            BookId = r.BookId ?? null,
                            UserIdReviewer = _metadata.UserId,
                            DateTimeReview = r.DateTimeReview ?? DateTime.Now,
                            ReviewerName = FormatReviewerName(r.User),
                            ReviewerLocation = string.Empty,
                            ReviewDateName = r.DateTimeReview?.ToString("dd/MM/yyyy") ?? string.Empty,
                            StayDuration = r.BookId is null ? 0 : (r.Book?.EndDate - r.Book?.StartDate)?.Days ?? 0,
                            Rating = r.Calification,
                            ReviewText = r.Comment ?? string.Empty,
                            Avatar = r.User?.PhotoLink ?? string.Empty
                        }).ToList(),
                };

                return profile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{_metadata.CorrelationId} | Exception when get all profile from user login: {_metadata.User}");
                throw new InfrastructureException(ex.Message);
            }
        }

        public async Task<TuUser> GetAdditionalProfile()
        {
            var additionalProfile = await _dbContext.TuUsers
                .Include(u => u.TuAdditionalProfiles)
                    .ThenInclude(us => us.TuAdditionalProfileLiveAts)
                        .ThenInclude(s => s.State)
                            .ThenInclude(c => c.Country)
                .Include(u => u.TuAdditionalProfiles)
                    .ThenInclude(ap => ap.TuAdditionalProfileLanguages)
                        .ThenInclude(ad => ad.Language)
                .FirstOrDefaultAsync(ap => ap.UserId == _metadata.UserId);

            if (additionalProfile == null)
            {
                _logger.LogError($"{_metadata.CorrelationId} | There is not additional profile for user with ID: {_metadata.UserId} and name: {_metadata.User}");
                throw new NotFoundException($"No existe informacion de perfil adicional para usuario con ID: {_metadata.UserId}");
            }

            return additionalProfile;
        }

        public async Task<int> UpsertAdditionalProfile(int objectId, string whatIDo,
            string wantedToGo, string pets, DateTime? birthday, List<TLanguage>? languages,
            string introduceYourself, int cityId, string cityName, string location)
        {
            TuAdditionalProfile additionalProfile;

            if (objectId > 0) // Update
            {
                additionalProfile = await _dbContext.TuAdditionalProfiles
                    .Include(x => x.TuAdditionalProfileLanguages)
                    .Include(l => l.TuAdditionalProfileLiveAts)
                    .FirstOrDefaultAsync(ap => ap.AdditionalProfileId == objectId);

                if (additionalProfile == null)
                {
                    _logger.LogError($"{_metadata.CorrelationId} | No additional profile found for user ID: {_metadata.UserId}");
                    throw new NotFoundException($"No existe información de perfil adicional para el usuario con ID: {_metadata.UserId}");
                }
            }
            else // Create
            {
                additionalProfile = new TuAdditionalProfile { UserId = _metadata.UserId };
                await _dbContext.TuAdditionalProfiles.AddAsync(additionalProfile);
            }

            if (!string.IsNullOrWhiteSpace(whatIDo)) additionalProfile.WhatIdo = whatIDo;
            if (!string.IsNullOrWhiteSpace(wantedToGo)) additionalProfile.WantedToGo = wantedToGo;
            if (!string.IsNullOrWhiteSpace(pets)) additionalProfile.Pets = pets;
            if (!string.IsNullOrWhiteSpace(introduceYourself)) additionalProfile.Additional = introduceYourself;

            if (birthday is not null && birthday.Value != DateTime.MinValue)
            {
                await _dbContext.Entry(additionalProfile)
                    .Reference(ap => ap.User)
                    .LoadAsync();
                additionalProfile.User!.DateOfBirth = DateOnly.FromDateTime(birthday.Value);
            }

            if (languages is { Count: > 0 })
            {
                additionalProfile.TuAdditionalProfileLanguages ??= new List<TuAdditionalProfileLanguage>();
                additionalProfile.TuAdditionalProfileLanguages.Clear();

                foreach (var lang in languages)
                {
                    additionalProfile.TuAdditionalProfileLanguages.Add(new TuAdditionalProfileLanguage
                    {
                        LanguageId = lang.LanguageId,
                        AdditionalProfileId = additionalProfile.AdditionalProfileId,
                        UserId = _metadata.UserId
                    });
                }
            }

            var liveAt = additionalProfile.TuAdditionalProfileLiveAts.FirstOrDefault();
            if (liveAt != null) // Update
            {
                if (cityId != 0) liveAt.StateId = cityId;
                if (!string.IsNullOrWhiteSpace(cityName)) liveAt.CityName = cityName;
                if (!string.IsNullOrWhiteSpace(location)) liveAt.Location = location;
            }
            else // Create
            {
                if (cityId != 0)
                {
                    additionalProfile.TuAdditionalProfileLiveAts.Add(new TuAdditionalProfileLiveAt
                    {
                        StateId = cityId,
                        CityName = cityName ?? "",
                        Location = location ?? ""
                    });
                }
            }

            await _dbContext.SaveChangesAsync();
            return additionalProfile.AdditionalProfileId;
        }

        public async Task<string> UpdateProfilePhoto(string photoLink)
        {
            try
            {
                var user = await _dbContext.TuUsers
                    .FirstOrDefaultAsync(u => u.UserId == _metadata.UserId);

                if (user == null)
                    throw new NotFoundException($"No existe usuario con ID: {_metadata.UserId}");

                user.PhotoLink = photoLink;
                user.UpdateDate = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();

                return "SUCCESS";
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { photoLink });
                throw new InfrastructureException(ex.Message);
            }
        }
        #endregion

        #region private funcs
        private static string FormatReviewerName(TuUser? user)
            => user is null ? "" : $"{user.Name} {user.LastName}".Trim();

        private static string FormatReviewDate(DateTime? date)
            => date?.ToString("dd/MM/yyyy") ?? string.Empty;

        private static string FormatTimeInCompany(DateTime? registerDate)
        {
            if (!registerDate.HasValue)
                return "Nuevo miembro";

            var timeSpan = DateTime.UtcNow - registerDate.Value;

            int years = timeSpan.Days / 365;
            int months = (timeSpan.Days % 365) / 30;
            int days = timeSpan.Days % 30;
            int totalMonths = (int)(timeSpan.TotalDays / 30);

            if (years > 0)
            {
                return months > 0
                    ? $"{years} año{(years > 1 ? "s" : "")} y {months} mes{(months > 1 ? "es" : "")}"
                    : $"{years} año{(years > 1 ? "s" : "")}";
            }
            else if (totalMonths > 0)
            {
                return $"{totalMonths} mes{(totalMonths > 1 ? "es" : "")}";
            }
            else
            {
                return days > 1
                    ? $"{days} días"
                    : "1 día";
            }
        }
        #endregion
    }
}
