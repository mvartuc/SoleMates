using Microsoft.AspNetCore.Mvc;
using SoleMates.Interfaces;
using SoleMates.Models;
using SoleMates.Repository;

namespace SoleMates.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;

        public RaceController(IRaceRepository raceRepository)
        {
            this._raceRepository = raceRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceRepository.GetAllAsync();
            return View(races);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceRepository.GetByIdAsync(id);
            return View(race);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Race race)
        {
            if (!ModelState.IsValid)
            {
                return View(race);
            }
            _raceRepository.Add(race);
            return RedirectToAction("Index");
        }
    }
}
