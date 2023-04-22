using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace ESTMS.API.Services;

public class LocalStorageFileUploader : IFileUploader
{
    private readonly IHostEnvironment _hostEnvironment;

    public LocalStorageFileUploader(IHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        string uploadsFolder = Path.Combine(_hostEnvironment.ContentRootPath, "uploads");

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        string filePath = Path.Combine(uploadsFolder, fileName);

        using FileStream stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return Path.Combine("/uploads", fileName);
    }
}
