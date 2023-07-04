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

        private bool IsSupportedFormat(IFormFile file)
        {
            string[] allowedFormats = { ".jpg", ".jpeg", ".png" };
            string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedFormats.Contains(fileExtension);
        }

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
            if (!IsSupportedFormat(imageFile))
            {
                TempData["ErrorMessage"] = "Выберите файл в формате JPG, JPEG или PNG.";
                return RedirectToAction("Index");
            }
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    // Генерируем уникальное имя файла
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

                    // Определяем путь к папке, где будут сохранены изображения на сервере
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "files");

                    // Создаем папку, если она не существует
                    if (!Directory.Exists(imagePath))
                    {
                        Directory.CreateDirectory(imagePath);
                    }

                    // Сохраняем файл на сервере
                    string filePath = Path.Combine(imagePath, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }
                    // Возвращаем сообщение об успешной загрузке
                    return Ok("Image uploaded successfully.");
                    /*var prediction = "Тут будет предикт"; //Когда будет всунута нейронка, это надо будет раскомментить и реализовать
                    return View("Result", prediction);*/
                }
                catch (Exception ex)
                {
                    // Если произошла ошибка при сохранении файла, обрабатываем ее
                    return BadRequest("Error uploading image: " + ex.Message);
                }
            }
            else
            {
                return BadRequest("Error uploading image");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}