using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? id { get; set; }
    public string? name { get; set; }
    public string? email { get; set; }
    public string? password { get; set; }
    public bool isAdmin { get; set; }
}
