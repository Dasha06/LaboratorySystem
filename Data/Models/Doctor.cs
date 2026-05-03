using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Doctor
{
    public long DocId { get; set; }

    public string DocFio { get; set; } = null!;

    public long? LpuId { get; set; }

    public virtual Lpu? Lpu { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Pattern> Patterns { get; set; } = new List<Pattern>();
}
