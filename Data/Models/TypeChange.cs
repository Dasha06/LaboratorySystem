using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class TypeChange
{
    public int TypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<OrderChange> OrderChanges { get; set; } = new List<OrderChange>();

    public virtual ICollection<PatientChange> PatientChanges { get; set; } = new List<PatientChange>();
}
