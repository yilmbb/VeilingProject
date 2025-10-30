using VeilingAPI.Models;

namespace VeilingAPI.Repositories
{
    public interface IGebruikerRepository
    {
        Task<IEnumerable<Gebruiker>> GetAllAsync();
        Task<Gebruiker?> GetByIdAsync(int id);
        Task<Gebruiker?> GetByEmailAsync(string email);
        Task<Gebruiker> AddAsync(Gebruiker gebruiker);
        Task<Gebruiker> UpdateAsync(Gebruiker gebruiker);
        Task<bool> DeleteAsync(int id);
        Task<int> SaveChangesAsync();
    }
}
