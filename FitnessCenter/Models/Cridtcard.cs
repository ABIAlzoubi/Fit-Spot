using System;
using System.Collections.Generic;

namespace FitnessCenter.Models;

public partial class Cridtcard
{
    public decimal CardId { get; set; }

    public decimal? CardNumber { get; set; }

    public DateTime? Expirationdate { get; set; }

    public decimal? CodeCvv { get; set; }

    public decimal? Cardbalance { get; set; }
}
