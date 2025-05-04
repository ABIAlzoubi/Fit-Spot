using System;
using System.Collections.Generic;

namespace FitnessCenter.Models;

public partial class Payment
{
    public decimal PaymentId { get; set; }

    public decimal? MemberId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual Member? Member { get; set; }
}
