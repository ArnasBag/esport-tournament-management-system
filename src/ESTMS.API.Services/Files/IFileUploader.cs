using Microsoft.AspNetCore.Http;

namespace ESTMS.API.Services.Files;

public interface IFileUploader
{
    Task<string> UploadFileAsync(IFormFile file);
    Task DeleteFileAsync(string fileUrl);
}
