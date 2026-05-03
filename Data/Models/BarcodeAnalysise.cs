using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class BarcodeAnalysise
{
    public decimal BarcodeId { get; set; }

    public long AnalysisId { get; set; }

    public string? Result { get; set; }

    public int AnalysisDepId { get; set; }

    public virtual Analysise Analysis { get; set; } = null!;

    public virtual BarcodeMaterial BarcodeMaterial { get; set; } = null!;
}
