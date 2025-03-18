using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[Route("api/restocks")]
[ApiController]
public class RestockController : ControllerBase
{
    private readonly MongoDBService _mongoDBService;

    public RestockController(MongoDBService mongoDBService)
    {
        _mongoDBService = mongoDBService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Restock>>> GetRestocks()
    {
        try
        {
            var restocks = await _mongoDBService
                .GetCollection<Restock>("Restocks")
                .Find(_ => true)
                .ToListAsync();
            return Ok(restocks);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Restock>> PostRestock([FromBody] Restock restock)
    {
        if (restock == null)
        {
            return BadRequest("Restock data is null");
        }

        try
        {
            var collection = _mongoDBService.GetCollection<Restock>("Restocks");
            await collection.InsertOneAsync(restock);
            return CreatedAtAction(nameof(GetRestocks), new { id = restock.id }, restock);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRestock(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Invalid restock ID.");
        }

        var collection = _mongoDBService.GetCollection<Restock>("Restocks");
        var result = await collection.DeleteOneAsync(r => r.id == id);

        if (result.DeletedCount == 0)
        {
            return NotFound("Restock non trouvé");
        }

        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAllRestocks()
    {
        try
        {
            var collection = _mongoDBService.GetCollection<Restock>("Restocks");
            var result = await collection.DeleteManyAsync(_ => true);

            return Ok($"Deleted {result.DeletedCount} restocks");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
