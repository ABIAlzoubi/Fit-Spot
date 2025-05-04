using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCenter.Models;

public partial class Homepage
{
    public decimal Id { get; set; }

    public string? Mainpic { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFileMainPic { get; set; }
    public string? Mainstatement { get; set; }

    public string? Joinuspic { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFileJoinUs { get; set; }
    public string? Joinusparagraph { get; set; }

    public string? Discountpic { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFileDiscount { get; set; }

    public string? Discountheader { get; set; }

    public string? Feedbackpic { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFileFeedback { get; set; }
}
