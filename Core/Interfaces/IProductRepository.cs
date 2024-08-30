using Core.Entities;

namespace Core.Interfaces;

/// <summary>
/// Provides methods for managing products in the repository.
/// </summary>
public interface IProductRepository
{
  /// <summary>
  /// Asynchronously retrieves a read-only list of products with optional filtering by brand and type, and optional sorting.
  /// </summary>
  /// <param name="brand">The brand to filter products by (optional).</param>
  /// <param name="type">The type to filter products by (optional).</param>
  /// <param name="sort">The sorting option for the products (optional).</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of products that match the specified filters and sorting options.</returns>
  Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort);

  /// <summary>
  /// Asynchronously retrieves a product by its unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier of the product.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the product if found; otherwise, null.</returns>
  Task<Product?> GetProductByIdAsync(int id);

  /// <summary>
  /// Asynchronously retrieves a read-only list of all unique product brands.
  /// </summary>
  /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of product brands.</returns>
  Task<IReadOnlyList<string>> GetBrandsAsync();

  /// <summary>
  /// Asynchronously retrieves a read-only list of all unique product types.
  /// </summary>
  /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of product types.</returns>
  Task<IReadOnlyList<string>> GetTypesAsync();

  /// <summary>
  /// Adds a new product to the repository.
  /// </summary>
  /// <param name="product">The product to add.</param>
  void AddProduct(Product product);

  /// <summary>
  /// Updates an existing product in the repository.
  /// </summary>
  /// <param name="product">The product to update.</param>
  void UpdateProduct(Product product);

  /// <summary>
  /// Deletes a product from the repository.
  /// </summary>
  /// <param name="product">The product to delete.</param>
  void DeleteProduct(Product product);

  /// <summary>
  /// Checks if a product exists in the repository by its unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier of the product.</param>
  /// <returns>True if the product exists; otherwise, false.</returns>
  bool ProductExists(int id);

  /// <summary>
  /// Asynchronously saves all changes made in the repository to the underlying data store.
  /// </summary>
  /// <returns>A task that represents the asynchronous save operation. The task result contains a boolean indicating whether the changes were successfully saved.</returns>
  Task<bool> SaveChangesAsync();
}

