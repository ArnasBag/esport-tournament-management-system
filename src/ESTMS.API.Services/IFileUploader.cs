using Microsoft.AspNetCore.Http;

namespace ESTMS.API.Services;

public interface IFileUploader
{
    Task<string> UploadFileAsync(IFormFile file);
}
