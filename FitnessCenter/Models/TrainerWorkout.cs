using System;
using System.Collections.Generic;

namespace FitnessCenter.Models;

public partial class TrainerWorkout
{
    public decimal Id { get; set; }

    public decimal? TrinerId { get; set; }

    public decimal? WorkoutId { get; set; }

    public virtual Staff? Triner { get; set; }

    public virtual Workout? Workout { get; set; }
}
