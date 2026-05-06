using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Contract
{
    public long ContractId { get; set; }

    public string ContractName { get; set; } = null!;

    public int ContractMoney { get; set; }

    public double ContractRemainsMoney { get; set; }

    public virtual ICollection<ContractAnalysise> ContractAnalysises { get; set; } = new List<ContractAnalysise>();

    public virtual ICollection<ContractComplex> ContractComplexes { get; set; } = new List<ContractComplex>();

    public virtual ICollection<LpuContract> LpuContracts { get; set; } = new List<LpuContract>();
}
