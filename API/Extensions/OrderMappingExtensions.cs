using API.DTOs;
using Core.Entities.OrderAggregate;

namespace API.Extensions;

/// <summary>
/// Static class providing extension methods for mapping orders.
/// </summary>
public static class OrderMappingExtensions
{ 
  /// <summary>
  /// Converts an Order entity to an OrderDto.
  /// </summary>
  /// <param name="order">The order entity.</param>
  /// <returns>The corresponding OrderDto.</returns>
  public static OrderDto ToDto(this Order order)
  {
    return new OrderDto 
    {
      Id = order.Id,
      BuyerEmail = order.BuyerEmail,
      OrderDate = order.OrderDate,
      ShippingAddress = order.ShippingAddress,
      PaymentSummary = order.PaymentSummary,
      DeliveryMethod = order.DeliveryMethod.Description,
      ShippingPrice = order.DeliveryMethod.Price,
      OrderItems = order.OrderItems.Select(x => x.ToDto()).ToList(),
      Subtotal = order.Subtotal,
      Discount = order.Discount,
      Total = order.GetTotal(),
      Status = order.Status.ToString(),
      PaymentIntentId = order.PaymentIntentId,
    };
  }

   /// <summary>
  /// Converts an OrderItem entity to an OrderItemDto.
  /// </summary>
  /// <param name="orderItem">The order item entity.</param>
  /// <returns>The corresponding OrderItemDto.</returns>
  public static OrderItemDto ToDto(this OrderItem orderItem)
  {
    return new OrderItemDto
    {
      ProductId = orderItem.ItemOrdered.ProductId,
      ProductName = orderItem.ItemOrdered.ProductName,
      PictureUrl = orderItem.ItemOrdered.PictureUrl,
      Price = orderItem.Price,
      Quantity = orderItem.Quantity
    };
  }
}
