using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class AnalysesTemplate
{
    public int AnalysisTempId { get; set; }

    public string AnalysisTempName { get; set; } = null!;

    public virtual ICollection<Analysise> Analyses { get; set; } = new List<Analysise>();
}
