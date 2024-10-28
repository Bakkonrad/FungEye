using FungEyeApi.Data.Entities;
using FungEyeApi.Data.Entities.Fungies;

namespace FungEyeApi.Models
{
    public class Fungi
    {
        public int Id { get; set; }
        public required string LatinName { get; set; }
        public string? PolishName { get; set; }
        public string? Description { get; set; }
        public string? Edibility { get; set; }
        public string? Toxicity { get; set; }
        public string? Habitat { get; set; }

        public List<string> ImagesUrl { get; set; }
        // public ICollection<UserFungiCollectionEntity> UserCollections { get; set; }
    }
}
