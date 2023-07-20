using System;
using System.Collections.Generic;

namespace SCAI.Models.Tables;

public partial class Appointment
{
    public int AppointmentsId { get; set; }

    public int FkDoctorId { get; set; }

    public int FkPatientId { get; set; }

    public int FkResultId { get; set; }

    public virtual Doctor FkDoctor { get; set; } = null!;

    public virtual Patient FkPatient { get; set; } = null!;

    public virtual Result FkResult { get; set; } = null!;
}
