using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCenter.Models;

public partial class Member
{
    public decimal MemberId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Image { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public DateTime? JoinDate { get; set; }

    public decimal? WorkoutPlaneId { get; set; }

    public decimal? RoleId { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual Workout? WorkoutPlane { get; set; }
}
