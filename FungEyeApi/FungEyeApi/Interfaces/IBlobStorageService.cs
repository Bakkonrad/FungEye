using FungEyeApi.Enums;
using FungEyeApi.Models;

namespace FungEyeApi.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadFile(IFormFile file, BlobContainerEnum blobContainer);
        Task<bool> DeleteFile(string fileUrl, BlobContainerEnum blobContainer);
    }
}
