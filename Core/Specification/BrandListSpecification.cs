using Core.Entities;

namespace Core.Specification;

/// <summary>
/// Represents a specification for querying distinct brand names from <see cref="Product"/> entities.
/// </summary>
public class BrandListSpecification : BaseSpecification<Product, string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="BrandListSpecification"/> class.
  /// </summary>
  public BrandListSpecification()
  {
    AddSelect(x => x.Brand);
    ApplyDistinct();
  }
}
