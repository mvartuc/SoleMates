using SoleMates.Models;

namespace SoleMates.Interfaces
{
    public interface IAdminRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<IEnumerable<Club>> GetAllClubs();
        Task<IEnumerable<Race>> GetAllRaces();

        Task<AppUser> GetUserById(string id);
        bool DeleteUser(AppUser user);

        bool Save();

    }
}
