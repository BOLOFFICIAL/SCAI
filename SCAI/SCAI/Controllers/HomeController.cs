using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SCAI.Models;
using Skin_Cancer;
using System.Diagnostics;
using SCAI.Models.Tables;

namespace SCAI.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;
        //private ResultData _result;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
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

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Result()
        {
            try
            {
                ViewBag.Img = TempData["Img"] as string;
                var result = JsonConvert.DeserializeObject<ResultData>(TempData["Result"] as string);
                ViewBag.BestValue = Math.Round(result.BestValue, 3).ToString() + "%";
                ViewBag.BestClass = result.BestClass;
                ViewBag.About = result.AboutCancer;
                ViewBag.ResultMessage = result.AllResults;
                
                return View();
            } 
            catch 
            {
                return RedirectPermanent("~/Home/Index");
            }
        }

        [HttpGet]
        public IActionResult SaveResult()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveResult(Patient patientModel)
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

                        /*Result newResult = new Result
                        {
                            FkPatientId = patientModel.PatientsId,
                            SkinPhoto = ViewBag.Img,
                            Description = ViewBag.About,
                            Diagnosis = ViewBag.BestClass
                        };
                        dbContext.Results.Add(newResult);*/
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