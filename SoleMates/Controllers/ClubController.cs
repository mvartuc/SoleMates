﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoleMates.Data;
using SoleMates.Interfaces;
using SoleMates.Models;
using SoleMates.ViewModels;

namespace SoleMates.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            this._clubRepository = clubRepository;
            this._photoService = photoService;
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAllAsync();
            return View(clubs);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubRepository.GetByIdAsync(id);
            return View(club);
        }

        public IActionResult Create()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel
            {
                AppUserId = currentUserId
            };
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubViewModel.Image);
                var club = new Club
                {
                    Title = clubViewModel.Title,
                    Description = clubViewModel.Description,
                    Image = result.Url.ToString(),
                    AppUserId = clubViewModel.AppUserId,
                    ClubCategory = clubViewModel.ClubCategory,
                    Address = new Address
                    {
                        Street = clubViewModel.Address.Street,
                        City = clubViewModel.Address.City,
                        State= clubViewModel.Address.State
                    }
                };
                _clubRepository.Add(club);
                if (User.IsInRole("admin"))
                    return RedirectToAction("Clubs", "Admin");
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed");
                return View(clubViewModel);
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            if (club == null) return View("Error");
            var clubViewModel = new EditClubViewModel
            {
                AppUserId = club.AppUserId,
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                ImageUrl = club.Image,
                ClubCategory = club.ClubCategory
            };
            return View(clubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubViewModel)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubViewModel);
            }
            var userClub = await _clubRepository.GetByIdNoTrackingAsync(id);

            if(userClub !=null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubViewModel);
                }
                var photoResult = await _photoService.AddPhotoAsync(clubViewModel.Image);
                var club = new Club
                {
                    Id = id,
                    Title = clubViewModel.Title,
                    Description = clubViewModel.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = clubViewModel.AddressId,
                    Address = clubViewModel.Address,
                    ClubCategory = clubViewModel.ClubCategory,
                    AppUserId = clubViewModel.AppUserId,
                    DateCreated = userClub.DateCreated
                };

                _clubRepository.Update(club);
                if (User.IsInRole("admin"))
                    return RedirectToAction("Clubs", "Admin");
                return RedirectToAction("Index");
            }
            else
            {
                return View(clubViewModel);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");
            return View(clubDetails);
        }

        [HttpPost, ActionName("DeleteClub")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");

            _clubRepository.Delete(clubDetails);
            if (User.IsInRole("admin"))
                return RedirectToAction("Clubs", "Admin");
            return RedirectToAction("Index");
        }
    }
}
