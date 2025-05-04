using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCenter.Models;

public partial class Loginpic
{
    public decimal Id { get; set; }

    public string? Loginimagepath { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public decimal? Adminid { get; set; }

    public virtual Staff? Admin { get; set; }
}
