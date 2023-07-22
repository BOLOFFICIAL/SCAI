using System;
using System.Collections.Generic;

namespace SCAI.Models.Tables;

public partial class Patient
{
    public int PatientsId { get; set; }

    public string PatientsLastName { get; set; } = null!;

    public string PatientsFirstName { get; set; } = null!;

    public string? PatientsMiddleName { get; set; }

    public string? PatientsPhoto { get; set; }

    public string PassportData { get; set; } = null!;

    public int Age { get; set; }

    public string Gender { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
