using FungEyeApi.Data;
using FungEyeApi.Data.Entities;
using FungEyeApi.Data.Entities.Fungies;
using FungEyeApi.Enums;
using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using FungEyeApi.Models.Posts;
using Microsoft.EntityFrameworkCore;

namespace FungEyeApi.Services
{
    public class FungiAtlasService : IFungiAtlasService
    {
        //private readonly IConfiguration _configuration;
        private readonly DataContext db;

        public FungiAtlasService(DataContext db)
        {
            this.db = db;
        }
        
        public async Task<bool> AddFungi(Fungi fungi)
        {
            try
            {
                var fungiEntity = FungiEntity.Create(fungi);
                await db.Fungies.AddAsync(fungiEntity);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during adding fungi: " + ex.Message);
            }
        }
        
        public async Task<bool> EditFungi(Fungi fungi)
        {
            try
            {
                var fungiEntity = await db.Fungies.FirstOrDefaultAsync(f => f.Id == fungi.Id);

                if (fungiEntity == null)
                {
                    return false;
                }

                fungiEntity.Description = fungi.Description;
                fungiEntity.LatinName = fungi.LatinName;
                fungiEntity.PolishName = fungi.PolishName;
                fungiEntity.Edibility = fungi.Edibility;
                fungiEntity.Toxicity = fungi.Toxicity;
                fungiEntity.Habitat = fungi.Habitat;
                //fungiEntity.Images = 

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during editing comment: " + ex.Message);
            }
        }
        
        public async Task<bool> DeleteFungi(int fungiId)
        {
            try
            {
                var fungi = await db.Fungies.FirstOrDefaultAsync(f => f.Id == fungiId);

                if (fungi == null)
                {
                    return false;
                }

                db.Fungies.Remove(fungi);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during deleting fungi: " + ex.Message);
            }
        }

        public async Task<Fungi> GetFungi(int fungiId)
        {
            try
            {
                var fungiEntity = await db.Fungies.FirstOrDefaultAsync(u => u.Id == fungiId);
                if (fungiEntity == null)
                {
                    throw new Exception("Fungi not found");
                }

                return new Fungi(fungiEntity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error during fetching fungi :" + ex.Message);
            }
        }

        public async Task<Fungi> GetFungi(string fungiName)
        {
            try
            {
                var fungiEntity = await db.Fungies.FirstOrDefaultAsync(f => f.LatinName == fungiName);
                if (fungiEntity == null)
                {
                    throw new Exception("Fungi not found");
                }

                return new Fungi(fungiEntity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error during fetching fungi:" + ex.Message);
            }
        }

        public async Task<List<Fungi>> GetFungies(int? page = null, string? search = null)
        {
            try
            {
                var query = db.Fungies.AsQueryable();

                if (!String.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(f => f.LatinName != null && f.LatinName.Contains(search) ||
                                             f.PolishName != null && f.PolishName.Contains(search));
                }

                int pageSize = 5;
                int pageNumber = page ?? 1;
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                var fungies = await query.ToListAsync();

                return fungies.Select(u => new Fungi(u)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error during retrieving fungies: " + ex.Message);
            }
        }

        public async Task<bool> AddFungiToCollection(int userId, int fungiId)
        {
            try
            {
                if (await db.FungiesUserCollections.AnyAsync(f => f.FungiId == fungiId && f.UserId == userId))
                {
                    return false;
                }

                var userCollectionEntity = UserFungiCollectionEntity.Create(userId, fungiId);

                await db.FungiesUserCollections.AddAsync(userCollectionEntity);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Adding fungi to user collection failed. " + ex.Message);
            }
        }

        public async Task<bool> DeleteFungiFromCollection(int userId, int fungiId)
        {
            try
            {
                var fungiInUserCollection = await db.FungiesUserCollections.FirstOrDefaultAsync(r => r.FungiId == fungiId && r.UserId == userId);

                if (fungiInUserCollection == null)
                {
                    return false;
                }

                db.FungiesUserCollections.Remove(fungiInUserCollection);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Deleting fungi from user collection failed. " + ex.Message);
            }
        }
    }
}
