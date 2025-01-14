using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[Route("api/stock")]
[ApiController]
public class StocksController : ControllerBase
{
    private readonly MongoDBService _mongoDBService;

    public StocksController(MongoDBService mongoDBService)
    {
        _mongoDBService = mongoDBService;
    }

    //GET :
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Beers>>> GetBeers()
    {
        try
        {
            var beers = _mongoDBService.GetCollection<Beers>("Beers").Find(_ => true).ToList();
            return Ok(beers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST:
    [HttpPost]
    public async Task<ActionResult<Beers>> PostBeer(Beers beer)
    {
        if (beer == null || string.IsNullOrEmpty(beer.name))
        {
            return BadRequest("Invalid beer data.");
        }

        var collection = _mongoDBService.GetCollection<Beers>("Beers");
        collection.InsertOne(beer);

        return CreatedAtAction(nameof(GetBeers), new { id = beer.id }, beer);
    }

    // PUT:
    [HttpPut("{id}")]
    public async Task<ActionResult<Beers>> PutBeer(string id, Beers beer)
    {
        if (beer == null || string.IsNullOrEmpty(id))
        {
            return BadRequest("Invalid beer data.");
        }

        beer.id = id; // S'assurer que l'ID correspond au paramètre de route
        var collection = _mongoDBService.GetCollection<Beers>("Beers");
        var result = await collection.ReplaceOneAsync(b => b.id == id, beer);

        if (result.MatchedCount == 0)
        {
            return NotFound("Beer not found");
        }

        return Ok(beer);
    }

    // DELETE:
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBeer(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Invalid beer id.");
        }

        var collection = _mongoDBService.GetCollection<Beers>("Beers");
        collection.DeleteOne(b => b.id == id);

        return NoContent();
    }

    // GET: /id
    [HttpGet("{id}")]
    public async Task<ActionResult<Beers>> GetBeer(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Invalid beer id.");
        }

        var collection = _mongoDBService.GetCollection<Beers>("Beers");
        var beer = collection.Find(b => b.id == id).FirstOrDefault();

        if (beer == null)
        {
            return NotFound();
        }

        return Ok(beer);
    }
}
