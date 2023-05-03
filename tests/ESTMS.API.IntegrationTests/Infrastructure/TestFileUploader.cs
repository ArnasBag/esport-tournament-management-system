using ESTMS.API.Services.Files;
using Microsoft.AspNetCore.Http;

namespace ESTMS.API.IntegrationTests.Infrastructure;

public class TestFileUploader : IFileUploader
{
    public Task DeleteFileAsync(string fileUrl)
    {
        return Task.CompletedTask;
    }

    public Task<string> UploadFileAsync(IFormFile file)
    {
        return Task.FromResult("file.png");
    }
}
