using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Tripod
{
    public long TripodId { get; set; }

    public string TripodName { get; set; } = null!;

    public DateOnly TripodCreateDate { get; set; }

    public int TripodMaxCell { get; set; }

    public virtual ICollection<TripodBarcodeMaterial> TripodBarcodeMaterials { get; set; } = new List<TripodBarcodeMaterial>();
}
