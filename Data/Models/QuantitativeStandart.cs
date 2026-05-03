using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class QuantitativeStandart
{
    public int QuantStandartId { get; set; }

    public int AnalysisWorkId { get; set; }

    public int RefGroupId { get; set; }

    public double QuantStandartLowNorm { get; set; }

    public double QuantStandartHighNorm { get; set; }

    public double QuantStandartLowPathology { get; set; }

    public double QuantStandartHighPathology { get; set; }

    public double QuantStandartLowCritical { get; set; }

    public double QuantStandartHighCritical { get; set; }

    public string? QuantStandartDescription { get; set; }

    public int MeasurementsId { get; set; }

    public virtual AnalysisWork AnalysisWork { get; set; } = null!;

    public virtual Measurement Measurements { get; set; } = null!;

    public virtual ReferentialGroup RefGroup { get; set; } = null!;
}
