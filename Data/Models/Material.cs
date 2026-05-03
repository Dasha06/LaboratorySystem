using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Material
{
    public int MaterialId { get; set; }

    public string MaterialName { get; set; } = null!;

    public virtual ICollection<Analysise> Analysises { get; set; } = new List<Analysise>();

    public virtual ICollection<BarcodeMaterial> BarcodeMaterials { get; set; } = new List<BarcodeMaterial>();

    public virtual ICollection<PatternAnalysise> PatternAnalysises { get; set; } = new List<PatternAnalysise>();
}
