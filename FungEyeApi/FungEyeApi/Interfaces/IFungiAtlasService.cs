using FungEyeApi.Models;

namespace FungEyeApi.Interfaces
{
    public interface IFungiAtlasService
    {
        Task<bool> AddFungi(Fungi fungi);
        Task<bool> EditFungi(Fungi fungi);

        Task<bool> DeleteFungi(int fungiId);
        Task<Fungi> GetFungi(int fungiId, int? userId = null);
        Task<Fungi> GetFungiByName(string fungiName, int? userId = null);
        Task<List<Fungi>> GetFungies(int? userId = null, int? page = null, string? search = null);

        Task<bool> AddFungiToCollection(int userId, int fungiId);
        Task<bool> DeleteFungiFromCollection(int userId, int fungiId);

    }
}
