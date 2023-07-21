using Microsoft.AspNetCore.Mvc;
using SCAI.Models.Tables;
using System.Text;
using System.Security.Cryptography;

namespace SCAI.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Doctor model) 
        { 
            if (ModelState.IsValid) 
            {
                try
                {
                    using (var dbContext = new ScaiDbContext())
                    {
                        var existingDoctor = dbContext.Doctors.FirstOrDefault(d => d.Username == model.Username);
                        if (existingDoctor != null)
                        {
                            ModelState.AddModelError("Username", "Логин уже используется. Пожалуйста, выберите другой логин.");
                            return View(model); // Возвращаем представление с ошибкой
                        }
                        // Создаем объект Doctor на основе данных из модели RegistrationViewModel
                        Doctor newDoctor = new Doctor
                        {
                            DoctorsLastName = model.DoctorsLastName,
                            DoctorsFirstName = model.DoctorsFirstName,
                            DoctorsMiddleName = model.DoctorsMiddleName,
                            DoctorsPhoto = model.DoctorsPhoto,
                            JobPosition = model.JobPosition,
                            Username = model.Username,
                            UserPassword = HashPassword(model.UserPassword) // В реальном приложении следует хешировать пароль перед сохранением
                        };

                        // Добавляем объект Doctor в контекст базы данных и сохраняем изменения
                        dbContext.Doctors.Add(newDoctor);
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View();
        }

        // Метод для хеширования пароля
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Конвертируем пароль в массив байтов и хешируем его
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Возвращаем хеш-значение в виде строки шестнадцатеричных символов
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
//Scaffold-DbContext "Host=localhost;Port=5432;Database=SCAI_DB;Username=postgres;Password=123321" Npgsql.EntityFrameworkCore.PostgreSQL
