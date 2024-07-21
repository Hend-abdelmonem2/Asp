using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace challenge_Diabetes.Model
{
    public class Sport
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("الرياضه")]
        [MaxLength(100)]
        public string Name { get; set; }
        [DisplayName("الوصف")]
        [MaxLength(1000)]
        public string Description { get; set; }
        public string Photo { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
