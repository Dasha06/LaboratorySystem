using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Measurement
{
    public int MeasurementId { get; set; }

    public string MeasurementName { get; set; } = null!;

    public virtual ICollection<QuantitativeStandart> QuantitativeStandarts { get; set; } = new List<QuantitativeStandart>();
}
