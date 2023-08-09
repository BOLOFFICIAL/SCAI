using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCAI.Models.Tables;

public partial class Patient
{
    public int PatientsId { get; set; }
    [Required(ErrorMessage = "Пожалуйста, введите фамилию")]
    [StringLength(50, ErrorMessage = "Фамилия должна содержать не более 50 символов")]
    public string PatientsLastName { get; set; } = null!;

    [Required(ErrorMessage = "Пожалуйста, введите имя")]
    [StringLength(50, ErrorMessage = "Имя должно содержать не более 50 символов")]
    public string PatientsFirstName { get; set; } = null!;

    [StringLength(50, ErrorMessage = "Отчество должно содержать не более 50 символов")]
    public string? PatientsMiddleName { get; set; }

    public string? PatientsPhoto { get; set; }

    public string PassportData { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Возраст должен быть положительным числом.")]
    public int Age { get; set; }

    public string Gender { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
