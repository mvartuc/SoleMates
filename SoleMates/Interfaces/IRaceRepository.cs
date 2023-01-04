using SoleMates.Models;

namespace SoleMates.Interfaces
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAllAsync();
        Task<Race> GetByIdAsync(int id);
        Task<Race> GetByIdNoTrackingAsync(int id);
        Task<IEnumerable<Race>> GetAllRacesByCityAsync(string city);
        bool Add(Race race);
        bool Update(Race race);
        bool Delete(Race race);
        bool Save();
    }
}
