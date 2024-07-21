namespace challange_Diabetes.Model
{
    public class PostComment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // User ID of the comment creator
        public string UserId { get; set; }
        

    }
}
