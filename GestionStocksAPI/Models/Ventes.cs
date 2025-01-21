using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Ventes
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? id { get; set; }
    public DateTime date { get; set; }
    public string idProduit { get; set; }

    //public string idUser { get; set; }
    public int quantite { get; set; }
    public double montant { get; set; }
    public string name { get; set; }
}
