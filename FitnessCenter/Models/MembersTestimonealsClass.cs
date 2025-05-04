namespace FitnessCenter.Models
{
    public class MembersTestimonealsClass
    {

        public decimal testimonialId { get; set; } 
        public string? TestimonialText { get; set; }
        public DateTime? TestimonialDate { get; set; }
        public string? MemberFirstName { get;set; }
        public string? MemberLastName { get;set; }
        public bool? Approved { get; set; }
    }
}
