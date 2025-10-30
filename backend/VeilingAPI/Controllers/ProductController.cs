using Microsoft.AspNetCore.Mvc;
using VeilingAPI.DTOs;
using VeilingAPI.Models;
using VeilingAPI.Services;

namespace VeilingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IGebruikerService _gebruikerService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, IGebruikerService gebruikerService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _gebruikerService = gebruikerService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
        public async Task<ActionResult<ProductResponse>> MaakProductAan([FromBody] ProductCreateRequest request)
        {
            try
            {
                var verkoper = await _gebruikerService.GetGebruikerByEmailAsync(request.verkoperEmail) as Verkoper;
                if (verkoper == null)
                {
                    return BadRequest(new { message = "Verkoper niet gevonden op basis van email" });
                }

                var nieuwProduct = new Product
                {
                    naam = request.naam,
                    beschrijving = request.beschrijving,
                    prijs = request.prijs,
                    voorraad = request.voorraad,
                    verkoper_id = verkoper.gebruiker_id
                };

                var opgeslagen = await _productService.MaakProductAanAsync(nieuwProduct);
                var response = new ProductResponse
                {
                    product_id = opgeslagen.product_id,
                    naam = opgeslagen.naam,
                    beschrijving = opgeslagen.beschrijving,
                    prijs = opgeslagen.prijs,
                    voorraad = opgeslagen.voorraad,
                    aangemaakt_op = opgeslagen.aangemaakt_op
                };

                return CreatedAtAction(nameof(GetMijnProducten), new { email = request.verkoperEmail }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij aanmaken product");
                return StatusCode(500, new { message = "Er is een fout opgetreden bij het aanmaken van het product" });
            }
        }

        [HttpGet("mijn")]
        [ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetMijnProducten([FromQuery] string email)
        {
            try
            {
                var verkoper = await _gebruikerService.GetGebruikerByEmailAsync(email) as Verkoper;
                if (verkoper == null)
                {
                    return BadRequest(new { message = "Verkoper niet gevonden op basis van email" });
                }

                var producten = await _productService.GetProductenVoorVerkoperAsync(verkoper.gebruiker_id);
                var response = producten.Select(p => new ProductResponse
                {
                    product_id = p.product_id,
                    naam = p.naam,
                    beschrijving = p.beschrijving,
                    prijs = p.prijs,
                    voorraad = p.voorraad,
                    aangemaakt_op = p.aangemaakt_op
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen producten voor verkoper");
                return StatusCode(500, new { message = "Er is een fout opgetreden bij het ophalen van de producten" });
            }
        }
    }
}


