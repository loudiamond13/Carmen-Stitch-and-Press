using Carmen_Stitch_and_Press.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carmen_Stitch_and_Press.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = Constants.Role_Company)]
    [Route("Company/User")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [Route("Index")]
        public IActionResult Index()
        {
            ViewData["IndexAPIUrl"] = _configuration["ApiURLs:CompUserIndex"];
            return View();
        }
    }
}
