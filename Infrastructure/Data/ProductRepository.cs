using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

/// <summary>
/// Repository for managing <see cref="Product"/> entities in the data store.
/// Implements <see cref="IProductRepository"/> to provide specific data access methods.
/// <param name="_context">The database context used to interact with the data store.</param>
/// </summary>    
public class ProductRepository(StoreContext _context) : IProductRepository
{
  /// <inheritdoc />
  public void AddProduct(Product product)
  {
    _context.Products.Add( product );
  }

  /// <inheritdoc />
  public void DeleteProduct(Product product)
  {
    _context.Products.Remove( product );
  }

  /// <inheritdoc />
  public async Task<IReadOnlyList<string>> GetBrandsAsync()
  {
    return await _context.Products.Select(x => x.Brand)
      .Distinct()
      .ToListAsync();
  }

  /// <inheritdoc />
  public async Task<Product?> GetProductByIdAsync(int id)
  {
    return await _context.Products.FindAsync( id );
  }

  /// <inheritdoc />
  public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
  {
    IQueryable<Product> query = _context.Products.AsQueryable();

    if(!string.IsNullOrWhiteSpace(brand))
      query = _context.Products.Where(x => x.Brand == brand);

    if(!string.IsNullOrWhiteSpace(type))
      query = _context.Products.Where(x => x.Type == type);
    
      query = sort switch 
      {
        "priceAsc" => query.OrderBy(x => x.Price),
        "priceDesc" => query.OrderByDescending(x => x.Price),
        _ => query.OrderBy(x => x.Name)
      };

    return await query.ToListAsync();
  }

  /// <inheritdoc />
  public async Task<IReadOnlyList<string>> GetTypesAsync()
  {
    return await _context.Products.Select(x => x.Type)
      .Distinct()
      .ToListAsync();
  }

  /// <inheritdoc />
  public bool ProductExists(int id)
  {
    return _context.Products.Any(x => x.Id == id);  
  }

  /// <inheritdoc />
  public async Task<bool> SaveChangesAsync()
  {
    return await _context.SaveChangesAsync() > 0;
  }

  /// <inheritdoc />
  public void UpdateProduct(Product product)
  {
    _context.Entry(product).State = EntityState.Modified;
  }
}
