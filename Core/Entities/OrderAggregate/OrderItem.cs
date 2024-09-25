namespace Core.Entities.OrderAggregate;

/// <summary>
/// Represents an item within an order.
/// </summary>
public class OrderItem : BaseEntity
{
  /// <summary>
  /// Details about the product that was ordered.
  /// </summary>
  public ProductItemOrdered ItemOrdered { get; set; } = null!;

  /// <summary>
  /// The price of the ordered item.
  /// </summary>
  public decimal Price { get; set; }

  /// <summary>
  /// The quantity of the item ordered.
  /// </summary>
  public int Quantity { get; set; }
}
