using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Beers
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? id { get; set; }

    public string? name { get; set; }
    public int quantity { get; set; }
    public double price { get; set; }

    // public string? Type { get; set; } // Lager, IPA, etc.
    // public double AlcoholPercentage { get; set; } // Taux d'alcool
}
