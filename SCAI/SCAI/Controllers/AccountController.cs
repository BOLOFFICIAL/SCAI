using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using SCAI.Models.Tables;

namespace SCAI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Doctor> _userManager;
        private readonly SignInManager<Doctor> _signInManager;

        public AccountController(UserManager<Doctor> userManager, SignInManager<Doctor> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Doctor model)
        {
            if (ModelState.IsValid)
            {
                Doctor doctor = new Doctor
                {
                    DoctorsFirstName = model.DoctorsFirstName,
                    DoctorsLastName = model.DoctorsLastName,
                    DoctorsMiddleName = model.DoctorsMiddleName,
                    DoctorsPhoto = model.DoctorsPhoto,
                    JobPosition = model.JobPosition,
                    Username = model.Username,
                    UserPassword = model.UserPassword
                };
                IdentityResult result = await _userManager.CreateAsync(doctor, model.UserPassword);

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
    }
}
