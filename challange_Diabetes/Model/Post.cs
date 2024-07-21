using Newtonsoft.Json;

namespace challange_Diabetes.Model
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property
        
        public ICollection<PostComment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

        // User ID of the post creator
        public string UserId { get; set; }

    }
}
