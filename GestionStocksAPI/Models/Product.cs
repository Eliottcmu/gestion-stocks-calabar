using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public enum ProduitType
{
    Biere,
    Vin,
    Gouter,
    Miam,
}

public class Products
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string id { get; set; }

    [BsonElement("name")]
    public string name { get; set; }

    [BsonElement("price")]
    public double price { get; set; }

    [BsonElement("quantity")]
    public int quantity { get; set; }

    [BsonElement("type")]
    [BsonRepresentation(BsonType.String)]
    public string type { get; set; }
}
