// ScoreController.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

[Route("api/scores")]
[ApiController]
public class ScoreController : ControllerBase
{
    private readonly MongoDBService _mongoDBService;
    private readonly ILogger<ScoreController> _logger;

    public ScoreController(MongoDBService mongoDBService, ILogger<ScoreController> logger)
    {
        _mongoDBService = mongoDBService;
        _logger = logger;
    }

    // GET: Retrieve leaderboard (top scores)
    [HttpGet("leaderboard")]
    public async Task<ActionResult<IEnumerable<dynamic>>> GetLeaderboard(int limit = 10)
    {
        try
        {
            var scoresCollection = _mongoDBService.GetCollection<Score>("Scores");
            var usersCollection = _mongoDBService.GetCollection<User>("Users");

            var pipeline = new[]
            {
                new BsonDocument(
                    "$group",
                    new BsonDocument
                    {
                        { "_id", "$UserId" },
                        { "highestScore", new BsonDocument("$max", "$Points") },
                    }
                ),
                new BsonDocument("$sort", new BsonDocument("highestScore", -1)),
                new BsonDocument("$limit", limit),
            };

            var results = await scoresCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();
            var leaderboard = new List<dynamic>();

            foreach (var result in results)
            {
                var userId = result["_id"].AsString;
                var user = await usersCollection.Find(u => u.id == userId).FirstOrDefaultAsync();

                leaderboard.Add(
                    new
                    {
                        UserId = userId,
                        UserName = user?.name ?? "Unknown User",
                        HighestScore = result["highestScore"].AsInt32,
                    }
                );
            }

            return Ok(leaderboard);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving leaderboard");
            return StatusCode(500, "Internal server error");
        }
    }

    // POST: Add a new score
    [HttpPost]
    public async Task<ActionResult<Score>> PostScore(Score score)
    {
        try
        {
            if (score == null || string.IsNullOrEmpty(score.UserId))
            {
                return BadRequest("Invalid score data.");
            }

            score.DateAchieved = DateTime.UtcNow;
            var collection = _mongoDBService.GetCollection<Score>("Scores");
            await collection.InsertOneAsync(score);

            return CreatedAtAction(nameof(GetUserScores), new { userId = score.UserId }, score);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding new score");
            return StatusCode(500, "Internal server error");
        }
    }

    // GET: Retrieve scores for a specific user
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Score>>> GetUserScores(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid user ID.");
            }

            var collection = _mongoDBService.GetCollection<Score>("Scores");
            var scores = await collection
                .Find(s => s.UserId == userId)
                .SortByDescending(s => s.Points)
                .ToListAsync();

            return Ok(scores);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user scores");
            return StatusCode(500, "Internal server error");
        }
    }

    // GET: Get user's highest score
    [HttpGet("user/{userId}/highest")]
    public async Task<ActionResult<Score>> GetUserHighestScore(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid user ID.");
            }

            var collection = _mongoDBService.GetCollection<Score>("Scores");
            var highestScore = await collection
                .Find(s => s.UserId == userId)
                .SortByDescending(s => s.Points)
                .FirstOrDefaultAsync();

            if (highestScore == null)
            {
                return NotFound("No scores found for this user");
            }

            return Ok(highestScore);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user's highest score");
            return StatusCode(500, "Internal server error");
        }
    }
}
