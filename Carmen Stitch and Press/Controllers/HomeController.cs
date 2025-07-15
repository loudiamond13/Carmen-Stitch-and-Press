using System.Diagnostics;
using Carmen_Stitch_and_Press.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carmen_Stitch_and_Press.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var imageDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "approved_layouts");

            var imageFiles = Directory.GetFiles(imageDir)
                .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                            f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                            f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                .Select(f => "/images/approved_layouts/" + Path.GetFileName(f))
                .ToList();

            List<string> images = imageFiles;

            return View(images);
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
