using Azure.Storage.Blobs;
using FungEyeApi.Enums;
using FungEyeApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FungEyeApi.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _connectionString;

        public BlobStorageService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("AzureBlobStorageConnection") ?? string.Empty;

            _blobServiceClient = new BlobServiceClient(_connectionString);
        }

        public async Task<string> UploadFile(IFormFile file, BlobContainerEnum blobContainer)
        {
            try
            {
                string _containerName = GetContainerName(blobContainer);
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var blobClient = blobContainerClient.GetBlobClient(fileName);

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                var url = blobClient.Uri.ToString();

                return url;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteFile(string fileUrl, BlobContainerEnum blobContainer)
        {
            try
            {
                if (fileUrl == "placeholder")
                {
                    return true;
                }
                string _containerName = GetContainerName(blobContainer);
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var fileName = Path.GetFileName(fileUrl);

                // Pobieranie klienta dla konkretnego bloba (pliku)
                var blobClient = blobContainerClient.GetBlobClient(fileName);
                var response = await blobClient.DeleteIfExistsAsync();
                return response.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string GetContainerName(BlobContainerEnum blobContainer)
        {
            return blobContainer switch
            {
                BlobContainerEnum.Posts => "posts",
                BlobContainerEnum.Users => "users",
                BlobContainerEnum.Fungies => "fungi",
                _ => throw new ArgumentException("Invalid blob container")
            };
        }
    }
}
