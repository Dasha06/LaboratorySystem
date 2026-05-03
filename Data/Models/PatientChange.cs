using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class PatientChange
{
    public long PatientId { get; set; }

    public int WorkerId { get; set; }

    public DateTime PatientChangeTime { get; set; }

    public int TypeId { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual TypeChange Type { get; set; } = null!;

    public virtual Worker Worker { get; set; } = null!;
}
