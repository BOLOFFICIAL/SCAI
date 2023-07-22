using System;
using System.Collections.Generic;

namespace SCAI.Models.Tables;

public partial class Result
{
    public int ResultsId { get; set; }

    public int FkPatientId { get; set; }

    public string? SkinPhoto { get; set; }

    public string Diagnosis { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Patient FkPatient { get; set; } = null!;
}
