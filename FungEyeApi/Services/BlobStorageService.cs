using Azure.Storage.Blobs;
using FungEyeApi.Interfaces;

namespace FungEyeApi.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly string? _connectionString;
        private readonly string _containerName = "zestyappimages";

        public BlobStorageService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("AzureBlobStorageConnection");
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                var blobServiceClient = new BlobServiceClient(_connectionString);
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var blobClient = blobContainerClient.GetBlobClient(fileName);

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                var url = blobClient.Uri.ToString();

                return url;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteFile(string fileUrl)
        {
            try
            {
                if (fileUrl == "https://zestyappblob.blob.core.windows.net/zestyappimages/placeholder.png")
                {
                    return true;
                }
                var blobServiceClient = new BlobServiceClient(_connectionString);
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);

                var fileName = Path.GetFileName(fileUrl);
                var blobClient = blobContainerClient.GetBlobClient(fileName);

                await blobClient.DeleteIfExistsAsync();
                return true;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
