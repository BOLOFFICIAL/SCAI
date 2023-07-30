using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using SCAI.Models.Tables;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using SCAI.Models;
using Microsoft.AspNetCore.Identity;

namespace SCAI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;
            //if(claimsUser.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        

        /// <summary>
        /// Метод для регистрации в системе - в БД
        /// </summary>
        /// <param name="model">Модель с полями для регистрации в БД</param>
        /// <returns>Представление с сообщением с результатом регистрации</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(DoctorRegistration model) 
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
                        // Создаем объект Doctor на основе данных из модели Registration
                        Doctor newDoctor = new Doctor
                        {
                            DoctorsLastName = model.DoctorsLastName,
                            DoctorsFirstName = model.DoctorsFirstName,
                            DoctorsMiddleName = model.DoctorsMiddleName,
                            DoctorsPhoto = model.DoctorsPhoto,
                            JobPosition = model.JobPosition,
                            Username = model.Username,
                            UserPassword = HashPassword(model.UserPassword)
                        };

                        // Добавляем объект Doctor в контекст базы данных и сохраняем изменения
                        dbContext.Doctors.Add(newDoctor);
                        dbContext.SaveChanges();
                    }
                    TempData["RegistrationMessage"] = "Регистрация прошла успешно! Теперь вы можете войти в свою учетную запись.";
                    //return RedirectToAction("RegistrationConfirmation");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View();
        }

        /// <summary>
        /// Метод для входа в систему - проверка наличия данных пользователя в БД
        /// </summary>
        /// <param name="model">Модель с данными логина и пароля из БД</param>
        /// <returns>Представление с сообщением с результатом логина</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(DoctorLogin model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var dbContext = new ScaiDbContext())
                    {
                        // Ищем пользователя по логину
                        var doctor = dbContext.Doctors.FirstOrDefault(d => d.Username == model.Username);
                        if (doctor != null)
                        {
                            // Хешируем введенный пользователем пароль
                            string hashedPassword = HashPassword(model.UserPassword);

                            // Сравниваем хешированный пароль из базы данных с хеш-значением введенного пароля
                            if (doctor.UserPassword == hashedPassword)
                            {
                                var claims = new List<Claim> 
                                { 
                                    new Claim(ClaimTypes.NameIdentifier, model.Username) 
                                };

                                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                                AuthenticationProperties properties = new AuthenticationProperties()
                                {
                                    AllowRefresh = true
                                };

                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                                //TempData["LoginMessage"] = "Вход прошел успешно!";
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Произошла ошибка при попытке входа. Пожалуйста, попробуйте еще раз.");
                }
            }
            // Возвращаем представление с сообщением об ошибке
            ModelState.AddModelError("", "Неверный логин или пароль."); // Пользователь не найден или пароль неверный - отображаем сообщение об ошибке
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
