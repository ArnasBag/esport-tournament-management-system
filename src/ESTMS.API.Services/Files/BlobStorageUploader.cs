using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace ESTMS.API.Services.Files;

public class BlobStorageUploader : IFileUploader
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageUploader(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        StreamReader streamReader = new StreamReader(file.OpenReadStream());
        var container = _blobServiceClient.GetBlobContainerClient("images");
        string filePath = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var blob = container.GetBlobClient(filePath);
        await blob.UploadAsync(streamReader.BaseStream);

        return filePath;
    }

    public async Task DeleteFileAsync(string fileUrl)
    {
        var container = _blobServiceClient.GetBlobContainerClient("images");
        var blob = container.GetBlobClient(fileUrl);
        await blob.DeleteIfExistsAsync();
    }
}
