using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[Route("api/ventes")]
[ApiController]
public class VentesController : ControllerBase
{
    private readonly MongoDBService _mongoDBService;

    public VentesController(MongoDBService mongoDBService)
    {
        _mongoDBService = mongoDBService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Ventes>>> GetVentes()
    {
        try
        {
            var ventes = await _mongoDBService
                .GetCollection<Ventes>("Ventes")
                .Find(_ => true)
                .ToListAsync();
            return Ok(ventes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Ventes>> PostVentes([FromBody] Ventes vente)
    {
        if (vente == null)
        {
            Console.WriteLine($"vente == null true !!");
            return BadRequest("Vente data is null");
        }
        try
        {
            var collection = _mongoDBService.GetCollection<Ventes>("Ventes");
            await collection.InsertOneAsync(vente);
            return CreatedAtAction(nameof(GetVentes), new { id = vente.id }, vente);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
