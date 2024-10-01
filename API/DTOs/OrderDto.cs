using Core.Entities.OrderAggregate;

namespace API.DTOs;

/// <summary>
/// DTO representing the details of an order.
/// </summary>
public class OrderDto
{
  /// <summary>
  /// The unique identifier for the order.
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// The date the order was placed.
  /// </summary>
  public DateTime OrderDate { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// The email of the buyer who placed the order.
  /// </summary>
  public required string BuyerEmail { get; set; }

  /// <summary>
  /// The shipping address associated with the order.
  /// </summary>
  public required ShippingAddress ShippingAddress { get; set; }

  /// <summary>
  /// The delivery method chosen for the order.
  /// </summary>
  public required string DeliveryMethod { get; set; }

  /// <summary>
  /// The price of shipping for the order.
  /// </summary>
  public decimal ShippingPrice { get; set; }

  /// <summary>
  /// The payment summary for the order.
  /// </summary>
  public required PaymentSummary PaymentSummary { get; set; }

  /// <summary>
  /// The list of items included in the order.
  /// </summary>
  public required List<OrderItemDto> OrderItems { get; set; }

  /// <summary>
  /// The subtotal amount for the order.
  /// </summary>
  public decimal Subtotal { get; set; }

  /// <summary>
  /// Gets or sets the discount applied to the order.
  /// </summary>
  public decimal Discount { get; set; }

  /// <summary>
  /// The current status of the order.
  /// </summary>
  public required string Status { get; set; }

  /// <summary>
  /// The total amount for the order including shipping.
  /// </summary>
  public decimal Total { get; set; }

  /// <summary>
  /// The payment intent ID associated with the order.
  /// </summary>
  public required string PaymentIntentId { get; set; }
}
