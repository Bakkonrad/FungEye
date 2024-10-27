namespace FungEyeApi.Data.Entities
{
    public class FungiImageEntity
    {
        public FungiImageEntity() { }

        public int Id { get; set; }
        public required string ImageUrl { get; set; }

        public int FungiEntityId { get; set; }
        public FungiEntity FungiEntity { get; set; }
    }
}
