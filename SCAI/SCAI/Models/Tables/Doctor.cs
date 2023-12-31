﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCAI.Models.Tables;

public partial class Doctor
{
    public int DoctorsId { get; set; }

    [Required(ErrorMessage = "Пожалуйста, введите фамилию")]
    [StringLength(50, ErrorMessage = "Фамилия должна содержать не более 50 символов")]
    public string DoctorsLastName { get; set; } = null!;

    [Required(ErrorMessage = "Пожалуйста, введите имя")]
    [StringLength(50, ErrorMessage = "Имя должно содержать не более 50 символов")]
    public string DoctorsFirstName { get; set; } = null!;

    [StringLength(50, ErrorMessage = "Отчество должно содержать не более 50 символов")]
    public string? DoctorsMiddleName { get; set; }

    public string? DoctorsPhoto { get; set; }

    [Required(ErrorMessage = "Пожалуйста, введите должность")]
    [StringLength(50, ErrorMessage = "Должность должна содержать не более 50 символов")]
    public string JobPosition { get; set; } = null!;

    [Required(ErrorMessage = "Пожалуйста, введите логин")]
    [StringLength(50, ErrorMessage = "Логин должен содержать не более 50 символов")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Пожалуйста, введите пароль")]
    [StringLength(16, MinimumLength = 6,  ErrorMessage = "Пароль должен содержать от 6 до 16 символов")]
    public string UserPassword { get; set; } = null!;

    [Required(ErrorMessage = "Пожалуйста, введите логин")]
    [DataType(DataType.Password)]
    [NotMapped]
    [Compare("UserPassword", ErrorMessage ="Пароли не совпадают")]

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
