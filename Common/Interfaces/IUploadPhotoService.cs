using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Common.Interfaces
{
    public interface IUploadPhotoService
    {
        Task<ImageUploadResult> UploadPhoto(IFormFile filename);

        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}