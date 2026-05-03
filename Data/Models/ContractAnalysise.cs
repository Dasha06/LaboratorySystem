using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class ContractAnalysise
{
    public long ContractId { get; set; }

    public long AnalysisId { get; set; }

    public double ContrAnalysisCost { get; set; }

    public virtual Analysise Analysis { get; set; } = null!;

    public virtual Contract Contract { get; set; } = null!;
}
