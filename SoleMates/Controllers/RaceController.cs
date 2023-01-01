using Microsoft.AspNetCore.Mvc;

namespace SoleMates.Controllers
{
    public class RaceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
