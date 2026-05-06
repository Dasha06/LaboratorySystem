using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class AnalysisDepartment
{
    public int AnalysisDepId { get; set; }

    public string AnalysisDepName { get; set; } = null!;

    public virtual ICollection<AnalysisComplex> AnalysisComplexes { get; set; } = new List<AnalysisComplex>();

    public virtual ICollection<Analysise> Analysises { get; set; } = new List<Analysise>();

    public virtual ICollection<BarcodeMaterial> BarcodeMaterials { get; set; } = new List<BarcodeMaterial>();
}
