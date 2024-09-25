using System.ComponentModel.DataAnnotations;
using Core.Entities.OrderAggregate;

namespace API.DTOs;

/// <summary>
/// DTO for creating an order.
/// </summary>
public class CreateOrderDto
{
  /// <summary>
  /// The ID of the cart associated with the order.
  /// </summary>
  [Required]
  public string CartId { get; set; } = string.Empty;

  /// <summary>
  /// The ID of the delivery method chosen for the order.
  /// </summary>
  [Required]
  public int DeliveryMethodId { get; set; }

  /// <summary>
  /// The shipping address for the order.
  /// </summary>
  [Required]
  public ShippingAddress ShippingAddress { get; set; } = null!;

  /// <summary>
  /// The payment summary for the order.
  /// </summary>
  [Required]
  public PaymentSummary PaymentSummary { get; set; } = null!;
}
