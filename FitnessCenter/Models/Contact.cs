using System;
using System.Collections.Generic;

namespace FitnessCenter.Models;

public partial class Contact
{
    public decimal NoteId { get; set; }

    public string? Notetext { get; set; }

    public DateTime? Notedate { get; set; }

    public string? Useremail { get; set; }
}
