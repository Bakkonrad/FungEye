using FungEyeApi.Data.Entities;
using FungEyeApi.Data.Entities.Fungies;
using Newtonsoft.Json;

namespace FungEyeApi.Models
{
    public class Fungi
    {
        public Fungi()
        {
        }

        public Fungi(FungiEntity fungiEntity)
        {
            Id = fungiEntity.Id;
            LatinName = fungiEntity.LatinName;
            PolishName = fungiEntity.PolishName;
            Description = fungiEntity.Description;
            Edibility = fungiEntity.Edibility;
            Toxicity = fungiEntity.Toxicity;
            Habitat = fungiEntity.Habitat;
            ImagesUrl = fungiEntity.Images?.Select(i => i.ImageUrl).ToList();
        }

        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("latinName")]
        public string? LatinName { get; set; }

        [JsonProperty("polishName")]
        public string? PolishName { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("edibility")]
        public string? Edibility { get; set; }

        [JsonProperty("toxicity")]
        public string? Toxicity { get; set; }

        [JsonProperty("habitat")]
        public string? Habitat { get; set; }

        [JsonProperty("savedByUser")]
        public bool? SavedByUser { get; set; }

        [JsonProperty("imagesUrl")]
        public List<string>? ImagesUrl { get; set; }

        [JsonProperty("imagesUrlsToDelete")]
        public List<string>? ImagesUrlsToDelete { get; set; }
    }
}
