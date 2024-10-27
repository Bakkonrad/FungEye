namespace FungEyeApi.Data.Entities.Fungies
{
    public class UserFungiCollectionEntity
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public int FungiId { get; set; }
        public FungiEntity Fungi { get; set; }
    }
}
