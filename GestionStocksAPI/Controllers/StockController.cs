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
    [HttpPut]
    public async Task<ActionResult<Beers>> PutBeer(Beers beer)
    {
        if (beer == null || string.IsNullOrEmpty(beer.id))
        {
            return BadRequest("Invalid beer data.");
        }

        var collection = _mongoDBService.GetCollection<Beers>("Beers");
        collection.ReplaceOne(b => b.id == beer.id, beer);

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
