using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[Route("api/stock")]
[ApiController]
public class StocksController : ControllerBase
{
    private readonly MongoDBService _mongoDBService;
    private readonly ILogger<StocksController> _logger;

    public StocksController(MongoDBService mongoDBService, ILogger<StocksController> logger)
    {
        _mongoDBService = mongoDBService;
        _logger = logger;
    }

    // Validation model
    public class ProductDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(
            100,
            MinimumLength = 2,
            ErrorMessage = "Name must be between 2 and 100 characters"
        )]
        public string name { get; set; }

        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000")]
        public double price { get; set; }

        [Range(0, 1000, ErrorMessage = "Quantity must be between 0 and 1000")]
        public int quantity { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [RegularExpression(
            "^(Biere|Vin|Cookie|KinderBueno)$",
            ErrorMessage = "Invalid product type"
        )]
        public string type { get; set; }
    }

    // Mapping method
    private Products MapDtoToProduct(ProductDto dto)
    {
        return new Products
        {
            name = dto.name,
            price = dto.price,
            quantity = dto.quantity,
            type = dto.type,
        };
    }

    // Detailed logging method
    private void LogProductCreation(ProductDto product)
    {
        _logger.LogInformation(
            "Attempting to create product: {ProductName}, Price: {Price}, Quantity: {Quantity}, Type: {Type}",
            product.name,
            product.price,
            product.quantity,
            product.type
        );
    }

    [HttpPost]
    public async Task<ActionResult<Products>> PostProduct([FromBody] ProductDto productDto)
    {
        // Log incoming data for debugging
        LogProductCreation(productDto);

        // Explicitly check model validity
        if (!ModelState.IsValid)
        {
            // Collect and return detailed validation errors
            var errors = ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => new
                {
                    Field = e.Key,
                    Errors = e.Value.Errors.Select(err => err.ErrorMessage),
                })
                .ToList();

            _logger.LogWarning("Product creation failed. Validation errors: {@Errors}", errors);

            return BadRequest(new { message = "Validation failed", errors = errors });
        }

        try
        {
            // Convert DTO to domain model
            var product = MapDtoToProduct(productDto);

            var collection = _mongoDBService.GetCollection<Products>("Products");
            collection.InsertOne(product);

            _logger.LogInformation("Product created successfully: {ProductId}", product.id);

            return CreatedAtAction(nameof(GetProducts), new { id = product.id }, product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            return StatusCode(500, "An error occurred while creating the product");
        }
    }

    //GET :
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
    {
        try
        {
            var products = _mongoDBService
                .GetCollection<Products>("Products")
                .Find(_ => true)
                .ToList();
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST:
    // [HttpPost]
    // public async Task<ActionResult<Products>> PostProduct([FromBody] Products product)
    // {
    //     if (product == null || string.IsNullOrEmpty(product.name))
    //     {
    //         return BadRequest("Invalid product data.");
    //     }

    //     var collection = _mongoDBService.GetCollection<Products>("Products");
    //     collection.InsertOne(product);

    //     return CreatedAtAction(nameof(GetProducts), new { id = product.id }, product);
    // }

    // PUT:
    [HttpPut("{id}")]
    public async Task<ActionResult<Products>> PutProduct(string id, Products product)
    {
        if (product == null || string.IsNullOrEmpty(id))
        {
            return BadRequest("Invalid product data.");
        }

        product.id = id;
        var collection = _mongoDBService.GetCollection<Products>("Products");
        var result = await collection.ReplaceOneAsync(b => b.id == id, product);

        if (result.MatchedCount == 0)
        {
            return NotFound("Product not found");
        }

        return Ok(product);
    }

    // DELETE:
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Invalid product id.");
        }

        var collection = _mongoDBService.GetCollection<Products>("Products");
        collection.DeleteOne(b => b.id == id);

        return NoContent();
    }

    // GET: /id
    [HttpGet("{id}")]
    public async Task<ActionResult<Products>> GetProduct(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Invalid product id.");
        }

        var collection = _mongoDBService.GetCollection<Products>("Products");
        var product = collection.Find(b => b.id == id).FirstOrDefault();

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }
}
