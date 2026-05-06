using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class AnalysisWork
{
    public long AnalysisWorkId { get; set; }

    public string AnalysisWorkName { get; set; } = null!;

    public int MaterialId { get; set; }

    public long AnalysisId { get; set; }

    public virtual Analysise Analysis { get; set; } = null!;

    public virtual Material Material { get; set; } = null!;

    public virtual ICollection<QualitativeStandart> QualitativeStandarts { get; set; } = new List<QualitativeStandart>();

    public virtual ICollection<QuantitativeStandart> QuantitativeStandarts { get; set; } = new List<QuantitativeStandart>();
}
