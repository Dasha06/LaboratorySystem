using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class QualitativeStandart
{
    public int QualtityStandartId { get; set; }

    public int RefGroupId { get; set; }

    public long AnalysisWorkId { get; set; }

    public string QualityStandartCondition { get; set; } = null!;

    public string? QualityStandartDescription { get; set; }

    public string QualityStandartTypeCodition { get; set; } = null!;

    public virtual AnalysisWork AnalysisWork { get; set; } = null!;

    public virtual ReferentialGroup RefGroup { get; set; } = null!;
}
