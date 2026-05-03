using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Pattern
{
    public int PatternId { get; set; }

    public long? ConLpuId { get; set; }

    public long? DocId { get; set; }

    public string PatternName { get; set; } = null!;

    public string PatternShortcut { get; set; } = null!;

    public virtual LpuContract? ConLpu { get; set; }

    public virtual Doctor? Doc { get; set; }

    public virtual ICollection<PatternAnalysise> PatternAnalysises { get; set; } = new List<PatternAnalysise>();
}
