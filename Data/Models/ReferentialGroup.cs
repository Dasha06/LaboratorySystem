using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class ReferentialGroup
{
    public int RefGroupId { get; set; }

    public string RefGroupName { get; set; } = null!;

    public string? RefGroupGender { get; set; }

    public double? RefGroupLowAge { get; set; }

    public string? RefGroupLowIf { get; set; }

    public double? RefGroupHighAge { get; set; }

    public string? RefGroupHighIf { get; set; }

    public string? RefGroupCondition { get; set; }

    public virtual ICollection<QualitativeStandart> QualitativeStandarts { get; set; } = new List<QualitativeStandart>();

    public virtual ICollection<QuantitativeStandart> QuantitativeStandarts { get; set; } = new List<QuantitativeStandart>();
}
