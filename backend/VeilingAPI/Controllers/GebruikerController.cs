using Microsoft.AspNetCore.Mvc;
using VeilingAPI.Models;
using VeilingAPI.Services;
using VeilingAPI.DTOs;

namespace VeilingAPI.Controllers
{
    /// <summary>
    /// Controller voor gebruikersbeheer
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GebruikerController : ControllerBase
    {
        private readonly IGebruikerService _gebruikerService;
        private readonly ILogger<GebruikerController> _logger;

        public GebruikerController(IGebruikerService gebruikerService, ILogger<GebruikerController> logger)
        {
            _gebruikerService = gebruikerService;
            _logger = logger;
        }

        /// <summary>
        /// Helper methode om Gebruiker om te zetten naar GebruikerResponseDto
        /// Detecteert automatisch of het een Koper of Verkoper is
        /// </summary>
        private GebruikerResponseDto MapToDto(Gebruiker gebruiker)
        {
            var dto = new GebruikerResponseDto
            {
                gebruiker_id = gebruiker.gebruiker_id,
                email = gebruiker.email,
                wachtwoord = gebruiker.wachtwoord,
                registreren = gebruiker.registreren,
                inloggen = gebruiker.inloggen,
                username = gebruiker.Username
            };

            // Detecteer type op basis van de concrete class
            if (gebruiker is Verkoper verkoper)
            {
                dto.Type = "Verkoper";
                dto.bedrijfsnaam = verkoper.bedrijfsnaam;
                dto.kvk = verkoper.kvk;
                dto.btw_nummer = verkoper.btw_nummer;
                dto.telefoon = verkoper.telefoon;
                dto.rekening_nummer = verkoper.rekening_nummer;
                dto.bedrijfsadres = verkoper.bedrijfsadres;
            }
            else if (gebruiker is Koper koper)
            {
                dto.Type = "Koper";
                dto.bedrijfsnaam = koper.bedrijfsnaam;
                dto.kvk = koper.kvk;
                dto.btw_nummer = koper.btw_nummer;
                dto.telefoon = koper.telefoon;
                dto.rekening_nummer = koper.rekening_nummer;
                dto.leveradres = koper.leveradres;
            }
            else
            {
                // Fallback als het een basis Gebruiker is (zou niet moeten gebeuren)
                dto.Type = "Unknown";
            }

            return dto;
        }

        /// <summary>
        /// Haal alle gebruikers op
        /// </summary>
        /// <returns>Lijst met alle gebruikers</returns>
        /// <response code="200">Retourneert lijst met gebruikers</response>
        /// <response code="500">Er is een interne serverfout opgetreden</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Gebruiker>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Gebruiker>>> GetAllGebruikers()
        {
            try
            {
                _logger.LogInformation("Ophalen van alle gebruikers");
                var gebruikers = await _gebruikerService.GetAllGebruikersAsync();
                return Ok(gebruikers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van alle gebruikers");
                return StatusCode(500, new { message = "Er is een fout opgetreden bij het ophalen van gebruikers", error = ex.Message });
            }
        }

        /// <summary>
        /// Haal een specifieke gebruiker op via ID
        /// </summary>
        /// <param name="id">Het gebruiker ID</param>
        /// <returns>De gevonden gebruiker</returns>
        /// <response code="200">Retourneert de gevonden gebruiker</response>
        /// <response code="404">Gebruiker niet gevonden</response>
        /// <response code="400">Ongeldig ID opgegeven</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Gebruiker), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Gebruiker>> GetGebruikerById(int id)
        {
            try
            {
                _logger.LogInformation("Ophalen van gebruiker met ID: {Id}", id);
                var gebruiker = await _gebruikerService.GetGebruikerByIdAsync(id);

                if (gebruiker == null)
                {
                    _logger.LogWarning("Gebruiker met ID {Id} niet gevonden", id);
                    return NotFound(new { message = $"Gebruiker met ID {id} niet gevonden" });
                }

                return Ok(gebruiker);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Ongeldig ID: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van gebruiker met ID: {Id}", id);
                return StatusCode(500, new { message = "Er is een fout opgetreden", error = ex.Message });
            }
        }

        /// <summary>
        /// Registreer een nieuwe gebruiker (Koper of Verkoper)
        /// </summary>
        /// <param name="request">De registratie gegevens met Type indicator</param>
        /// <returns>De nieuw aangemaakte gebruiker met Type indicator</returns>
        /// <response code="201">Gebruiker succesvol aangemaakt</response>
        /// <response code="400">Ongeldige gegevens of gebruiker bestaat al</response>
        /// <response code="500">Er is een interne serverfout opgetreden</response>
        [HttpPost]
        [ProducesResponseType(typeof(GebruikerResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GebruikerResponseDto>> RegistreerGebruiker([FromBody] RegistratieRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Ongeldige model state bij registratie");
                    return BadRequest(ModelState);
                }

                // Valideer Type
                if (request.Type != "Koper" && request.Type != "Verkoper")
                {
                    return BadRequest(new { message = "Type moet 'Koper' of 'Verkoper' zijn" });
                }

                _logger.LogInformation("Registreren van nieuwe {Type} met email: {Email}", request.Type, request.email);

                // Maak het juiste type object aan op basis van request.Type
                Gebruiker gebruiker;

                if (request.Type == "Verkoper")
                {
                    gebruiker = new Verkoper
                    {
                        email = request.email,
                        wachtwoord = request.wachtwoord,
                        bedrijfsnaam = request.bedrijfsnaam,
                        kvk = request.kvk,
                        btw_nummer = request.btw_nummer,
                        telefoon = request.telefoon,
                        bedrijfsadres = request.adres, // Verkoper heeft bedrijfsadres
                        rekening_nummer = request.rekening_nummer
                    };
                }
                else // Koper
                {
                    gebruiker = new Koper
                    {
                        email = request.email,
                        wachtwoord = request.wachtwoord,
                        bedrijfsnaam = request.bedrijfsnaam,
                        kvk = request.kvk,
                        btw_nummer = request.btw_nummer,
                        telefoon = request.telefoon,
                        leveradres = request.adres, // Koper heeft leveradres
                        rekening_nummer = request.rekening_nummer
                    };
                }

                // Registreer de gebruiker (EF Core zet automatisch de juiste discriminator)
                var nieuweGebruiker = await _gebruikerService.RegistreerGebruikerAsync(gebruiker);

                _logger.LogInformation("Gebruiker succesvol geregistreerd met ID: {Id} als {Type}",
                    nieuweGebruiker.gebruiker_id, request.Type);

                // Map naar DTO met Type indicator
                var responseDto = MapToDto(nieuweGebruiker);

                return CreatedAtAction(
                    nameof(GetGebruikerById),
                    new { id = nieuweGebruiker.gebruiker_id },
                    responseDto
                );
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validatiefout bij registratie");
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Business logic fout bij registratie");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij registreren van gebruiker");
                return StatusCode(500, new { message = "Er is een fout opgetreden bij het registreren", error = ex.Message });
            }
        }

        /// <summary>
        /// Update een bestaande gebruiker
        /// </summary>
        /// <param name="id">Het gebruiker ID</param>
        /// <param name="request">De bijgewerkte gebruiker gegevens</param>
        /// <returns>De bijgewerkte gebruiker met Type indicator</returns>
        /// <response code="200">Gebruiker succesvol bijgewerkt</response>
        /// <response code="400">Ongeldige gegevens of ID komt niet overeen</response>
        /// <response code="404">Gebruiker niet gevonden</response>
        /// <response code="500">Er is een interne serverfout opgetreden</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GebruikerResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GebruikerResponseDto>> UpdateGebruiker(int id, [FromBody] UpdateGebruikerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Ongeldige model state bij update");
                    return BadRequest(ModelState);
                }

                if (id != request.gebruiker_id)
                {
                    _logger.LogWarning("ID mismatch: URL ID {UrlId} != Body ID {BodyId}", id, request.gebruiker_id);
                    return BadRequest(new { message = "ID in URL komt niet overeen met ID in body" });
                }

                _logger.LogInformation("Updaten van gebruiker met ID: {Id}", id);

                // Haal de bestaande gebruiker op
                var bestaandeGebruiker = await _gebruikerService.GetGebruikerByIdAsync(id);

                if (bestaandeGebruiker == null)
                {
                    _logger.LogWarning("Gebruiker met ID {Id} niet gevonden", id);
                    return NotFound(new { message = $"Gebruiker met ID {id} niet gevonden" });
                }

                // Update de gemeenschappelijke velden
                bestaandeGebruiker.email = request.email;
                bestaandeGebruiker.wachtwoord = request.wachtwoord;

                // Update type-specifieke velden op basis van het type gebruiker
                if (bestaandeGebruiker is Verkoper verkoper)
                {
                    verkoper.bedrijfsnaam = request.bedrijfsnaam;
                    verkoper.kvk = request.kvk;
                    verkoper.btw_nummer = request.btw_nummer;
                    verkoper.telefoon = request.telefoon;
                    verkoper.rekening_nummer = request.rekening_nummer;
                    verkoper.bedrijfsadres = request.bedrijfsadres ?? string.Empty;
                }
                else if (bestaandeGebruiker is Koper koper)
                {
                    koper.bedrijfsnaam = request.bedrijfsnaam;
                    koper.kvk = request.kvk;
                    koper.btw_nummer = request.btw_nummer;
                    koper.telefoon = request.telefoon;
                    koper.rekening_nummer = request.rekening_nummer;
                    koper.leveradres = request.leveradres ?? string.Empty;
                }

                // Update in database
                var updatedGebruiker = await _gebruikerService.UpdateGebruikerAsync(bestaandeGebruiker);

                _logger.LogInformation("Gebruiker met ID {Id} succesvol bijgewerkt", id);

                // Map naar DTO met Type indicator
                var responseDto = MapToDto(updatedGebruiker);
                return Ok(responseDto);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validatiefout bij update");
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Business logic fout bij update");

                if (ex.Message.Contains("bestaat niet"))
                {
                    return NotFound(new { message = ex.Message });
                }

                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij updaten van gebruiker met ID: {Id}", id);
                return StatusCode(500, new { message = "Er is een fout opgetreden bij het bijwerken", error = ex.Message });
            }
        }

        /// <summary>
        /// Verwijder een gebruiker
        /// </summary>
        /// <param name="id">Het gebruiker ID</param>
        /// <returns>Bevestiging van verwijdering</returns>
        /// <response code="200">Gebruiker succesvol verwijderd</response>
        /// <response code="404">Gebruiker niet gevonden</response>
        /// <response code="400">Ongeldig ID opgegeven</response>
        /// <response code="500">Er is een interne serverfout opgetreden</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteGebruiker(int id)
        {
            try
            {
                _logger.LogInformation("Verwijderen van gebruiker met ID: {Id}", id);
                var result = await _gebruikerService.VerwijderGebruikerAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Gebruiker met ID {Id} niet gevonden bij verwijderen", id);
                    return NotFound(new { message = $"Gebruiker met ID {id} niet gevonden" });
                }

                _logger.LogInformation("Gebruiker met ID {Id} succesvol verwijderd", id);
                return Ok(new { message = $"Gebruiker met ID {id} succesvol verwijderd" });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Ongeldig ID bij verwijderen: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij verwijderen van gebruiker met ID: {Id}", id);
                return StatusCode(500, new { message = "Er is een fout opgetreden bij het verwijderen", error = ex.Message });
            }
        }

        /// <summary>
        /// Zoek een gebruiker op email
        /// </summary>
        /// <param name="email">Het email adres</param>
        /// <returns>De gevonden gebruiker met Type indicator</returns>
        /// <response code="200">Retourneert de gevonden gebruiker met type</response>
        /// <response code="404">Gebruiker niet gevonden</response>
        /// <response code="400">Ongeldig email opgegeven</response>
        [HttpGet("email/{email}")]
        [ProducesResponseType(typeof(GebruikerResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GebruikerResponseDto>> GetGebruikerByEmail(string email)
        {
            try
            {
                _logger.LogInformation("Ophalen van gebruiker met email: {Email}", email);
                var gebruiker = await _gebruikerService.GetGebruikerByEmailAsync(email);

                if (gebruiker == null)
                {
                    _logger.LogWarning("Gebruiker met email {Email} niet gevonden", email);
                    return NotFound(new { message = $"Gebruiker met email {email} niet gevonden" });
                }

                // Map naar DTO met Type indicator
                var responseDto = MapToDto(gebruiker);
                return Ok(responseDto);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Ongeldig email: {Email}", email);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van gebruiker met email: {Email}", email);
                return StatusCode(500, new { message = "Er is een fout opgetreden", error = ex.Message });
            }
        }

        /// <summary>
        /// Logt een gebruiker in met email en wachtwoord
        /// </summary>
        /// <param name="loginRequest">Login gegevens</param>
        /// <returns>Gebruiker met Type (Koper/Verkoper) of foutmelding</returns>
        /// <response code="200">Login succesvol, retourneert gebruiker met type</response>
        /// <response code="400">Email en wachtwoord zijn verplicht</response>
        /// <response code="401">Ongeldig email of wachtwoord</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(GebruikerResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GebruikerResponseDto>> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Wachtwoord))
                {
                    _logger.LogWarning("Login poging met lege email of wachtwoord");
                    return BadRequest(new { message = "Email en wachtwoord zijn verplicht" });
                }

                _logger.LogInformation("Login poging voor email: {Email}", loginRequest.Email);
                var gebruiker = await _gebruikerService.GetGebruikerByEmailAsync(loginRequest.Email);

                if (gebruiker == null)
                {
                    _logger.LogWarning("Login mislukt: gebruiker met email {Email} niet gevonden", loginRequest.Email);
                    return Unauthorized(new { message = "Ongeldig email of wachtwoord" });
                }

                // Controleer wachtwoord (voor nu plaintext, maar we voegen straks hashing toe)
                if (gebruiker.wachtwoord != loginRequest.Wachtwoord)
                {
                    _logger.LogWarning("Login mislukt: onjuist wachtwoord voor email {Email}", loginRequest.Email);
                    return Unauthorized(new { message = "Ongeldig email of wachtwoord" });
                }

                // Update login datum
                gebruiker.inloggen = DateTime.Now;
                await _gebruikerService.UpdateGebruikerAsync(gebruiker);

                _logger.LogInformation("Login succesvol voor gebruiker met ID: {Id}", gebruiker.gebruiker_id);

                // Map naar DTO met Type indicator
                var responseDto = MapToDto(gebruiker);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij inloggen voor email: {Email}", loginRequest.Email);
                return StatusCode(500, new { message = "Er is een fout opgetreden bij het inloggen", error = ex.Message });
            }
        }
    }

    /// <summary>
    /// Login request model
    /// </summary>
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Wachtwoord { get; set; } = string.Empty;
    }
}
