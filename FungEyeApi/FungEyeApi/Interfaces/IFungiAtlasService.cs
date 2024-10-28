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
        Task<List<Fungi>> GetFungies(int? page = null);
        
        Task<bool> SaveFungi(int userId, int fungiId);

    }
}
