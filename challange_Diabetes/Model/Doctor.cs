using challange_Diabetes.Model;
using System.ComponentModel.DataAnnotations;

namespace challenge_Diabetes.Model
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string userName { get; set; }
        [Phone]
        public string phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string address { get; set; }
        public string appointment { get; set; }
        public int DetectionPrice { get; set; }
        public string Doctorspecialization { get; set; }
        public string? Photo { get;set; }
        public string about { get; set; }
        public string waitingTime { get; set; }
        public bool Favorite { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public virtual ICollection<DoctorComment> DoctorComments { get; set; } = new List<DoctorComment>(); 
    }
}
