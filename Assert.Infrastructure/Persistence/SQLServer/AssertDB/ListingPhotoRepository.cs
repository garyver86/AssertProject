using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingPhotoRepository : IListingPhotoRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly ISystemConfigurationRepository _systemConfigurationRepository;


        public ListingPhotoRepository(InfraAssertDbContext infraAssertDbContext, ISystemConfigurationRepository systemConfigurationRepository)
        {
            _context = infraAssertDbContext;
            _systemConfigurationRepository = systemConfigurationRepository;
        }

        public async Task<ReturnModel> DeleteListingRentImage(long listingRentId, int photoId)
        {
            var result = await _context.TlListingPhotos.Where(x => x.ListingPhotoId == photoId).FirstOrDefaultAsync();
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
                _context.TlListingPhotos.Remove(result);
                await _context.SaveChangesAsync();
                return new ReturnModel { StatusCode = ResultStatusCode.OK, HasError = false };
            }
        }

        public async Task<List<TlListingPhoto>> GetByListingRentId(long listingRentId)
        {
            var result = await _context.TlListingPhotos.Where(x => x.ListingRentId == listingRentId)
                .Include(x => x.SpaceType).ToListAsync();
            return result;
        }

        public async Task<List<TlListingPhoto>> GetByListingRentId(long listingRentId, int userID)
        {
            var result = await _context.TlListingPhotos.Where(x => x.ListingRentId == listingRentId && x.ListingRent.OwnerUserId == userID)
                .Include(x => x.SpaceType).ToListAsync(); ;
            return result;
        }

        public async Task<ReturnModel> UploadPhoto(long listingRentId, string fileName, string description, int? spaceType, bool isMain)
        {
            if (isMain)
            {
                var result = _context.TlListingPhotos.Where(x => x.ListingRentId == listingRentId && x.IsPrincipal == true).FirstOrDefault();
                if (result != null)
                {
                    result.IsPrincipal = false;
                    _context.SaveChanges();
                }
            }
            var photo = await _context.TlListingPhotos.Where(x => x.ListingRentId == listingRentId && x.Name == fileName).FirstOrDefaultAsync();
            if (photo != null)
            {
                throw new InvalidOperationException($"Ya existe la imagen {fileName}.");
            }
            else
            {
                var newPhoto = new TlListingPhoto
                {
                    ListingRentId = listingRentId,
                    Name = fileName,
                    IsPrincipal = isMain,
                    Description = description,
                    SpaceTypeId = spaceType,
                    PhotoLink = _systemConfigurationRepository.GetListingResourcePath() + fileName
                };
                _context.TlListingPhotos.Add(newPhoto);
                await _context.SaveChangesAsync();
                return new ReturnModel { StatusCode = ResultStatusCode.OK, HasError = false };
            }
        }

        public async Task<TlListingPhoto> UpdatePhoto(long listingRentId, ProcessData_PhotoModel photo)
        {
            TlListingPhoto photoDb = await _context.TlListingPhotos.Where(x => x.ListingPhotoId == photo.PhotoId).FirstOrDefaultAsync();

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
                    var result = _context.TlListingPhotos.Where(x => x.ListingRentId == listingRentId && x.IsPrincipal == true).FirstOrDefault();
                    if (result != null)
                    {
                        result.IsPrincipal = false;
                        await _context.SaveChangesAsync();
                    }
                }
                photoDb.SpaceTypeId = photo.SpaceTypeId ?? photoDb.SpaceTypeId;
                await _context.SaveChangesAsync();
                return photoDb;
            }
        }
    }
}
