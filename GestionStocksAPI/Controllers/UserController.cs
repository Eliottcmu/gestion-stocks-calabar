using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

[Route("api/users")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly MongoDBService _mongoDBService;
    private readonly ILogger<UserController> _logger;

    public UserController(MongoDBService mongoDBService, ILogger<UserController> logger)
    {
        _mongoDBService = mongoDBService;
        _logger = logger;
    }

    // GET: Retrieve all users (accès réservé aux administrateurs)
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        try
        {
            var users = await _mongoDBService
                .GetCollection<User>("Users")
                .Find(user => true)
                .ToListAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération des utilisateurs");
            return StatusCode(500, "Erreur interne du serveur");
        }
    }

    // POST: Add a new user (accès réservé aux administrateurs)
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        if (user == null || string.IsNullOrEmpty(user.name))
        {
            return BadRequest("Données utilisateur invalides.");
        }

        var collection = _mongoDBService.GetCollection<User>("Users");
        await collection.InsertOneAsync(user);

        // Retourne l'utilisateur créé via l'action GetUser (récupération par ID)
        return CreatedAtAction(nameof(GetUser), new { id = user.id }, user);
    }

    // PUT: Update an existing user
    [HttpPut("{id:length(24)}")]
    public async Task<ActionResult<User>> PutUser(string id, User user)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("Admin");

        // Les utilisateurs non-admin ne peuvent mettre à jour que leurs propres données
        if (!isAdmin && userId != id)
        {
            return Forbid();
        }

        if (user == null || string.IsNullOrEmpty(id))
        {
            return BadRequest("Données utilisateur invalides.");
        }

        user.id = id; // S'assurer que l'ID correspond au paramètre de la route
        var collection = _mongoDBService.GetCollection<User>("Users");
        var result = await collection.ReplaceOneAsync(u => u.id == id, user);

        if (result.MatchedCount == 0)
        {
            return NotFound("Utilisateur non trouvé");
        }

        return Ok(user);
    }

    // DELETE: Remove a user by ID (accès réservé aux administrateurs)
    [HttpDelete("{id:length(24)}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("ID utilisateur invalide.");
        }

        var collection = _mongoDBService.GetCollection<User>("Users");
        var result = await collection.DeleteOneAsync(u => u.id == id);

        if (result.DeletedCount == 0)
        {
            return NotFound("Utilisateur non trouvé");
        }

        return NoContent();
    }

    // GET: Retrieve a user by ID
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("ID utilisateur invalide.");
        }

        var collection = _mongoDBService.GetCollection<User>("Users");
        var user = await collection.Find(u => u.id == id).FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound("Utilisateur non trouvé");
        }

        return Ok(user);
    }

    // POST: Inscription (accessible sans authentification)
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<User>> RegisterUser(User newUser)
    {
        if (
            newUser == null
            || string.IsNullOrEmpty(newUser.name)
            || string.IsNullOrEmpty(newUser.password)
            || string.IsNullOrEmpty(newUser.email)
        )
        {
            return BadRequest("Nom, mot de passe et email sont requis.");
        }

        var collection = _mongoDBService.GetCollection<User>("Users");

        // Vérification si username existe déjà
        var existingUser = await collection.Find(u => u.name == newUser.name).FirstOrDefaultAsync();
        if (existingUser != null)
        {
            return Conflict("Un utilisateur avec ce nom existe déjà.");
        }

        await collection.InsertOneAsync(newUser);
        return CreatedAtAction(nameof(GetUser), new { id = newUser.id }, newUser);
    }
}
