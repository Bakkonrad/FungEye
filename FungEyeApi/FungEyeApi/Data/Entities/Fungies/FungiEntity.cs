using FungEyeApi.Data.Entities.Fungies;
using FungEyeApi.Models;

namespace FungEyeApi.Data.Entities
{
    public class FungiEntity
    {
        public int Id { get; set; }
        public string? LatinName { get; set; }
        public string? PolishName { get; set; }
        public string? Description { get; set; }
        public string? Edibility { get; set; }
        public string? Toxicity { get; set; }
        public string? Habitat { get; set; }

        public ICollection<FungiImageEntity>? Images { get; set; }
        public ICollection<UserFungiCollectionEntity>? UserCollections { get; set; }


        public static FungiEntity Create(Fungi fungi)
        {
            var result = new FungiEntity();

            result.LatinName = fungi.LatinName;
            result.PolishName = fungi.PolishName;
            result.Description = fungi.Description;
            result.Edibility = fungi.Edibility;
            result.Toxicity = fungi.Toxicity;
            result.Habitat = fungi.Habitat;

            return result;
        }
    }
}
