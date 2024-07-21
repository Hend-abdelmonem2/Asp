using challange_Diabetes.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace challenge_Diabetes.Model
{
    public class ApplicationUser :IdentityUser 
    {
       public string ?Photo { get; set; }
        public string ?address { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<Measuring_Sugar> Measuring_Sugars { get; set; } = new List<Measuring_Sugar>();
        public virtual ICollection<Measuring_pressure> Measuring_pressures { get; set; } = new List<Measuring_pressure>();
        public virtual ICollection<Sport> Sports { get; set; } = new List<Sport>();
        public virtual ICollection<Observer> Observers { get; set; } = new List<Observer>();
        public virtual ICollection<Food> Foods { get; set; } = new List<Food>();
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
       // public virtual ICollection<Community> Communities { get; set; } = new List<Community>();
        public virtual ICollection <Post>Posts { get; set; }= new List<Post>();
        public virtual ICollection<PostComment> Comments { get; set; }= new List<PostComment>();
        public virtual ICollection<Measuring_weight> Measuring_Weights { get; set; } = new List<Measuring_weight>();
        public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
        public ICollection<PaymentMethod> PaymentMethods { get; set; }=new List<PaymentMethod>();
        public ICollection <PaymentRequest> PaymentRequests { get; set; }=new List<PaymentRequest>();
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    }
}
