using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class ContractComplex
{
    public long ContractId { get; set; }

    public int ComplexId { get; set; }

    public double ContractComplexCost { get; set; }

    public virtual AnalysisComplex Complex { get; set; } = null!;

    public virtual Contract Contract { get; set; } = null!;
}
