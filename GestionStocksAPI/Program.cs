using MongoDB.Bson;
using MongoDB.Driver;

const string connectionUri =
    "mongodb+srv://eliott:passwordBarStock@clusterbarstock.q2bv2.mongodb.net/?retryWrites=true&w=majority&appName=ClusterBarStock";

var settings = MongoClientSettings.FromConnectionString(connectionUri);

// Set the ServerApi field of the settings object to set the version of the Stable API on the client
settings.ServerApi = new ServerApi(ServerApiVersion.V1);

// Create a new client and connect to the server
var client = new MongoClient(settings);

var collection = client.GetDatabase("Calabar").GetCollection<BsonDocument>("StockBiere");
var filter = Builders<BsonDocument>.Filter.Eq("nom", "Biere1");
var document = collection.Find(filter).First();

// Send a ping to confirm a successful connection
try
{
    var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
    Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
Console.WriteLine(document);
