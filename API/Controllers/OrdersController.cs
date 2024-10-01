using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Controller for managing orders.
/// </summary>
/// <param name="cartService">Service for handling shopping cart operations.</param>
/// <param name="unit">Unit of Work pattern for accessing repositories and saving changes.</param>
[Authorize]
public class OrdersController(ICartService cartService, IUnitOfWork unit) : BaseApiController
{ 
  /// <summary>
  /// Creates a new order for the user.
  /// </summary>
  /// <param name="orderDto">Data transfer object containing order details.</param>
  /// <returns>The newly created order or a BadRequest result.</returns>
  [HttpPost]
  public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto)
  {
    var email = User.GetEmail();

    var cart = await cartService.GetCartAsync(orderDto.CartId);

    if(cart == null) return BadRequest("Cart not found");

    if(cart.PaymentIntentId == null) return BadRequest("No payment intent for this order");

    var items = new List<OrderItem>();

    foreach (var item in cart.Items)
    {
      var productItem = await unit.Repository<Product>().GetByIdAsync(item.ProductId);

      if(productItem == null) return BadRequest("Problem with the order");

      var itemOrdered = new ProductItemOrdered 
      {
        ProductId = item.ProductId,
        ProductName = item.ProductName,
        PictureUrl = item.PictureUrl
      };

      var orderItem = new OrderItem 
      {
        ItemOrdered = itemOrdered,
        Price = item.Price,
        Quantity = item.Quantity
      };

      items.Add(orderItem);
    }

    var deliveryMethod = await unit.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId);

    if(deliveryMethod == null) return BadRequest("No delivery method selected");

    var order = new Order
    {
      OrderItems = items,
      DeliveryMethod = deliveryMethod,
      ShippingAddress = orderDto.ShippingAddress,
      PaymentSummary = orderDto.PaymentSummary,
      PaymentIntentId = cart.PaymentIntentId,
      Subtotal = items.Sum(x => x.Price * x.Quantity),
      Discount =  orderDto.Discount,
      BuyerEmail = email
    };

    unit.Repository<Order>().Add(order);

    if(await unit.Complete())
    {
      return order;
    }

    return BadRequest("Problem creating order");
  }

  /// <summary>
  /// Gets a list of orders for the current user.
  /// </summary>
  /// <returns>List of orders in a read-only format.</returns>
  [HttpGet]
  public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
  {
    var spec = new OrderSpecification(User.GetEmail());

    var orders = await unit.Repository<Order>().ListAsync(spec);

    var ordersToReturn = orders.Select(o => o.ToDto()).ToList();

    return Ok(ordersToReturn);
  }

  /// <summary>
  /// Retrieves a specific order by its identifier.
  /// </summary>
  /// <param name="id">The ID of the order to retrieve.</param>
  /// <returns>The order data transfer object or a BadRequest result.</returns>
  [HttpGet("{id:int}")]
  public async Task<ActionResult<OrderDto>> GetOrderById(int id) 
  {
    var spec = new OrderSpecification(User.GetEmail(), id);

    var order = await unit.Repository<Order>().GetEntityWithSpec(spec);

    if(order == null) return NotFound();
    
    return Ok(order.ToDto());
  }
}
