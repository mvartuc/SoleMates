using Microsoft.EntityFrameworkCore;
using SoleMates.Data;
using SoleMates.Interfaces;
using SoleMates.Models;

namespace SoleMates.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        public AdminRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public bool DeleteUser(AppUser user)
        {
            _context.Users.Remove(user);
            return Save();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public async Task<IEnumerable<Club>> GetAllClubs()
        {
            return await _context.Clubs.ToListAsync();
        }
        public async Task<IEnumerable<Race>> GetAllRaces()
        {
            return await _context.Races.ToListAsync();
        }
    }
}
