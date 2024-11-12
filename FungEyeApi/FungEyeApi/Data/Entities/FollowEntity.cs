namespace FungEyeApi.Data.Entities
{
    public class FollowEntity
    {
        public int UserId { get; set; }
        public UserEntity? User { get; set; }

        public int FollowedUserId { get; set; }
        public UserEntity? FollowedUser { get; set; }
    }
}
