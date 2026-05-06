using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class BarcodeMaterial
{
    public decimal BarcodeMatId { get; set; }

    public long? OrderId { get; set; }

    public int? MaterialId { get; set; }

    public int AnalysisDepId { get; set; }

    public virtual AnalysisDepartment AnalysisDep { get; set; } = null!;

    public virtual ICollection<BarcodeAnalysise> BarcodeAnalysises { get; set; } = new List<BarcodeAnalysise>();

    public virtual ICollection<BarcodeComplex> BarcodeComplexes { get; set; } = new List<BarcodeComplex>();

    public virtual Material? Material { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ICollection<TripodBarcodeMaterial> TripodBarcodeMaterials { get; set; } = new List<TripodBarcodeMaterial>();
}
