
namespace FungEyeApi.Data.Entities
{
    public class ReportEntity
    {
        public int Id { get; set; }
        public required int ReportedById { get; set; }
        public UserEntity? ReportedBy { get; set; }
        public bool Completed { get; set; }


        // Post to which the report belongs
        public int? PostId { get; set; }
        public PostEntity? Post { get; set; }

        // Comment to which the report belongs
        public int? CommentId { get; set; }
        public CommentEntity? Comment { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public static ReportEntity Create(int reportedById, int? postId = null, int? commentId = null)
        {
            return new ReportEntity
            {
                ReportedById = reportedById,
                PostId = postId ?? null,
                CommentId = commentId ?? null,
                Completed = false,
                CreatedAt = DateTime.Now
            };
        }
    }


}
