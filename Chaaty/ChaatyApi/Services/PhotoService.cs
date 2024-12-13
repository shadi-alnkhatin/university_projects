using ChaatyApi.Helpers;
using ChaatyApi.Services.interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace ChaatyApi.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary cloudinary;
        public PhotoService(IOptions<CloudinarySettings> options)
        {

            var acc = new Account
                (
                    options.Value.CloudinaryName,
                    options.Value.ApiKey,
                    options.Value.ApiSecret
                );
            cloudinary=new Cloudinary( acc );
            
        }
        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeleteImageAsync(string PublicId)
        {
            var deleteParams = new DeletionParams(PublicId);

            var result = await cloudinary.DestroyAsync(deleteParams);

            return result;
        }

    }
}
