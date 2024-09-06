using Core.Entities;

namespace Core.Specification;

/// <summary>
/// Represents a specification for querying <see cref="Product"/> entities based on optional filter criteria such as brand, type, and sorting.
/// </summary>
public class ProductSpecification : BaseSpecification<Product>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ProductSpecification"/> class with filtering parameters for products.
  /// </summary>
  /// <param name="specParams">The specification parameters that define the search criteria, brands, and types to filter the products.</param>
  public ProductSpecification(ProductSpecParams specParams) : base(x =>
    (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
    (specParams.Brands.Count == 0 || specParams.Brands.Contains(x.Brand)) && 
    (specParams.Types.Count == 0 || specParams.Types.Contains(x.Type))
  )
  {
    ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

    switch (specParams.Sort)
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
