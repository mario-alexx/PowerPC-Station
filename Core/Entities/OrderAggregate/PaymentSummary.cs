namespace Core.Entities.OrderAggregate;

/// <summary>
/// Summary of the payment details for the order.
/// </summary>
public class PaymentSummary
{
  /// <summary>
  /// The last 4 digits of the payment card.
  /// </summary>
  public int Last4 { get; set; }

  /// <summary>
  /// The brand of the payment card.
  /// </summary>
  public required string Brand { get; set; }

  /// <summary>
  /// The expiration month of the payment card.
  /// </summary>
  public int ExpMonth { get; set; }

  /// <summary>
  /// The expiration year of the payment card.
  /// </summary>
  public int ExpYear { get; set; }
}
