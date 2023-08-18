using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCAI.Models.Tables;

public class DoctorRegistration
{
    public int DoctorsId { get; set; }

    [DisplayName("Фамилия врача:")]
    [Required(ErrorMessage = "Пожалуйста, введите фамилию")]
    [StringLength(50, ErrorMessage = "Фамилия должна содержать не более 50 символов")]
    public string DoctorsLastName { get; set; } = null!;

    [DisplayName("Имя врача:")]
    [Required(ErrorMessage = "Пожалуйста, введите имя")]
    [StringLength(50, ErrorMessage = "Имя должно содержать не более 50 символов")]
    public string DoctorsFirstName { get; set; } = null!;

    [DisplayName("Отчество врача (если имеется):")]
    [StringLength(50, ErrorMessage = "Отчество должно содержать не более 50 символов")]
    public string? DoctorsMiddleName { get; set; }

    [DisplayName("Фото врача:")]
    public string? DoctorsPhoto { get; set; }

    [DisplayName("Должность врача:")]
    [Required(ErrorMessage = "Пожалуйста, введите должность")]
    [StringLength(50, ErrorMessage = "Должность должна содержать не более 50 символов")]
    public string JobPosition { get; set; } = null!;

    [DisplayName("Логин:")]
    [Required(ErrorMessage = "Пожалуйста, введите логин")]
    [StringLength(50, ErrorMessage = "Логин должен содержать не более 50 символов")]
    public string Username { get; set; } = null!;

    [DisplayName("Пароль:")]
    [Required(ErrorMessage = "Пожалуйста, введите пароль")]
    [StringLength(16, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 16 символов")]
    public string UserPassword { get; set; } = null!;

    [DisplayName("Подтверждение пароля:")]
    [Required(ErrorMessage = "Пожалуйста, подтвердите пароль")]
    [DataType(DataType.Password)]
    [NotMapped]
    [Compare("UserPassword", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}

public class DoctorLogin
{
    [DisplayName("Логин")]
    [Required(ErrorMessage = "Пожалуйста, введите логин")] // Логин врача
    [StringLength(50, ErrorMessage = "Логин должен содержать не более 50 символов")]
    public string Username { get; set; } = null!;

    [DisplayName("Пароль")]
    [Required(ErrorMessage = "Пожалуйста, введите пароль")] // Пароль врача
    [StringLength(16, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 16 символов")]
    public string UserPassword { get; set; } = null!;
}