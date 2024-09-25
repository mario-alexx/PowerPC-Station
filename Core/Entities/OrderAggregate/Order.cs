namespace Core.Entities.OrderAggregate;

/// <summary>
/// Represents an order placed by a buyer.
/// </summary>
public class Order : BaseEntity
{
  /// <summary>
  /// The date when the order was placed.
  /// </summary>
  public DateTime OrderDate { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// The email of the buyer who placed the order.
  /// </summary>
  public required string BuyerEmail { get; set; }

  /// <summary>
  /// The shipping address for the order.
  /// </summary>
  public ShippingAddress ShippingAddress { get; set; } = null!;

  /// <summary>
  /// The delivery method chosen for the order.
  /// </summary>
  public DeliveryMethod DeliveryMethod { get; set; } = null!;

  /// <summary>
  /// Summary of the payment details for the order.
  /// </summary>
  public PaymentSummary PaymentSummary { get; set; } = null!;

  /// <summary>
  /// The list of items included in the order.
  /// </summary>
  public List<OrderItem> OrderItems { get; set; } = [];

  /// <summary>
  /// The subtotal amount of the order.
  /// </summary>
  public decimal Subtotal { get; set; }

  /// <summary>
  /// The current status of the order.
  /// </summary>
  public OrderStatus Status { get; set; } = OrderStatus.Pending;

  /// <summary>
  /// The payment intent ID associated with the order.
  /// </summary>
  public required string PaymentIntentId { get; set; }

  /// <summary>
  /// Calculates the total amount for the order including the delivery cost.
  /// </summary>
  /// <returns>The total price of the order.</returns>
  public decimal GetTotal()
  {
    return Subtotal + DeliveryMethod.Price;
  }
}
