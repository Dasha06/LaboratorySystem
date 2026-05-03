using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Worker
{
    public int WorkerId { get; set; }

    public string WorkerFio { get; set; } = null!;

    public string WorkerPassword { get; set; } = null!;

    public string WorkerLogin { get; set; } = null!;

    public virtual ICollection<OrderChange> OrderChanges { get; set; } = new List<OrderChange>();

    public virtual ICollection<PatientChange> PatientChanges { get; set; } = new List<PatientChange>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
