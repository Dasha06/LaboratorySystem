using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Order
{
    public long OrderId { get; set; }

    public long? DocId { get; set; }

    public string? OrderLpuDepartment { get; set; }

    public string OrderStatus { get; set; } = null!;

    public long PatientId { get; set; }

    public long LpuId { get; set; }

    public bool OrderIsCountingInContract { get; set; }

    public virtual ICollection<BarcodeMaterial> BarcodeMaterials { get; set; } = new List<BarcodeMaterial>();

    public virtual Doctor? Doc { get; set; }

    public virtual Lpu Lpu { get; set; } = null!;

    public virtual ICollection<OrderChange> OrderChanges { get; set; } = new List<OrderChange>();

    public virtual Patient Patient { get; set; } = null!;

    public virtual ICollection<LpuContract> ConLpus { get; set; } = new List<LpuContract>();

    // Вычисляемое свойство для получения даты создания заказа из первого OrderChange
    public DateTime? CreatedAt => OrderChanges?.MinBy(oc => oc.OrderChangeTime)?.OrderChangeTime;
}
