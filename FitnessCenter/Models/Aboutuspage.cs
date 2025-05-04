using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCenter.Models;

public partial class Aboutuspage
{
    public decimal Id { get; set; }

    public string? Headerpic { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFileHeaderPic {  get; set; }
    public string? Headpic1 { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFileHeadPic1 { get; set; }
    public string? Headpic2 { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFileHeadPic2 { get; set; }
    public string? Aboutusheadertext { get; set; }

    public string? Aboutusparagraph { get; set; }
}
