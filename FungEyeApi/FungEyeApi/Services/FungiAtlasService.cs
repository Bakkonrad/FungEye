using FungEyeApi.Data;
using FungEyeApi.Interfaces;

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


    }
}
