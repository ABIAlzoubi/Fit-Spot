using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCenter.Models;

public partial class Sharedlayout
{
    public decimal Id { get; set; }

    public string? Logo { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public string? Facebooklink { get; set; }

    public string? Twitterlink { get; set; }

    public string? Githublink { get; set; }

    public string? Photerparagraph { get; set; }

    public string? Homelocation { get; set; }

    public string? Copywritestatement { get; set; }
}
