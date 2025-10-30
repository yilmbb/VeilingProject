using Microsoft.EntityFrameworkCore;
using VeilingAPI.Data;
using VeilingAPI.Models;

namespace VeilingAPI.Repositories
{
    public class GebruikerRepository : IGebruikerRepository
    {
        private readonly VeilingDbContext _context;

        public GebruikerRepository(VeilingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Gebruiker>> GetAllAsync()
        {
            return await _context.Gebruikers
                .ToListAsync();
        }

        public async Task<Gebruiker?> GetByIdAsync(int id)
        {
            return await _context.Gebruikers
                .FirstOrDefaultAsync(g => g.gebruiker_id == id);
        }

        public async Task<Gebruiker?> GetByEmailAsync(string email)
        {
            return await _context.Gebruikers
                .FirstOrDefaultAsync(g => g.email == email);
        }

        public async Task<Gebruiker> AddAsync(Gebruiker gebruiker)
        {
            await _context.Gebruikers.AddAsync(gebruiker);
            await SaveChangesAsync();
            return gebruiker;
        }

        public async Task<Gebruiker> UpdateAsync(Gebruiker gebruiker)
        {
            _context.Entry(gebruiker).State = EntityState.Modified;
            await SaveChangesAsync();
            return gebruiker;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var gebruiker = await GetByIdAsync(id);
            if (gebruiker == null)
            {
                return false;
            }

            _context.Gebruikers.Remove(gebruiker);
            await SaveChangesAsync();
            return true;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
