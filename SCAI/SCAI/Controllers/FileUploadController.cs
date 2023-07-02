using Microsoft.AspNetCore.Mvc;

namespace SCAI.Controllers
{
    public class FileUploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
