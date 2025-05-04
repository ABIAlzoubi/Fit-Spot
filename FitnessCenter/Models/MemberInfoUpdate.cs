using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCenter.Models
{
    public class MemberInfoUpdate
    {
        public decimal MemberId { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Image { get; set; }
        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }

        public string? Password { get; set; }

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    }
}
