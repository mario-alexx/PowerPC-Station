using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  
/// <summary>
/// Controller for managing products.
/// </summary>
/// <param name="unit">Unit of Work pattern for accessing repositories and saving changes.</param>
  public class ProductsController(IUnitOfWork unit) : BaseApiController
  {
    /// <summary>
    /// Gets a paginated list of products based on the provided filtering and pagination parameters.
    /// </summary>
    /// <param name="specParams">The parameters used for filtering, sorting, and paginating products.</param>
    /// <returns>A paginated list of products that match the specified criteria.</returns>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams specParams) 
    {
      var spec = new ProductSpecification(specParams);
      var result = await CreatePagedResult(unit.Repository<Product>(), spec, specParams.PageIndex, specParams.PageSize);
      return result;
    }

    /// <summary>
    /// Retrieves a specific product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the product if found; otherwise, a not found response.</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id) 
    {
      var product = await unit.Repository<Product>().GetByIdAsync(id);

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
      unit.Repository<Product>().Add(product);
      if(await unit.Complete()) 
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

      unit.Repository<Product>().Update(product);

      if(await unit.Complete()) 
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
      var product = await unit.Repository<Product>().GetByIdAsync(id);

      if(product == null) return NotFound();

      unit.Repository<Product>().Remove(product);

      if(await unit.Complete())
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
      var spec = new BrandListSpecification();

      return Ok(await unit.Repository<Product>().ListAsync(spec));
    }

    /// <summary>
    /// Retrieves a list of all unique product types.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of product types.</returns>
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
      var spec = new TypeListSpecification();

      return Ok(await unit.Repository<Product>().ListAsync(spec));
    }

    /// <summary>
    /// Checks if a product exists in the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>True if the product exists; otherwise, false.</returns>
    private bool ProductExist(int id) 
    {
      return unit.Repository<Product>().Exists(id);
    }
  }
}
