using Microsoft.AspNetCore.Mvc;
using SCAI.Models.Tables;

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
                        // Создаем объект Doctor на основе данных из модели RegistrationViewModel
                        Doctor newDoctor = new Doctor
                        {
                            DoctorsLastName = model.DoctorsLastName,
                            DoctorsFirstName = model.DoctorsFirstName,
                            DoctorsMiddleName = model.DoctorsMiddleName,
                            DoctorsPhoto = model.DoctorsPhoto,
                            JobPosition = model.JobPosition,
                            Username = model.Username,
                            UserPassword = model.UserPassword // В реальном приложении следует хешировать пароль перед сохранением
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
    }
}
