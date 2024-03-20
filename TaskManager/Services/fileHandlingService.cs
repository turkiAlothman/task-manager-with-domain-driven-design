using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Net;
using TaskManager.Models.DomainModels;

namespace TaskManager.Services
{
    public static class fileHandlingService
    {
public static async Task UploadFileCompletedEventArgs(List<Attachments> attachments ,List<IFormFile> Files , IConfiguration _configuration) {
            Directory.CreateDirectory((_configuration.GetValue<string>("TaskAttachmantsStoragePathUrl") + attachments.First().task.Id).Replace("static", _configuration.GetValue<string>("StoragePath")));
            foreach ((Attachments attach, IFormFile file) in attachments.Zip(Files))
            {
                string startPerfix = _configuration.GetValue<string>("TaskAttachmantsStoragePathUrl") + attach.task.Id;
                string Url = startPerfix + "/" + Path.GetFileName(file.FileName).Replace(" ", "_");
                string PhysicalPath = Url.Replace("static", _configuration.GetValue<string>("StoragePath"));
                string FullPath = Path.GetFullPath(PhysicalPath);
                Debug.WriteLine(FullPath);

                attach.url = Url;
                attach.PhysicalPath = PhysicalPath;


                using (var stream = System.IO.File.Create(FullPath))
                    await file.CopyToAsync(stream);
            }

        }
    }
}
