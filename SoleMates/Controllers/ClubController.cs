using Microsoft.AspNetCore.Mvc;
using SoleMates.Data;

namespace SoleMates.Controllers
{
    public class ClubController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClubController(ApplicationDbContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            var clubs = _context.Clubs.ToList();
            return View(clubs);
        }
    }
}
