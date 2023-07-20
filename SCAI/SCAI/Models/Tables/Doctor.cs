using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCAI.Models.Tables;

public partial class Doctor
{
    public int DoctorsId { get; set; }

    public string DoctorsLastName { get; set; } = null!;

    public string DoctorsFirstName { get; set; } = null!;

    public string? DoctorsMiddleName { get; set; }

    public string? DoctorsPhoto { get; set; }

    public string JobPosition { get; set; } = null!;

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string UserPassword { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Compare("UserPassword", ErrorMessage ="Пароли не совпадают")]
    public string ConfirmPassword { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
