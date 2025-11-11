namespace EShop.Services.Interface
{
    public interface ICloudinaryService
    {
        Task<string?> UploadImageAsync(IFormFile file, CancellationToken cancellationToken);
        Task<bool> DeleteImageAsync(string publicId, CancellationToken cancellationToken);
    }
}
