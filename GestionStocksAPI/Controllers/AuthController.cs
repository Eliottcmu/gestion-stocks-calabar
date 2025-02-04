using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly MongoDBService _mongoDBService;
    private readonly JwtService _jwtService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        MongoDBService mongoDBService,
        JwtService jwtService,
        ILogger<AuthController> logger
    )
    {
        _mongoDBService = mongoDBService;
        _jwtService = jwtService;
        _logger = logger;
    }

    // GET: Vérifier si l'utilisateur courant est admin
    // Cette action nécessite que l'utilisateur soit authentifié pour que User soit correctement renseigné.
    [HttpGet("isadmin")]
    [Authorize]
    public IActionResult IsAdmin()
    {
        var isAdmin = User.IsInRole("Admin");
        return Ok(new { isAdmin });
    }

    // POST: Authentifier un utilisateur et générer un token JWT
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
    {
        if (request == null)
        {
            return BadRequest("La requête ne peut pas être nulle.");
        }

        try
        {
            var collection = _mongoDBService.GetCollection<User>("Users");
            var user = await collection.Find(u => u.name == request.Name).FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized("Identifiants invalides.");
            }

            // En production, utilisez un algorithme de hachage pour vérifier le mot de passe (ex. BCrypt)
            if (user.password != request.Password)
            {
                return Unauthorized("Identifiants invalides.");
            }

            var token = _jwtService.GenerateToken(user);

            return Ok(new AuthResponse { Token = token, User = user });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la connexion.");
            return StatusCode(500, "Erreur interne du serveur.");
        }
    }
}
