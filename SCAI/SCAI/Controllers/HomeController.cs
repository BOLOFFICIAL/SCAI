using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SCAI.Models;
using Skin_Cancer;
using System.Diagnostics;
using SCAI.Models.Tables;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.AspNetCore.Http;

namespace SCAI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int _lastResultId;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile imageFile)
        {
            await Task.Delay(1);
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    if (ValidationFile(imageFile.FileName))
                    {
                        throw new Exception("Файл не является картинкой");
                    }
                    var analise = new Analise();
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "files");
                    if (!Directory.Exists(imagePath))
                    {
                        Directory.CreateDirectory(imagePath);
                    }
                    var filePath = Path.Combine(imagePath, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }
                    TempData["Img"] = SCAIHelp.GetLocalFilePath(filePath);
                    TempData["Result"] = JsonConvert.SerializeObject(new ResultData(MlAnalis.Analise(filePath)));
                    return RedirectPermanent("~/Home/Result");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message.ToString();
                    return RedirectPermanent("~/Home/Error");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Не удалось загрузить файл";
                return RedirectPermanent("~/Home/Error");
            }
        }

        [HttpGet]
        public IActionResult Result(Result resultModel)
        {
            try
            {
                ViewBag.Img = TempData["Img"] as string;
                var result = JsonConvert.DeserializeObject<ResultData>(TempData["Result"] as string);
                ViewBag.BestValue = Math.Round(result.BestValue, 3).ToString() + "%";
                ViewBag.BestClass = result.BestClass;
                ViewBag.About = result.AboutCancer;
                ViewBag.ResultMessage = result.AllResults;

                using (var dbContext = new ScaiDbContext())
                {
                    Result newResult = new Result
                    {
                        SkinPhoto = ViewBag.Img,
                        Diagnosis = ViewBag.BestClass,
                        Description = ViewBag.About
                    };
                    dbContext.Results.Add(newResult);
                    dbContext.SaveChanges();

                    _lastResultId = newResult.ResultsId;
                    _httpContextAccessor.HttpContext.Session.SetInt32("LastResultId", _lastResultId);
                }
                return View();
            }
            catch
            {
                return RedirectPermanent("~/Home/Index");
            }
        }

        [HttpGet]
        public IActionResult PatientAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PatientAdd(Patient patientModel, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var dbContext = new ScaiDbContext())
                    {
                        Patient newPatient = new Patient
                        {
                            PatientsFirstName = patientModel.PatientsFirstName,
                            PatientsLastName = patientModel.PatientsLastName,
                            PatientsMiddleName = patientModel.PatientsMiddleName,
                            PatientsPhoto = patientModel.PatientsPhoto,
                            PassportData = patientModel.PassportData,
                            Age = patientModel.Age,
                            Gender = patientModel.Gender
                        };
                        dbContext.Patients.Add(newPatient);
                        dbContext.SaveChanges();
                    }
                    return View();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View();

        }

        [HttpGet]
        public IActionResult AppointmentAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AppointmentAdd(Appointment appointmentModel)
        {
            try
            {
                using (var dbContext = new ScaiDbContext())
                {
                    var user = User;
                    var doctorUsername = user.FindFirstValue(ClaimTypes.NameIdentifier);

                    // Найдите доктора по Username
                    var doctor = dbContext.Doctors.FirstOrDefault(d => d.Username == doctorUsername);
                    if (doctor != null) 
                    {
                        Appointment newAppointment = new Appointment
                        {
                            DoctorComment = appointmentModel.DoctorComment,
                            FkDoctorId = doctor.DoctorsId,
                            FkPatientId = appointmentModel.FkPatientId,
                            FkResultId = _httpContextAccessor.HttpContext.Session.GetInt32("LastResultId") ?? 1,
                        };
                        dbContext.Appointments.Add(newAppointment);
                        dbContext.SaveChanges();

                        // Сохраненная запись с текущим id
                        var savedAppointment = dbContext.Appointments.FirstOrDefault(a => a.AppointmentsId == newAppointment.AppointmentsId);

                        // Запись в ViewBag
                        ViewBag.Appointment = savedAppointment;

                        return View("AppointmentAdd");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewBag.Error = TempData["ErrorMessage"] as string;
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool ValidationFile(string fileName)
        {
            string type = Path.GetExtension(fileName);
            switch (type)
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".svg":
                    return false;
                default:
                    return true;
            }
        }
    }
}