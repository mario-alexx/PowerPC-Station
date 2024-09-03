using Core.Entities;

namespace Core.Specification;

/// <summary>
/// Represents a specification for querying distinct types from <see cref="Product"/> entities.
/// </summary>
public class TypeListSpecification : BaseSpecification<Product, string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="TypeListSpecification"/> class.
  /// </summary>
  public TypeListSpecification()
  {
    AddSelect(x => x.Type);
    ApplyDistinct();
  }
}
