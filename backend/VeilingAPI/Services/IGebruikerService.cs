using VeilingAPI.Models;

namespace VeilingAPI.Services
{
    public interface IGebruikerService
    {
        Task<IEnumerable<Gebruiker>> GetAllGebruikersAsync();
        Task<Gebruiker?> GetGebruikerByIdAsync(int id);
        Task<Gebruiker?> GetGebruikerByEmailAsync(string email);
        Task<Gebruiker> RegistreerGebruikerAsync(Gebruiker gebruiker);
        Task<Gebruiker> UpdateGebruikerAsync(Gebruiker gebruiker);
        Task<bool> VerwijderGebruikerAsync(int id);
        Task<bool> BestaatEmailAsync(string email);
    }
}
