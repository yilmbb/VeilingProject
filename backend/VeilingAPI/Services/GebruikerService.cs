using VeilingAPI.Models;
using VeilingAPI.Repositories;

namespace VeilingAPI.Services
{
    public class GebruikerService : IGebruikerService
    {
        private readonly IGebruikerRepository _gebruikerRepository;

        public GebruikerService(IGebruikerRepository gebruikerRepository)
        {
            _gebruikerRepository = gebruikerRepository;
        }

        public async Task<IEnumerable<Gebruiker>> GetAllGebruikersAsync()
        {
            return await _gebruikerRepository.GetAllAsync();
        }

        public async Task<Gebruiker?> GetGebruikerByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Gebruiker ID moet groter zijn dan 0", nameof(id));
            }

            return await _gebruikerRepository.GetByIdAsync(id);
        }

        public async Task<Gebruiker?> GetGebruikerByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email mag niet leeg zijn", nameof(email));
            }

            return await _gebruikerRepository.GetByEmailAsync(email);
        }

        public async Task<Gebruiker> RegistreerGebruikerAsync(Gebruiker gebruiker)
        {
            // Validatie: Check of gebruiker niet null is
            if (gebruiker == null)
            {
                throw new ArgumentNullException(nameof(gebruiker), "Gebruiker mag niet null zijn");
            }

            // Validatie: Check of verplichte velden zijn ingevuld
            if (string.IsNullOrWhiteSpace(gebruiker.email))
            {
                throw new ArgumentException("Email is verplicht", nameof(gebruiker.email));
            }

            if (string.IsNullOrWhiteSpace(gebruiker.wachtwoord))
            {
                throw new ArgumentException("Wachtwoord is verplicht", nameof(gebruiker.wachtwoord));
            }

            // Business Logic: Check of email al bestaat
            var bestaatEmail = await BestaatEmailAsync(gebruiker.email);
            if (bestaatEmail)
            {
                throw new InvalidOperationException($"Email '{gebruiker.email}' is al geregistreerd");
            }

            // Business Logic: Zet registratie datum
            gebruiker.registreren = DateTime.Now;

            // Business Logic: Inloggen is initieel null
            gebruiker.inloggen = null;

            // Voeg gebruiker toe aan database
            return await _gebruikerRepository.AddAsync(gebruiker);
        }

        public async Task<Gebruiker> UpdateGebruikerAsync(Gebruiker gebruiker)
        {
            // Validatie: Check of gebruiker niet null is
            if (gebruiker == null)
            {
                throw new ArgumentNullException(nameof(gebruiker), "Gebruiker mag niet null zijn");
            }

            // Validatie: Check of gebruiker bestaat
            var bestaandeGebruiker = await _gebruikerRepository.GetByIdAsync(gebruiker.gebruiker_id);
            if (bestaandeGebruiker == null)
            {
                throw new InvalidOperationException($"Gebruiker met ID {gebruiker.gebruiker_id} bestaat niet");
            }

            // Business Logic: Als email is gewijzigd, check of nieuwe email al bestaat
            if (!bestaandeGebruiker.email.Equals(gebruiker.email, StringComparison.OrdinalIgnoreCase))
            {
                var emailBestaat = await BestaatEmailAsync(gebruiker.email);
                if (emailBestaat)
                {
                    throw new InvalidOperationException($"Email '{gebruiker.email}' is al in gebruik door een andere gebruiker");
                }
            }

            // Update gebruiker
            return await _gebruikerRepository.UpdateAsync(gebruiker);
        }

        public async Task<bool> VerwijderGebruikerAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Gebruiker ID moet groter zijn dan 0", nameof(id));
            }

            // Business Logic: Check of gebruiker bestaat
            var gebruiker = await _gebruikerRepository.GetByIdAsync(id);
            if (gebruiker == null)
            {
                return false;
            }

            // Verwijder gebruiker
            return await _gebruikerRepository.DeleteAsync(id);
        }

        public async Task<bool> BestaatEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var gebruiker = await _gebruikerRepository.GetByEmailAsync(email);
            return gebruiker != null;
        }
    }
}
