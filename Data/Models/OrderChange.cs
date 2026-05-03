using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class OrderChange
{
    public long OrderId { get; set; }

    public int WorkerId { get; set; }

    public DateTime OrderChangeTime { get; set; }

    public int TypeId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual TypeChange Type { get; set; } = null!;

    public virtual Worker Worker { get; set; } = null!;
}
