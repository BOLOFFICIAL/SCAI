using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SCAI.Models;
using Skin_Cancer;
using System.Diagnostics;

namespace SCAI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;
        private ResultData _result;

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
                    TempData["Result"] = JsonConvert.SerializeObject(new ResultData(analise.AnalisePhoto(filePath)));
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
            var result = JsonConvert.DeserializeObject<ResultData>(TempData["Result"] as string);
            ViewBag.BestValue = result.BestValue;
            ViewBag.BestClass = SkinCancers.Cancers[result.BestClass]; 
            ViewBag.ResultMessage = result.AllResults; 
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