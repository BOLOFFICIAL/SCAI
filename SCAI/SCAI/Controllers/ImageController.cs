using Microsoft.AspNetCore.Mvc;

namespace SCAI.Controllers
{
    public class ImageController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        //[HttpPost("/Image/Upload")]
        public IActionResult Upload(IFormFile imageFile)
        {
            //return View();
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    // Генерируем уникальное имя файла
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

                    // Определяем путь к папке, где будут сохранены изображения на сервере
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

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
                }
                catch (Exception ex)
                {
                    // Если произошла ошибка при сохранении файла, обрабатываем ее
                    return BadRequest("Error uploading image: " + ex.Message);
                }
            }
            

            // Если файл не был выбран, возвращаем ошибку
            //return BadRequest("No image file uploaded.");
            return View();
        }
    }   
}
