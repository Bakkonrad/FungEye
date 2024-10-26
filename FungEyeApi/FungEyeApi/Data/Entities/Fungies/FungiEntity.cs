using FungEyeApi.Data.Entities.Fungies;

namespace FungEyeApi.Data.Entities
{
    public class FungiEntity
    {
        public FungiEntity() { }

        public int Id { get; set; }
        public required string LatinName { get; set; }
        public string? PolishName { get; set; }
        public string? Description { get; set; }
        public string? Edibility { get; set; }
        public string? Toxicity { get; set; }
        public string? Habitat { get; set; }

        public ICollection<FungiImageEntity> Images { get; set; }
        public ICollection<UserFungiCollectionEntity> UserCollections { get; set; }
    }
}
