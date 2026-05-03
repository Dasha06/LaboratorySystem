using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class PatternAnalysise
{
    public int PatternId { get; set; }

    public long AnalysisId { get; set; }

    public bool PatAnalysisIsActive { get; set; }

    public int MaterialNumber { get; set; }

    public int? MaterialId { get; set; }

    public virtual Material? Material { get; set; }

    public virtual Pattern Pattern { get; set; } = null!;
}
