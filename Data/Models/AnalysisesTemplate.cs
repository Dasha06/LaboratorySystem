using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class AnalysisesTemplate
{
    public int AnalysisTempId { get; set; }

    public long AnalysisId { get; set; }

    public string AnalysisTempName { get; set; } = null!;

    public virtual Analysise Analysis { get; set; } = null!;
}
