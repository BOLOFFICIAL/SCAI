using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SCAI.Models;
using System.Diagnostics;

namespace SCAI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;

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
        public IActionResult Index(IFormFile imageFile)
        {
            
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    if (ValidationFile(imageFile.FileName)) 
                    {
                        throw new Exception("Фаил не является картинкой");
                    }
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "files");
                    if (!Directory.Exists(imagePath))
                    {
                        Directory.CreateDirectory(imagePath);
                    }
                    string filePath = Path.Combine(imagePath, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }
                    TempData["ResultMessage"] = "Фото сохранено на сервере с названием " + uniqueFileName;
                    return RedirectPermanent("~/Home/Result");
                }
                catch(Exception ex)
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
            ViewBag.Message = TempData["ResultMessage"] as string;
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