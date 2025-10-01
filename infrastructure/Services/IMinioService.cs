using Microsoft.AspNetCore.Http;

namespace infrastructure.Services
{
    public interface IMinioService
    {
        Task<string> UploadFileAsync(IFormFile file, string objectName);
        Task<bool> DeleteFileAsync(string objectName);
        Task<string> GetPresignedUrlAsync(string objectName);
        Task<bool> FileExistsAsync(string objectName);
        Task EnsureBucketExistsAsync();
    }
}
