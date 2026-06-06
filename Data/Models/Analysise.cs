using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Analysise
{
    public long AnalysisId { get; set; }

    public string AnalysisName { get; set; } = null!;

    public int? AnalysisDepId { get; set; }

    public string AnalysisCodeName { get; set; } = null!;

    public string AnalysisNomenclatureCode { get; set; } = null!;

    public virtual AnalysisDepartment? AnalysisDep { get; set; }

    public virtual ICollection<AnalysisWork> AnalysisWorks { get; set; } = new List<AnalysisWork>();

    public virtual ICollection<BarcodeAnalysise> BarcodeAnalysises { get; set; } = new List<BarcodeAnalysise>();

    public virtual ICollection<ContractAnalysise> ContractAnalysises { get; set; } = new List<ContractAnalysise>();

    public virtual ICollection<AnalysesTemplate> AnalysisTemps { get; set; } = new List<AnalysesTemplate>();

    public virtual ICollection<AnalysisComplex> Complexes { get; set; } = new List<AnalysisComplex>();
}
