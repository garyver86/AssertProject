﻿using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace Assert.Infrastructure.InternalServices
{
    public class ImageService : IImageService
    {

        private readonly long _maxFileSize = 20 * 1024 * 1024; // 20MB
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private readonly int[] _allowedResolutions = { 1920, 2560, 3840 }; // Anchos permitidos (Confirmar si se validarán)
        private readonly ISystemConfigurationRepository _SystemConfigurationRepository;
        private readonly IErrorHandler _errorHandler;

        public ImageService(ISystemConfigurationRepository systemConfigurationRepository, IErrorHandler errorHandler)
        {
            _SystemConfigurationRepository = systemConfigurationRepository;
            _errorHandler = errorHandler;
        }

        private void EnsureDirectoryExists(string _basePath)
        {
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public async Task<List<ReturnModel>> SaveListingRentImages(IEnumerable<IFormFile> imageFiles, bool useTechnicalMessages)
        {
            List<ReturnModel> savedFiles = new List<ReturnModel>();

            string _basePath = await _SystemConfigurationRepository.GetListingResourcePath();
            EnsureDirectoryExists(_basePath);
            foreach (var imageFile in imageFiles)
            {
                try
                {
                    var fileName = await SaveSingleImageAsync(imageFile, useTechnicalMessages, _basePath);
                    savedFiles.Add(fileName);
                }
                catch (Exception ex)
                {
                    savedFiles.Add(new ReturnModel
                    {
                        ResultError = _errorHandler.GetErrorException(ResultStatusCode.InternalError, ex, "", useTechnicalMessages),
                        StatusCode = ResultStatusCode.InternalError,
                        Data = imageFile.FileName
                    });
                }
            }

            return savedFiles;
        }

        private async Task<ReturnModel> SaveSingleImageAsync(IFormFile imageFile, bool useTechnicalMessages, string _basePath)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "El archivo de imagen no es válido", useTechnicalMessages),
                    HasError = true
                };
            }

            var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(fileExtension) || !_allowedExtensions.Contains(fileExtension))
            {
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, $"Formato de imagen no soportado. Extensiones permitidas: {string.Join(", ", _allowedExtensions)}", useTechnicalMessages),
                    HasError = true
                };
            }

            if (imageFile.Length > _maxFileSize)
            {

                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, $"Formato de imagen no soportado. Extensiones permitidas: {string.Join(", ", _allowedExtensions)}", useTechnicalMessages),
                    HasError = true
                };
            }

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_basePath, fileName);

            await using var fileStream = new FileStream(
                filePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 81920,
                useAsync: true);

            await imageFile.CopyToAsync(fileStream);

            return new ReturnModel
            {
                StatusCode = ResultStatusCode.OK,
                HasError = false,
                Data = fileName
            };
        }

        public async Task RemoveListingRentImage(string imageName)
        {
            if (string.IsNullOrWhiteSpace(imageName) || !(await VerifyListingRentImage(imageName)))
            {
                return;
            }
            string _basePath = await _SystemConfigurationRepository.GetListingResourcePath();
            var imagePath = Path.Combine(_basePath, imageName);
            int attempt = 0;

            while (attempt < 3)
            {
                try
                {
                    File.Delete(imagePath);
                    return;
                }
                catch (IOException ex) when (IsFileLocked(ex))
                {
                    attempt++;

                    if (attempt < 3)
                    {
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    return;
                }
            }

            return;
        }

        public async Task<bool> VerifyListingRentImage(string fileName)
        {
            string _basePath = await _SystemConfigurationRepository.GetListingResourcePath();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            var imagePath = Path.Combine(_basePath, fileName);
            return File.Exists(imagePath);
        }

        /// <summary>
        /// Elimina una imagen cuando se libere (patrón Dispose)
        /// </summary>
        public IDisposable ScheduleDeleteOnRelease(string imageName)
        {
            return new ImageDeleteDisposable(this, imageName);
        }

        private bool IsFileLocked(IOException ex)
        {
            var errorCode = ex.HResult & 0xFFFF;
            return errorCode == 32 || errorCode == 33; // ERROR_SHARING_VIOLATION o ERROR_LOCK_VIOLATION
        }

        // Implementación del disposable para eliminación diferida
        private class ImageDeleteDisposable : IDisposable
        {
            private readonly IImageService _service;
            private readonly string _imageName;

            public ImageDeleteDisposable(IImageService service, string imageName)
            {
                _service = service;
                _imageName = imageName;
            }

            public void Dispose()
            {
                try
                {
                    _service.RemoveListingRentImage(_imageName);
                }
                catch (Exception ex)
                {
                    //TODO: Guardar Log Error
                }
            }
        }
    }
}
