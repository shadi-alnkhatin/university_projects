using CloudinaryDotNet.Actions;

namespace ChaatyApi.Services.interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile file);
        Task<DeletionResult> DeleteImageAsync(string PublicId);

    }
}
