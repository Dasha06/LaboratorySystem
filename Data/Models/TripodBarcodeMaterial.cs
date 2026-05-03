using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class TripodBarcodeMaterial
{
    public long TripodId { get; set; }

    public decimal BarcodeMatId { get; set; }

    public int AnalysisDepId { get; set; }

    public virtual BarcodeMaterial BarcodeMaterial { get; set; } = null!;

    public virtual Tripod Tripod { get; set; } = null!;
}
