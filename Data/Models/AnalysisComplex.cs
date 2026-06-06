using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class AnalysisComplex
{
    public int ComplexId { get; set; }

    public string ComplexName { get; set; } = null!;

    public string ComplexCodeName { get; set; } = null!;

    public int AnalysisDepId { get; set; }

    public string? ComplexDescription { get; set; }

    public string ComplexNomenclatureCode { get; set; } = null!;

    public virtual AnalysisDepartment AnalysisDep { get; set; } = null!;

    public virtual ICollection<BarcodeComplex> BarcodeComplexes { get; set; } = new List<BarcodeComplex>();

    public virtual ICollection<ContractComplex> ContractComplexes { get; set; } = new List<ContractComplex>();

    public virtual ICollection<Analysise> Analyses { get; set; } = new List<Analysise>();
}
