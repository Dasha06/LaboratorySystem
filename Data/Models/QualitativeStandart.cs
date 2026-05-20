using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class QualitativeStandart
{
    public long QualtityStandartId { get; set; }

    public int RefGroupId { get; set; }

    public long AnalysisWorkId { get; set; }

    public virtual AnalysisWork AnalysisWork { get; set; } = null!;

    public virtual ICollection<QualityParameter> QualityParameters { get; set; } = new List<QualityParameter>();

    public virtual ReferentialGroup RefGroup { get; set; } = null!;
}
