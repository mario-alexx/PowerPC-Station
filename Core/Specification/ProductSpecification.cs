using Core.Entities;

namespace Core.Specification;

/// <summary>
/// Represents a specification for querying <see cref="Product"/> entities based on optional filter criteria such as brand, type, and sorting.
/// </summary>
public class ProductSpecification : BaseSpecification<Product>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ProductSpecification"/> class with optional filtering criteria.
  /// </summary>
  /// <param name="brand">The brand filter to apply to the products; or null to apply no brand filtering.</param>
  /// <param name="type">The type filter to apply to the products; or null to apply no type filtering.</param>
  /// <param name="sort">The sorting option to apply to the products; or null to apply no specific sorting.</param>
  public ProductSpecification(string? brand, string? type, string? sort) : base(x =>
    (string.IsNullOrWhiteSpace(brand) || x.Brand == brand) && 
    (string.IsNullOrWhiteSpace(type) || x.Type == type)
  )
  {
    switch (sort)
    {
      case "priceAsc":
        AddOrderBy(x => x.Price);
        break;
      case "priceDesc": 
        AddOrderByDescending(x => x.Price);
        break;   
      default:
        AddOrderBy(x => x.Name);
        break;
    }
  }
}
