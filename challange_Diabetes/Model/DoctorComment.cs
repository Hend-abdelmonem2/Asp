namespace challange_Diabetes.Model
{
    public class DoctorComment
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public string UserProfilePictureUrl { get; set; }
        public string UserId { get; set; }
    }
}
