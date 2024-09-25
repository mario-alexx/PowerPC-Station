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
}
