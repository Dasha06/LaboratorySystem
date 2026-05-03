using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Order
{
    public long OrderId { get; set; }

    public long? DocId { get; set; }

    public string? OrderLpuDepartment { get; set; }

    public string? OrderStatus { get; set; }

    public long PatientId { get; set; }

    public long? LpuId { get; set; }

    public virtual ICollection<BarcodeMaterial> BarcodeMaterials { get; set; } = new List<BarcodeMaterial>();

    public virtual Doctor? Doc { get; set; }

    public virtual Lpu? Lpu { get; set; }

    public virtual ICollection<OrderChange> OrderChanges { get; set; } = new List<OrderChange>();

    public virtual Patient Patient { get; set; } = null!;

    public virtual ICollection<LpuContract> ConLpus { get; set; } = new List<LpuContract>();
}
