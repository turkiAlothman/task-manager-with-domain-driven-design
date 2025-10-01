using infrastructure.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace infrastructure.Services
{
    public class MinioService : IMinioService
    {
        private readonly IMinioClient _minioClient;
        private readonly MinioSettings _minioSettings;

        public MinioService(IMinioClient minioClient, IOptions<MinioSettings> minioSettings)
        {
            _minioClient = minioClient;
            _minioSettings = minioSettings.Value;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string objectName)
        {
            await EnsureBucketExistsAsync();

            using var stream = file.OpenReadStream();
            
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_minioSettings.BucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(file.ContentType);

            await _minioClient.PutObjectAsync(putObjectArgs);
            
            return objectName;
        }

        public async Task<bool> DeleteFileAsync(string objectName)
        {
            try
            {
                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(_minioSettings.BucketName)
                    .WithObject(objectName);

                await _minioClient.RemoveObjectAsync(removeObjectArgs);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> GetPresignedUrlAsync(string objectName)
        {
            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(_minioSettings.BucketName)
                .WithObject(objectName)
                .WithExpiry(_minioSettings.PresignedUrlExpiry);

            return await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
        }

        public async Task<bool> FileExistsAsync(string objectName)
        {
            try
            {
                var statObjectArgs = new StatObjectArgs()
                    .WithBucket(_minioSettings.BucketName)
                    .WithObject(objectName);

                await _minioClient.StatObjectAsync(statObjectArgs);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task EnsureBucketExistsAsync()
        {
            var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(_minioSettings.BucketName);

            bool found = await _minioClient.BucketExistsAsync(bucketExistsArgs);
            
            if (!found)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(_minioSettings.BucketName);
                
                await _minioClient.MakeBucketAsync(makeBucketArgs);
            }
        }
    }
}
