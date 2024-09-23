namespace Core.Entities;

/// <summary>
/// Represents a delivery method available for orders.
/// </summary>
public class DeliveryMethod : BaseEntity
{
  /// <summary>
  /// Short name of the delivery method.
  /// </summary>
  public required string ShortName { get; set; }

  /// <summary>
  /// Estimated delivery time for the method.
  /// </summary>
  public required string DeliveryTime { get; set; }

  /// <summary>
  /// Detailed description of the delivery method.
  /// </summary>
  public required string Description { get; set; }

  /// <summary>
  /// Price of the delivery method.
  /// </summary>
  public decimal Price { get; set; }
}
