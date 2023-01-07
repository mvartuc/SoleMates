using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoleMates.Data;
using SoleMates.Interfaces;

namespace SoleMates.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;
        public AdminController(IAdminRepository adminRepository)
        {
            this._adminRepository = adminRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Users()
        {
            var users = await _adminRepository.GetAllUsers();
            return View(users);
        }

        public async Task<IActionResult> Clubs()
        {
            var clubs = await _adminRepository.GetAllClubs();
            return View(clubs);
        }

        public async Task<IActionResult> Races()
        {
            var races = await _adminRepository.GetAllRaces();
            return View(races);
        }
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _adminRepository.GetUserById(id);
            _adminRepository.DeleteUser(user);
            return RedirectToAction("Users", "Admin");
        }
    }
}
