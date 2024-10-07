using FungEyeApi.Models;

namespace FungEyeApi.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadFile(IFormFile file);
        Task<bool> DeleteFile(string fileUrl);
    }
}
