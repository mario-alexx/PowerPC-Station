namespace Core.Entities.OrderAggregate;

/// <summary>
/// Represents the status of an order.
/// </summary>
public enum OrderStatus
{
  Pending,
  PaymentReceived,
  PaymentFailed,
  PaymentMismatch
}
