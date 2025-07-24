using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingPhotoRepository : IListingPhotoRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly ISystemConfigurationRepository _systemConfigurationRepository;
        private readonly DbContextOptions<InfraAssertDbContext> dbOptions;

        public ListingPhotoRepository(InfraAssertDbContext infraAssertDbContext, ISystemConfigurationRepository systemConfigurationRepository,
            IServiceProvider serviceProvider)
        {
            _context = infraAssertDbContext;
            _systemConfigurationRepository = systemConfigurationRepository;
            dbOptions = serviceProvider.GetRequiredService<DbContextOptions<InfraAssertDbContext>>();
        }

        public async Task<ReturnModel> DeleteListingRentImage(long listingRentId, int photoId)
        {
            using (var dbContedt = new InfraAssertDbContext(dbOptions))
            {
                var result = await dbContedt.TlListingPhotos.Where(x => x.ListingPhotoId == photoId).FirstOrDefaultAsync();
                if (result == null)
                {
                    throw new InvalidOperationException($"la imagen no puede ser encontrada.");
                }
                else if (result.ListingRentId != listingRentId)
                {
                    throw new InvalidOperationException($"la imagen no corresponde al listing rent.");
                }
                else
                {
                    dbContedt.TlListingPhotos.Remove(result);
                    await dbContedt.SaveChangesAsync();
                    return new ReturnModel { StatusCode = ResultStatusCode.OK, HasError = false, Data = result.Name };
                }
            }
        }

        public async Task<List<TlListingPhoto>> GetByListingRentId(long listingRentId)
        {
            using (var dbContedt = new InfraAssertDbContext(dbOptions))
            {
                var result = await dbContedt.TlListingPhotos.Where(x => x.ListingRentId == listingRentId)
                .Include(x => x.SpaceType).ToListAsync();
                return result;
            }
        }

        public async Task<List<TlListingPhoto>> GetByListingRentId(long listingRentId, int userID)
        {
            using (var dbContedt = new InfraAssertDbContext(dbOptions))
            {
                var result = await dbContedt.TlListingPhotos.Where(x => x.ListingRentId == listingRentId && x.ListingRent.OwnerUserId == userID)
                .Include(x => x.SpaceType).ToListAsync();
                return result;
            }
        }

        public async Task<ReturnModel> UploadPhoto(long listingRentId, string fileName, string description, int? spaceType, bool isMain)
        {
            using (var dbContedt = new InfraAssertDbContext(dbOptions))
            {
                if (isMain)
                {
                    var result = dbContedt.TlListingPhotos.Where(x => x.ListingRentId == listingRentId && x.IsPrincipal == true).FirstOrDefault();
                    if (result != null)
                    {
                        result.IsPrincipal = false;
                        //dbContedt.SaveChanges();
                    }
                }
                var photo = await dbContedt.TlListingPhotos.Where(x => x.ListingRentId == listingRentId && x.Name == fileName).FirstOrDefaultAsync();
                if (photo != null)
                {
                    throw new InvalidOperationException($"Ya existe la imagen {fileName}.");
                }
                else
                {
                    if (spaceType == 0)
                    {
                        spaceType = null;
                    }
                    var newPhoto = new TlListingPhoto
                    {
                        ListingRentId = listingRentId,
                        Name = fileName,
                        IsPrincipal = isMain,
                        Description = description,
                        SpaceTypeId = spaceType,
                        //PhotoLink = _systemConfigurationRepository.GetListingResourceUrl() + fileName
                        PhotoLink = fileName
                    };
                    dbContedt.TlListingPhotos.Add(newPhoto);
                    dbContedt.SaveChanges();
                    return new ReturnModel { StatusCode = ResultStatusCode.OK, HasError = false, Data = fileName, ResultError = new ErrorCommon { Code = newPhoto.ListingPhotoId.ToString() } };
                }
            }
        }

        public async Task<TlListingPhoto> UpdatePhoto(long listingRentId, ProcessData_PhotoModel photo)
        {
            using (var dbContedt = new InfraAssertDbContext(dbOptions))
            {
                TlListingPhoto photoDb = await dbContedt.TlListingPhotos.Where(x => x.ListingPhotoId == photo.PhotoId).FirstOrDefaultAsync();

                if (photoDb == null)
                {
                    throw new InvalidOperationException($"la imagen no puede ser encontrada.");
                }
                else if (photoDb.ListingRentId != listingRentId)
                {
                    throw new InvalidOperationException($"la imagen no corresponde al listing rent.");
                }
                else
                {
                    photoDb.Description = photo.Description ?? photoDb.Description;
                    photoDb.IsPrincipal = photo.IsPrincipal;
                    if (photo.IsPrincipal ?? false)
                    {
                        var result = dbContedt.TlListingPhotos.Where(x => x.ListingRentId == listingRentId && x.ListingPhotoId != photo.PhotoId && x.IsPrincipal == true).FirstOrDefault();
                        if (result != null)
                        {
                            result.IsPrincipal = false;
                            //await dbContedt.SaveChangesAsync();
                        }
                    }
                    photoDb.SpaceTypeId = photo.SpaceTypeId ?? photoDb.SpaceTypeId;
                    await dbContedt.SaveChangesAsync();
                    return photoDb;
                }
            }
        }
    }
}
