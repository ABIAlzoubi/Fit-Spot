using System;
using System.Collections.Generic;

namespace FitnessCenter.Models;

public partial class Testimonial
{
    public decimal TestemonialId { get; set; }

    public string? TestimonialsText { get; set; }

    public DateTime? TestimonialsDate { get; set; }

    public decimal? MemberId { get; set; }

    public bool? Approved { get; set; }

    public virtual Member? Member { get; set; }
}
