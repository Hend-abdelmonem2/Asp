using challenge_Diabetes.Model;

namespace challange_Diabetes.Model
{
    public class Favorite
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string UserId { get; set; }
        public Doctor Doctor { get; set; }

    }
}
