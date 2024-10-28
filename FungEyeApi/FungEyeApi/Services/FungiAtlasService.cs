using FungEyeApi.Data;
using FungEyeApi.Enums;
using FungEyeApi.Interfaces;
using FungEyeApi.Models;

namespace FungEyeApi.Services
{
    public class FungiAtlasService : IFungiAtlasService
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext db;

        public FungiAtlasService(DataContext db, IConfiguration configuration)
        {
            this._configuration = configuration;
            this.db = db;
        }
        
        public async Task<bool> AddFungi(Fungi fungi)
        {
            throw new NotImplementedException();
        }
        
        public async Task<bool> EditFungi(Fungi fungi)
        {
            throw new NotImplementedException();
        }
        
        public async Task<bool> DeleteFungi(int fungiId)
        {
            throw new NotImplementedException();
        }

        public async Task<Fungi> GetFungi(int fungiId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Fungi>> GetFungies(int? page = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveFungi(int userId, int fungiId)
        {
            throw new NotImplementedException();
        }
    }
}
