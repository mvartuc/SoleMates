using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoleMates.Data;
using SoleMates.Interfaces;
using SoleMates.ViewModels;

namespace SoleMates.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardController(IDashboardRepository dashboardRepository)
        {
            this._dashboardRepository = dashboardRepository;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("admin"))
                return RedirectToAction("Index", "Admin");

            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs

            };
            return View(dashboardViewModel);
        }
    }
}
