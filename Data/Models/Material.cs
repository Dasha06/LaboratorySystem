using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Material
{
    public int MaterialId { get; set; }

    public string MaterialName { get; set; } = null!;

    public virtual ICollection<AnalysisWork> AnalysisWorks { get; set; } = new List<AnalysisWork>();

    public virtual ICollection<BarcodeMaterial> BarcodeMaterials { get; set; } = new List<BarcodeMaterial>();
}
