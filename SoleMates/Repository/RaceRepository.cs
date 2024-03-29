﻿using Microsoft.EntityFrameworkCore;
using SoleMates.Data;
using SoleMates.Interfaces;
using SoleMates.Models;

namespace SoleMates.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private ApplicationDbContext _context;

        public RaceRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public bool Add(Race race)
        {
            _context.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _context.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAllAsync()
        {
            return await _context.Races.ToListAsync();
        }

        public async Task<IEnumerable<Race>> GetAllRacesByCityAsync(string city)
        {
            return await _context.Races.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Race> GetByIdAsync(int id)
        {
            return await _context.Races.Include(i => i.Address).Include(u => u.AppUser).FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<Race> GetByIdNoTrackingAsync(int id)
        {
            return await _context.Races.Include(i => i.Address).Include(u => u.AppUser).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Race race)
        {
            _context.Update(race);
            return Save();
        }
    }
}
