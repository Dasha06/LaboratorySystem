using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class LpuContract
{
    public long ConLpuId { get; set; }

    public long ContractId { get; set; }

    public long LpuId { get; set; }

    public bool ConLpuIsActive { get; set; }

    public virtual Contract Contract { get; set; } = null!;

    public virtual Lpu Lpu { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
