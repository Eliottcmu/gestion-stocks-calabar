using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly MongoDBService _mongoDBService;
    private readonly ILogger<UserController> _logger;

    public UserController(MongoDBService mongoDBService, ILogger<UserController> logger)
    {
        _mongoDBService = mongoDBService;
        _logger = logger;
    }

    // GET: Retrieve all users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        try
        {
            var users = await _mongoDBService
                .GetCollection<User>("Users")
                .Find(_ => true)
                .ToListAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode(500, "Internal server error");
        }
    }

    // POST: Add a new user
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        if (user == null || string.IsNullOrEmpty(user.name))
        {
            return BadRequest("Invalid user data.");
        }

        var collection = _mongoDBService.GetCollection<User>("Users");
        await collection.InsertOneAsync(user);

        return CreatedAtAction(nameof(GetUsers), new { id = user.id }, user);
    }

    // PUT: Update an existing user
    [HttpPut]
    public async Task<ActionResult<User>> PutUser(User user)
    {
        if (user == null || string.IsNullOrEmpty(user.id))
        {
            return BadRequest("Invalid user data.");
        }

        var collection = _mongoDBService.GetCollection<User>("Users");
        var result = await collection.ReplaceOneAsync(u => u.id == user.id, user);

        if (result.MatchedCount == 0)
        {
            return NotFound("User not found");
        }

        return Ok(user);
    }

    // DELETE: Remove a user by ID
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Invalid user ID.");
        }

        var collection = _mongoDBService.GetCollection<User>("Users");
        var result = await collection.DeleteOneAsync(u => u.id == id);

        if (result.DeletedCount == 0)
        {
            return NotFound("User not found");
        }

        return NoContent();
    }

    // GET: Retrieve a user by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Invalid user ID.");
        }

        var collection = _mongoDBService.GetCollection<User>("Users");
        var user = await collection.Find(u => u.id == id).FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound("User not found");
        }

        return Ok(user);
    }
}
