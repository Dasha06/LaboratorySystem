using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Patient
{
    public long PatientId { get; set; }

    public string PatientFirstName { get; set; } = null!;

    public string PatientSecondName { get; set; } = null!;

    public string? PatientLastName { get; set; }

    public DateOnly? PatientBirthday { get; set; }

    public string? PatientEmail { get; set; }

    public string PatientGender { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<PatientChange> PatientChanges { get; set; } = new List<PatientChange>();
}
