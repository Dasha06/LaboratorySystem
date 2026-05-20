using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class QualityParameter
{
    public long QualityParamId { get; set; }

    public long QualitativeStandartId { get; set; }

    public string QualityCondition { get; set; } = null!;

    public string? QualityDescription { get; set; }

    public string QualityTypeCondition { get; set; } = null!;

    public virtual QualitativeStandart QualitativeStandart { get; set; } = null!;
}
