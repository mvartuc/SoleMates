using Microsoft.AspNetCore.Mvc;
using SoleMates.Interfaces;
using SoleMates.Models;
using SoleMates.Repository;
using SoleMates.Services;
using SoleMates.ViewModels;

namespace SoleMates.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;

        public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
        {
            this._raceRepository = raceRepository;
            this._photoService = photoService;
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
        public async Task<IActionResult> Create(CreateRaceViewModel raceViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceViewModel.Image);
                var race = new Race
                {
                    Title = raceViewModel.Title,
                    Description = raceViewModel.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = raceViewModel.Address.Street,
                        City = raceViewModel.Address.City,
                        State = raceViewModel.Address.State
                    }
                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed");
                return View(raceViewModel);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null) return View("Error");
            var raceViewModel = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                ImageUrl = race.Image,
                RaceCategory = race.RaceCategory
            };
            return View(raceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race");
                return View("Edit", raceViewModel);
            }
            var userRace = await _raceRepository.GetByIdNoTrackingAsync(id);

            if (userRace != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userRace.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(raceViewModel);
                }
                var photoResult = await _photoService.AddPhotoAsync(raceViewModel.Image);
                var race = new Race
                {
                    Id = id,
                    Title = raceViewModel.Title,
                    Description = raceViewModel.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = raceViewModel.AddressId,
                    Address = raceViewModel.Address
                };

                _raceRepository.Update(race);
                return RedirectToAction("Index");
            }
            else
            {
                return View(raceViewModel);
            }
        }

    }
}
