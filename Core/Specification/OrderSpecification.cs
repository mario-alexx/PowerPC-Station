using Core.Entities.OrderAggregate;

namespace Core.Specification;

/// <summary>
/// Specification for retrieving orders based on the buyer's email or email and order ID.
/// </summary>
public class OrderSpecification : BaseSpecification<Order>
{ 
  /// <summary>
  /// Initializes a new specification for retrieving orders by buyer email.
  /// </summary>
  /// <param name="email">The email of the buyer.</param>
  public OrderSpecification(string email) : base(x => x.BuyerEmail == email)
  {
    AddInclude(x => x.OrderItems);
    AddInclude(x => x.DeliveryMethod);
    AddOrderByDescending(x => x.OrderDate);
  }

  /// <summary>
  /// Initializes a new specification for retrieving a specific order by buyer email and order ID.
  /// </summary>
  /// <param name="email">The email of the buyer.</param>
  /// <param name="id">The ID of the order.</param>
  public OrderSpecification(string email, int id) : base(x => x.BuyerEmail == email && x.Id == id)
  {
    AddInclude("OrderItems");
    AddInclude("DeliveryMethod");
  }

  /// <summary>
  /// Constructor for retrieving an order by payment intent ID.
  /// </summary>
  /// <param name="paymentIntentId">The payment intent ID to filter the order by.</param>
  /// <param name="isPaymentIntent">A flag to indicate if the query is for a payment intent.</param>
  public OrderSpecification(string paymentIntentId, bool isPaymentIntent) : base(x => x.PaymentIntentId == paymentIntentId) 
  {
    AddInclude("OrderItems");
    AddInclude("DeliveryMethod");
  }
}
