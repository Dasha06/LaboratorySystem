using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class BarcodeComplex
{
    public decimal BarcodeMatId { get; set; }

    public int ComplexId { get; set; }

    public int AnalysisDepId { get; set; }

    public virtual BarcodeMaterial BarcodeMaterial { get; set; } = null!;

    public virtual AnalysisComplex Complex { get; set; } = null!;
}
