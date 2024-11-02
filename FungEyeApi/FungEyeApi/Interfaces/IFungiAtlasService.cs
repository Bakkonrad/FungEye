using FungEyeApi.Data.Entities;
using FungEyeApi.Enums;
using FungEyeApi.Models;

namespace FungEyeApi.Interfaces
{
    public interface IFungiAtlasService
    {
        Task<bool> AddFungi(Fungi fungi);
        Task<bool> EditFungi(Fungi fungi);
        
        Task<bool> DeleteFungi(int fungiId);
        Task<Fungi> GetFungi(int fungiId);
        Task<Fungi> GetFungi(string fungiName);
        Task<List<Fungi>> GetFungies(int? page = null, string? search = null);
        
        Task<bool> AddFungiToCollection(int userId, int fungiId);
        Task<bool> DeleteFungiFromCollection(int userId, int fungiId);

    }
}
