using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCenter.Models;

public partial class Staff
{
    public decimal StaffId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Image { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public DateTime? JoinDate { get; set; }

    public decimal? RoleId { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Loginpic> Loginpics { get; set; } = new List<Loginpic>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<TrainerWorkout> TrainerWorkouts { get; set; } = new List<TrainerWorkout>();
}
