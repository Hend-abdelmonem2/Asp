using System.ComponentModel.DataAnnotations;

namespace challenge_Diabetes.DTO
{
    public class RegisterModelDTO 
    {
       

        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required, StringLength(128)]
        public string Email { get; set; }
        [Required, StringLength(100)]
        public string Phone { get; set; }
        public IFormFile Photo { get; set; }
        
        [Required, StringLength(256)]
        public string Password { get; set; }
        [Required]
        
        public string Address{ get; set; }
    }
}


