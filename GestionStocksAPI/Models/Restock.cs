using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Restock
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? id { get; set; }

    public DateTime date { get; set; }

    [BsonElement("idProduit")]
    public required string idProduit { get; set; }

    [BsonElement("quantiteAjoutee")]
    public int quantiteAjoutee { get; set; }

    [BsonElement("coutTotal")]
    public double coutTotal { get; set; }

    [BsonElement("fournisseur")]
    public required string fournisseur { get; set; }

    [BsonElement("note")]
    public string? note { get; set; }
}
