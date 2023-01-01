using Microsoft.AspNetCore.Mvc;

namespace SoleMates.Controllers
{
    public class ClubController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
