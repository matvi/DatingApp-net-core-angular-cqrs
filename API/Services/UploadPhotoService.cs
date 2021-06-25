using System.Threading.Tasks;
using API.Configuration;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class UploadPhotoService : IUploadPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public UploadPhotoService(IOptions<CloudinarySettings> cloudinarySettings)
        {
            var account = new Account(cloudinarySettings.Value.CloudName, cloudinarySettings.Value.ApiKey, cloudinarySettings.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result;
        }

        public async Task<ImageUploadResult> UploadPhoto(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face");
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = transformation
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult;
        }
    }
}