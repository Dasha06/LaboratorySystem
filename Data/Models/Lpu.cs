using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Lpu
{
    public long LpuId { get; set; }

    public string LpuName { get; set; } = null!;

    public string? LpuEmail { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public virtual ICollection<LpuContract> LpuContracts { get; set; } = new List<LpuContract>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
