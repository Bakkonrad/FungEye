using FungEyeApi.Models;

namespace FungEyeApi.Data.Entities
{
    public class FriendshipEntity
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public int FriendId { get; set; }
        public UserEntity Friend { get; set; }
    }
}
