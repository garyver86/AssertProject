using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingPhotoRepository : IListingPhotoRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly IImageService _imageService;
        public ListingPhotoRepository(InfraAssertDbContext infraAssertDbContext, IImageService imageService)
        {
            _context = infraAssertDbContext;
            _imageService = imageService;
        }

        public async Task<List<TlListingPhoto>> GetByListingRentId(long listingRentId)
        {
            var result = await _context.TlListingPhotos.Where(x => x.ListingRentId == listingRentId).ToListAsync();
            return result;
        }

        public async Task<List<TlListingPhoto>> GetByListingRentId(long listingRentId, int userID)
        {
            var result = await _context.TlListingPhotos.Where(x => x.ListingRentId == listingRentId && x.ListingRent.OwnerUserId == userID).ToListAsync();
            return result;
        }

        public async Task UpdatePhotos(long listingRentId, List<ProcessData_PhotoModel> photos)
        {
            List<TlListingPhoto> actualList = await _context.TlListingPhotos.Where(x => x.ListingRentId == listingRentId).ToListAsync();
            List<string> alreadyList = [];
            List<string> removeds = [];
            if (photos == null)
            {
                photos = [];
            }
            foreach (var photo in actualList)
            {
                var db_photo = photos.Where(x => x.FileName == photo.Name).FirstOrDefault();
                if (db_photo == null) //Si es que no existe en la lista actualizada, se elimina la foto
                {
                    _context.TlListingPhotos.Remove(photo);
                    removeds.Add(photo.Name);
                }
                else
                {
                    if (!db_photo.Title.IsNullOrEmpty() && db_photo.Title != photo.Description)
                    {
                        photo.Description = db_photo.Title;
                    }
                    alreadyList.Add(photo.Name);
                }
            }
            foreach (var photo in photos)
            {
                if (!alreadyList.Where(x => x == photo.FileName).Any())
                {
                    bool existFile = await _imageService.VerifyListingRentImage(photo.FileName);
                    if (existFile)
                    {
                        TlListingPhoto newPhoto = new TlListingPhoto
                        {
                            Description = photo.Title,
                            ListingRentId = listingRentId,
                            Name = photo.FileName
                        };
                        _context.TlListingPhotos.Add(newPhoto);
                    }
                }
            }

            await _context.SaveChangesAsync();
            if (removeds.Count > 0)
            {
                foreach (var fileName in removeds)
                {
                    await _imageService.RemoveListingRentImage(fileName);
                }
            }
        }
    }
}
