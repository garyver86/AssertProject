using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Azure.Core.GeoJson;
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
                    throw new NotFoundException($"la imagen no puede ser encontrada.");
                else if (result.ListingRentId != listingRentId)
                    throw new NotFoundException($"la imagen no corresponde al listing rent.");

                var remainingPhotos = await dbContedt.TlListingPhotos
                    .Where(x => x.ListingRentId == listingRentId && x.ListingPhotoId != photoId)
                    .OrderBy(x => x.Position)
                    .ToListAsync();

                dbContedt.TlListingPhotos.Remove(result);

                int position = 1;
                foreach (var photo in remainingPhotos)
                {
                    if (photo.IsPrincipal == true && position != 1)
                        photo.IsPrincipal = false;

                    photo.Position = position++;
                }

                if (remainingPhotos.Any() && !remainingPhotos.Any(p => p.IsPrincipal == true))
                {
                    remainingPhotos.First().IsPrincipal = true;
                    remainingPhotos.First().Position = 1;
                }

                await dbContedt.SaveChangesAsync();
                return new ReturnModel { StatusCode = ResultStatusCode.OK, HasError = false, Data = result.Name };
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

        public async Task<ReturnModel> UploadPhoto(long listingRentId, string fileName,
            string description, int? spaceType, bool isMain)
        {
            using (var dbContedt = new InfraAssertDbContext(dbOptions))
            {
                var existingPhotos = await dbContedt.TlListingPhotos
                    .Where(x => x.ListingRentId == listingRentId)
                    .OrderBy(x => x.Position)
                    .ToListAsync();

                if (existingPhotos.Any(x => x.Name == fileName))
                    throw new InvalidOperationException($"Ya existe la imagen {fileName}.");

                if (isMain)
                {
                    var currentMainPhoto = existingPhotos.FirstOrDefault(x => x.IsPrincipal == true);

                    if (currentMainPhoto != null)
                    {
                        currentMainPhoto.IsPrincipal = false;
                        currentMainPhoto.Position = existingPhotos.Count + 1;
                    }
                }

                int newPosition = isMain ? 1 : (existingPhotos.Any() ? existingPhotos.Max(x => x.Position!.Value) + 1 : 1);

                if (spaceType == 0)
                    spaceType = null;
                var newPhoto = new TlListingPhoto
                {
                    ListingRentId = listingRentId,
                    Name = fileName,
                    IsPrincipal = isMain,
                    Description = description,
                    SpaceTypeId = spaceType,
                    Position = newPosition,
                    //PhotoLink = _systemConfigurationRepository.GetListingResourceUrl() + fileName
                    PhotoLink = fileName
                };
                dbContedt.TlListingPhotos.Add(newPhoto);
                dbContedt.SaveChanges();

                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = fileName,
                    ResultError = new ErrorCommon
                    {
                        Code = newPhoto.ListingPhotoId.ToString()
                    }
                };

            }
        }

        public async Task<TlListingPhoto> UpdatePhoto(long listingRentId, ProcessData_PhotoModel photo)
        {
            using (var dbContedt = new InfraAssertDbContext(dbOptions))
            {
                TlListingPhoto photoDb = await dbContedt.TlListingPhotos
                    .Where(x => x.ListingPhotoId == photo.PhotoId).FirstOrDefaultAsync();

                if (photoDb == null)
                    throw new InvalidOperationException($"la imagen no puede ser encontrada.");
                else if (photoDb.ListingRentId != listingRentId)
                    throw new InvalidOperationException($"la imagen no corresponde al listing rent.");

                var allPhotos = await dbContedt.TlListingPhotos
                    .Where(x => x.ListingRentId == listingRentId)
                    .OrderBy(x => x.Position)
                    .ToListAsync();

                photoDb.Description = photo.Description ?? photoDb.Description;
                if (photo.IsPrincipal ?? false)
                {
                    var result = allPhotos
                        .Where(x => x.IsPrincipal == true && x.ListingPhotoId != photoDb.ListingPhotoId)
                        .FirstOrDefault();
                    if (result != null)
                    {
                        result.IsPrincipal = false;
                        result.Position = photoDb.Position;
                    }

                    photoDb.IsPrincipal = true;
                    photoDb.Position = 1;
                }
                photoDb.SpaceTypeId = photo.SpaceTypeId ?? photoDb.SpaceTypeId;
                await dbContedt.SaveChangesAsync();
                return photoDb;

            }
        }

        public async Task<string> UpdatePhotoPosition(long listingRentId, long listingPhotoId, int newPostition)
        {
            using (var dbContent = new InfraAssertDbContext(dbOptions))
            {
                var photo = await dbContent.TlListingPhotos
                    .Where(x => x.ListingRentId == listingRentId && x.ListingPhotoId == listingPhotoId)
                    .FirstOrDefaultAsync();

                if (photo == null)
                    throw new NotFoundException($"la imagen no puede ser encontrada. Verifique e intente nuevamente.");

                if (photo.Position == newPostition)
                    return "UPDATED";
                else
                {
                    var photoOldPosition = await dbContent.TlListingPhotos
                    .Where(x => x.ListingRentId == listingRentId && x.Position == newPostition)
                    .FirstOrDefaultAsync();

                    if (photoOldPosition is not null && photoOldPosition.Position is not null)
                    {
                        photoOldPosition.Position = photo.Position;
                        if (photo.Position == 1)
                            photoOldPosition.IsPrincipal = true;
                        else
                            photoOldPosition.IsPrincipal = false;
                    }

                    photo.Position = newPostition;
                    if (newPostition == 1)
                        photo.IsPrincipal = true;
                    else
                        photo.IsPrincipal = false;

                    await dbContent.SaveChangesAsync();
                    return "UPDATE";
                }
            }
        }

        public async Task<string> SortListingRentPhotos()
        {
            try
            {
                using (var dbContent = new InfraAssertDbContext(dbOptions))
                {
                    var listingRents = await dbContent.TlListingRents
                        .Include(x => x.TlListingPhotos)
                        .Where(x => x.TlListingPhotos.Any())
                        .AsNoTracking()
                        .ToListAsync();

                    var updateTasks = listingRents.Select(listingRent =>
                    {
                        var orderedPhotos = listingRent.TlListingPhotos
                            .OrderByDescending(x => x.IsPrincipal)
                            .ToList();

                        int position = 1;
                        foreach (var photo in orderedPhotos)
                            photo.Position = position++;

                        dbContent.AttachRange(listingRent.TlListingPhotos);
                        foreach (var photo in listingRent.TlListingPhotos)
                            dbContent.Entry(photo).Property(x => x.Position).IsModified = true;

                        return Task.CompletedTask;
                    }).ToList();

                    await Task.WhenAll(updateTasks);
                    await dbContent.SaveChangesAsync();

                    return "UPDATED";
                }
            }
            catch (Exception ex)
            {
                throw new InfrastructureException($"Error al actualizar posiciones de fotos: {ex.Message}");
            }
        }
    }
}
