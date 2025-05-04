using System;
using System.Collections.Generic;

namespace FitnessCenter.Models;

public partial class Role
{
    public decimal RoleId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
