using Microsoft.AspNetCore.Mvc;
using VeilingAPI.Models;
using VeilingAPI.Repositories;

namespace VeilingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IGebruikerRepository _gebruikerRepository;

        public TestController(IGebruikerRepository gebruikerRepository)
        {
            _gebruikerRepository = gebruikerRepository;
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<Gebruiker>>> GetAllUsers()
        {
            try
            {
                var users = await _gebruikerRepository.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving users", error = ex.Message });
            }
        }

        [HttpPost("create-test-user")]
        public async Task<ActionResult<Gebruiker>> CreateTestUser()
        {
            try
            {
                // Create a test Verkoper (seller)
                var testUser = new Verkoper
                {
                    wachtwoord = "TestPassword123!",
                    email = "test_" + DateTime.Now.Ticks + "@example.com",
                    registreren = DateTime.Now,
                    bedrijfsnaam = "Test Bedrijf BV",
                    kvk = "12345678",
                    btw_nummer = "NL123456789B01",
                    telefoon = "0612345678",
                    bedrijfsadres = "Teststraat 123, 1234AB Amsterdam",
                    rekening_nummer = "NL01BANK0123456789"
                };

                var createdUser = await _gebruikerRepository.AddAsync(testUser);
                return CreatedAtAction(nameof(GetAllUsers), new { id = createdUser.gebruiker_id }, createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating user", error = ex.Message });
            }
        }
    }
}
