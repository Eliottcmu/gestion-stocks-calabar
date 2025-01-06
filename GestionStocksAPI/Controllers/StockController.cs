using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[Route("api/[controller]")]
[ApiController]
public class StocksController : ControllerBase
{
    private readonly MongoDBService _mongoDBService;

    public StocksController(MongoDBService mongoDBService)
    {
        _mongoDBService = mongoDBService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var stocks = _mongoDBService.GetCollection<Stock>("Stocks").Find(_ => true).ToList();
        return Ok(stocks);
    }

    [HttpPost]
    public IActionResult Create(Stock stock)
    {
        var collection = _mongoDBService.GetCollection<Stock>("Stocks");
        collection.InsertOne(stock);
        return CreatedAtAction(nameof(Get), new { id = stock.Id }, stock);
    }
}
