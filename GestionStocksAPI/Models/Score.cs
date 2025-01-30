// Score.cs
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Score
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; }

    public int Points { get; set; }

    public DateTime DateAchieved { get; set; }

    // Navigation property (not stored in MongoDB)
    public User? User { get; set; }
}
