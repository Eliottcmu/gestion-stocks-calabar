// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using MongoDB.Driver;

// [Authorize]
// [Route("api/profile")]
// [ApiController]
// public class ProfileController : ControllerBase
// {
//     private readonly MongoDBService _mongoDBService;
//     private readonly ILogger<UserController> _logger;

//     public ProfileController(MongoDBService mongoDBService, ILogger<UserController> logger)
//     {
//         _mongoDBService = mongoDBService;
//         _logger = logger;
//     }

//     //GET : retrieve one user profile
//     [HttpGet]
//     public async Task<ActionResult<User>> GetUserProfile()
//     {
//         try
//         {
//             var userId = User.FindFirst("sub")?.Value;
//             if (string.IsNullOrEmpty(userId))
//             {
//                 return Unauthorized();
//             }

//             var collection = _mongoDBService.GetCollection<User>("Users");
//             var user = await collection.Find(u => u.id == userId).FirstOrDefaultAsync();

//             if (user == null)
//             {
//                 return NotFound("Profile not found");
//             }

//             return Ok(user);
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "Error retrieving user profile");
//             return StatusCode(500, "Internal server error");
//         }
//     }

//     //PUT : update user profile
//     [HttpPut]
//     public async Task<ActionResult<User>> UpdateUserProfile(User updatedProfile)
//     {
//         try
//         {
//             var userId = User.FindFirst("sub")?.Value;
//             if (string.IsNullOrEmpty(userId))
//             {
//                 return Unauthorized();
//             }

//             if (updatedProfile == null)
//             {
//                 return BadRequest("Invalid profile data");
//             }

//             updatedProfile.id = userId;
//             var collection = _mongoDBService.GetCollection<User>("Users");
//             var result = await collection.ReplaceOneAsync(u => u.id == userId, updatedProfile);

//             if (result.MatchedCount == 0)
//             {
//                 return NotFound("Profile not found");
//             }

//             return Ok(updatedProfile);
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "Error updating user profile");
//             return StatusCode(500, "Internal server error");
//         }
//     }
// }
