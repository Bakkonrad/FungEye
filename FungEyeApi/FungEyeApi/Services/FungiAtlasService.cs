using FungEyeApi.Data;
using FungEyeApi.Data.Entities;
using FungEyeApi.Data.Entities.Fungies;
using FungEyeApi.Enums;
using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FungEyeApi.Services
{
    public class FungiAtlasService : IFungiAtlasService
    {
        private readonly DataContext db;
        private readonly IBlobStorageService _blobStorageService;

        public FungiAtlasService(DataContext db, IBlobStorageService blobStorageService)
        {
            this.db = db;
            _blobStorageService = blobStorageService;
        }

        public async Task<bool> AddFungi(Fungi fungi)
        {
            try
            {
                var fungiEntity = FungiEntity.Create(fungi);
                await db.Fungies.AddAsync(fungiEntity);
                await db.SaveChangesAsync();

                var fungiId = fungiEntity.Id;

                if (fungi.ImagesUrl != null)
                {
                    fungiEntity.Images = fungi.ImagesUrl.Select(i => new FungiImageEntity { ImageUrl = i, FungiEntityId = fungiId }).ToList();
                    await db.SaveChangesAsync();
                }

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
                var fungiImages = await db.FungiesImages.Where(i => i.FungiEntityId == fungi.Id).ToListAsync();

                if (fungiEntity == null)
                {
                    throw new Exception("Error during editing fungi: fungi not found.");
                }

                if (fungi.ImagesUrlsToDelete != null && fungi.ImagesUrlsToDelete.Count > 0)
                {
                    foreach (var imageUrl in fungi.ImagesUrlsToDelete)
                    {
                        var imageEntity = fungiImages.FirstOrDefault(i => i.ImageUrl == imageUrl);
                        if (imageEntity == null)
                        {
                            continue;
                        }
                        // delete image entity from database and blob storage
                        await _blobStorageService.DeleteFile(imageEntity.ImageUrl, BlobContainerEnum.Fungies);
                        db.FungiesImages.Remove(imageEntity);
                    }
                }

                fungiEntity.Description = fungi.Description;
                fungiEntity.LatinName = fungi.LatinName;
                fungiEntity.PolishName = fungi.PolishName;
                fungiEntity.Edibility = fungi.Edibility;
                fungiEntity.Toxicity = fungi.Toxicity;
                fungiEntity.Habitat = fungi.Habitat;
                if(fungi.ImagesUrl != null)
                {
                    var imageEntities = fungi.ImagesUrl?.Select(i => new FungiImageEntity { ImageUrl = i, FungiEntityId = fungiEntity.Id }).ToList();
                    
                    foreach(var image in imageEntities!)
                    {
                        fungiEntity.Images!.Add(image);

                    }
                }

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during editing fungi: " + ex.Message);
            }
        }

        public async Task<bool> DeleteFungi(int fungiId)
        {
            try
            {
                var fungi = await db.Fungies.FirstOrDefaultAsync(f => f.Id == fungiId);
                var fungiImages = await db.FungiesImages.Where(i => i.FungiEntityId == fungiId).ToListAsync();

                if (fungi == null)
                {
                    return false;
                }

                foreach (var image in fungiImages)
                {
                    await _blobStorageService.DeleteFile(image.ImageUrl, BlobContainerEnum.Fungies);
                }

                db.FungiesImages.RemoveRange(fungiImages);
                db.Fungies.Remove(fungi);

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during deleting fungi: " + ex.Message);
            }
        }

        public async Task<Fungi> GetFungi(int fungiId, int? userId = null)
        {
            try
            {
                var fungiEntity = await db.Fungies.Include(f => f.Images).FirstOrDefaultAsync(u => u.Id == fungiId);
                if (fungiEntity == null)
                {
                    throw new Exception("Fungi not found");
                }

                var result = await GetFungiInfo(new Fungi(fungiEntity), userId);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during fetching fungi :" + ex.Message);
            }
        }

        public async Task<Fungi> GetFungiByName(string fungiName, int? userId = null)
        {
            try
            {
                var fungiEntity = await db.Fungies.Include(f => f.Images).FirstOrDefaultAsync(f => f.LatinName == fungiName);
                if (fungiEntity == null)
                {
                    throw new Exception("Fungi not found");
                }

                var result = await GetFungiInfo(new Fungi(fungiEntity), userId);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during fetching fungi:" + ex.Message);
            }
        }

        public async Task<List<Fungi>> GetFungies(GetFungiParams getFungiParams)
        {
            try
            {
                var query = db.Fungies.Include(f => f.UserCollections).Include(f => f.Images).OrderBy(f => f.PolishName).AsQueryable();


                if (getFungiParams.SavedByUser != null && getFungiParams.SavedByUser is true)
                {
                    //query fungies saved by user
                    query = query.Where(f => f.UserCollections!.Any(uf => uf.UserId == getFungiParams.UserId));
                }



                List<Fungi> result = new List<Fungi>();

                //if search parameter is provided, filter fungies by search parameter but if Letter parameter is provided, filter fungies by first letter of PolishName
                if (!String.IsNullOrWhiteSpace(getFungiParams.Letter))
                {
                    query = query.Where(f => f.PolishName != null && f.PolishName.StartsWith(getFungiParams.Letter));
                }
                else if(!String.IsNullOrWhiteSpace(getFungiParams.Search))
                {
                    query = query.Where(f => f.LatinName != null && f.LatinName.Contains(getFungiParams.Search) ||
                                             f.PolishName != null && f.PolishName.Contains(getFungiParams.Search));
                }
                else if(!String.IsNullOrWhiteSpace(getFungiParams.Edibility)) 
                {
                    query = query.Where(f => f.Edibility != null && f.Edibility.Contains(getFungiParams.Edibility));
                }
                else if (!String.IsNullOrWhiteSpace(getFungiParams.Toxicity))
                {
                    query = query.Where(f => f.Toxicity != null && f.Toxicity.Contains(getFungiParams.Toxicity));
                }
                else if (!String.IsNullOrWhiteSpace(getFungiParams.Habitat))
                {
                    query = query.Where(f => f.Habitat != null && f.Habitat.Contains(getFungiParams.Habitat));
                }

                int pageSize = getFungiParams.PageSize ?? 20;
                int pageNumber = getFungiParams.Page ?? 1;
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                var fungies = await query.ToListAsync();

                foreach (var fungi in fungies)
                {
                    result.Add(await GetFungiInfo(new Fungi(fungi), getFungiParams.UserId));
                }

                return result;
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

        private async Task<Fungi> GetFungiInfo(Fungi fungi, int? userId = null)
        {
            if (userId == null)
            {
                return fungi;
            }
            var collections = db.FungiesUserCollections.AsQueryable();

            fungi.SavedByUser = await collections.AnyAsync(f => f.FungiId == fungi.Id && f.UserId == userId);

            return fungi;
        }
    }
}
