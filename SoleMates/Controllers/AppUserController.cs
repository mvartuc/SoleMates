using Microsoft.AspNetCore.Mvc;

namespace SoleMates.Controllers
{
    public class AppUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }        
        public IActionResult Register()
        {
            return View();
        }
    }
}
