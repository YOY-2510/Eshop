using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EShop.Data;
using EShop.Services.Interface;
using Microsoft.Extensions.Options;

namespace EShop.Services
{
    public class CloudinaryService 
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string?> UploadImageAsync(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0) return null;

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "eshop"
            };
            var result = await _cloudinary.UploadAsync(uploadParams, cancellationToken);
            return result.StatusCode == System.Net.HttpStatusCode.OK ? result.SecureUrl.ToString() : null;
        }

        public async Task<bool> DeleteImageAsync(string publicId, CancellationToken cancellationToken)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result.Result == "ok" || result.Result == "not found";
        }
    }
}
