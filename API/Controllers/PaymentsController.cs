using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Controller for managing payments.
/// </summary>
/// <param name="paymentService">Service for handling payment operations.</param>
/// <param name="unit">Unit of Work pattern for accessing repositories and saving changes.</param>
public class PaymentsController(IPaymentService paymentService, IUnitOfWork unit) 
: BaseApiController
{ 
  
  /// <summary>
  /// Creates or updates the payment intent for the given cart.
  /// </summary>
  /// <param name="cartId">The ID of the cart to create or update the payment intent for.</param>
  /// <returns>The updated <see cref="ShoppingCart"/> object.</returns>
  [Authorize]
  [HttpPost("{cartId}")]
  public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePaymentIntent(string cartId) 
  {
    var cart = await paymentService.CreateOrUpdatePaymentIntent(cartId);

    if(cart == null) return BadRequest("Problem with your cart");

    return Ok(cart);
  } 

  /// <summary>
  /// Retrieves a list of available delivery methods.
  /// </summary>
  /// <returns>A list of <see cref="DeliveryMethod"/> objects.</returns>
  [HttpGet("delivery-methods")]
  public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods() 
  {
    return Ok(await unit.Repository<DeliveryMethod>().ListAllAsync());
  }
}
