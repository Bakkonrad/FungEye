using FungEyeApi.Data.Entities;
using Newtonsoft.Json;

namespace FungEyeApi.Models
{
    public class Report
    {
        public Report() {}
        public Report(ReportEntity reportEntity) 
        {
            Id = reportEntity.Id;
            CreatedAt = reportEntity.CreatedAt;
            PostId = reportEntity.PostId;
            CommentId = reportEntity.CommentId;
            Completed = reportEntity.Completed;
            ReportedBy = new FollowUser(reportEntity.ReportedBy);
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("reportedBy")]
        public FollowUser? ReportedBy { get; set; }

        [JsonProperty("postId")]
        public int? PostId { get; set; }

        [JsonProperty("commentId")]
        public int? CommentId { get; set; }

        [JsonProperty("completed")]
        public bool Completed { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
