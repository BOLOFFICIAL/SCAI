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