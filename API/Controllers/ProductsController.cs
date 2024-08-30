using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

  /// <summary>
  /// API controller for managing products in the store.
  /// <param name="_repo">The product repository used to interact with the data store.</param>
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController(IProductRepository _repo) : ControllerBase
  {
    /// <summary>
    /// Retrieves a list of products with optional filters for brand, type, and sorting.
    /// </summary>
    /// <param name="brand">The brand filter for the products.</param>
    /// <param name="type">The type filter for the products.</param>
    /// <param name="sort">The sorting option for the products.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of products.</returns>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort) 
    {
      return Ok( await _repo.GetProductsAsync(brand, type, sort));
    }

    
    /// <summary>
    /// Retrieves a specific product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the product if found; otherwise, a not found response.</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id) 
    {
      var product = await _repo.GetProductByIdAsync(id);

      if(product == null) return NotFound();

      return product;
    }

    /// <summary>
    /// Creates a new product in the data store.
    /// </summary>
    /// <param name="product">The product to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created product.</returns>
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product) 
    {
      _repo.AddProduct(product);
      if(await _repo.SaveChangesAsync()) 
        return CreatedAtAction("GetProduct", new {id = product.Id}, product);

      return BadRequest("Problem creating product.");
    }

    /// <summary>
    /// Updates an existing product in the data store.
    /// </summary>
    /// <param name="id">The unique identifier of the product to update.</param>
    /// <param name="product">The updated product data.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates success or failure.</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product) 
    {
      if(id != product.Id || !ProductExist(id)) 
        return BadRequest("Cannot update this product.");

      _repo.UpdateProduct(product);

      if(await _repo.SaveChangesAsync()) 
        return NoContent();
      
      return BadRequest("Problem updating the product");
    }

    /// <summary>
    /// Deletes a product from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates success or failure.</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
      var product = await _repo.GetProductByIdAsync(id);

      if(product == null) return NotFound();

      _repo.DeleteProduct(product);

      if(await _repo.SaveChangesAsync())
        return NoContent();
      
      return BadRequest("Problem deleting the product");
    }

    /// <summary>
    /// Retrieves a list of all unique product brands.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of product brands.</returns>
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands() 
    {
      return Ok(await _repo.GetBrandsAsync());
    }

    /// <summary>
    /// Retrieves a list of all unique product types.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of product types.</returns>
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
      return Ok(await _repo.GetTypesAsync());
    }

    /// <summary>
    /// Checks if a product exists in the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>True if the product exists; otherwise, false.</returns>
    private bool ProductExist(int id) 
    {
      return _repo.ProductExists(id);
    }
  }
}
