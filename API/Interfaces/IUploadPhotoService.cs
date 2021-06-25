using System.Threading.Tasks;
using API.Configuration;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IUploadPhotoService
    {
        Task<ImageUploadResult> UploadPhoto(IFormFile filename);

        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}