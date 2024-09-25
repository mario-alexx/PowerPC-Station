namespace Core.Entities.OrderAggregate;

/// <summary>
/// Represents the shipping address for an order.
/// </summary>
public class ShippingAddress
{
  /// <summary>
  /// The recipient's name for the shipping address.
  /// </summary>
  public required string Name { get; set; }

  /// <summary>
  /// The first line of the shipping address.
  /// </summary>
  public required string Line1 { get; set; }

  /// <summary>
  /// The second line of the shipping address (optional).
  /// </summary>
  public string? Line2 { get; set; }

  /// <summary>
  /// The city for the shipping address.
  /// </summary>
  public required string City { get; set; }

  /// <summary>
  /// The state for the shipping address.
  /// </summary>
  public required string State { get; set; }

  /// <summary>
  /// The postal code for the shipping address.
  /// </summary>
  public required string PostalCode { get; set; }

  /// <summary>
  /// The country for the shipping address.
  /// </summary>
  public required string Country { get; set; }
}
