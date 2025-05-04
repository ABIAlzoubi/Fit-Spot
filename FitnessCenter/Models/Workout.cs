using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCenter.Models;

public partial class Workout
{
    public decimal WorkoutId { get; set; }

    public string? WorkoutName { get; set; }

    public decimal? WorkoutDuration { get; set; }

    public string? Shift { get; set; }

    public decimal? Price { get; set; }

    public string? Image { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();

    public virtual ICollection<TrainerWorkout> TrainerWorkouts { get; set; } = new List<TrainerWorkout>();
}
