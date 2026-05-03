using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class AnalysisWork
{
    public int AnalysisWorkId { get; set; }

    public string AnalysisWorkName { get; set; } = null!;

    public long? AnalysisId { get; set; }

    public virtual Analysise? Analysis { get; set; }

    public virtual ICollection<QualitativeStandart> QualitativeStandarts { get; set; } = new List<QualitativeStandart>();

    public virtual ICollection<QuantitativeStandart> QuantitativeStandarts { get; set; } = new List<QuantitativeStandart>();
}
