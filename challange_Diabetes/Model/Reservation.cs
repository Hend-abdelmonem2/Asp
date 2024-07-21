using System.ComponentModel.DataAnnotations;

namespace challange_Diabetes.Model
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required, StringLength(100)]
        public string Phone { get; set; }

        [Required]
        public int age { get; set; }
        [Required]
        public string sex { get; set; }
        public string user_Id { get; set; }
        public int  doctor_Id { get; set; }
         public string Date { get; set; }
        public string Time { get; set; }


    }
}
