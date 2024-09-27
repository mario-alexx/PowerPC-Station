using API.Extensions;
using API.SignalR;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Stripe;

namespace API.Controllers;

/// <summary>
/// Controller for managing payments.
/// </summary>
/// <param name="paymentService">Service for handling payment operations.</param>
/// <param name="unit">Unit of Work pattern for accessing repositories and saving changes.</param>
/// <param name="logger">Logger to capture errors and important information.</param>
/// <param name="config">Configuration for accessing app settings, including Stripe secret.</param>
/// <param name="hubContext">SignalR Hub context for sending notifications to the client.</param>
public class PaymentsController(IPaymentService paymentService,IUnitOfWork unit, 
  ILogger<PaymentsController> logger, IConfiguration config, IHubContext<NotificationHub> hubContext) 
: BaseApiController
{ 

  private readonly string _whSecret = config["StripeSettings:WhSecret"]!;
  
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

  /// <summary>
  /// Handles incoming Stripe webhook events for payment processing.
  /// </summary>
  /// <returns>Returns Ok for valid events, or BadRequest for invalid events.</returns>
  [HttpPost("webhook")]
  public async Task<IActionResult> StripeWebhook()
  {
    var json = await new StreamReader(Request.Body).ReadToEndAsync();

    try
    {
      // Constructs the Stripe event based on the received JSON data
      var stripeEvent = ConstructStripeEvent(json);

      if(stripeEvent.Data.Object is not PaymentIntent intent) 
        return BadRequest("Invalid event data");

        await HandlePaymentIntentSucceeded(intent);

        return Ok();
    }
    catch (StripeException ex)
    {  
      logger.LogError(ex, "Stripe webhook error");
      return StatusCode(StatusCodes.Status500InternalServerError, "Webhook error");
    }
  }

  /// <summary>
  /// Handles a successful payment intent by updating the corresponding order's status.
  /// </summary>
  /// <param name="intent">The successful Stripe payment intent.</param>
  private async Task HandlePaymentIntentSucceeded(PaymentIntent intent)
  {
    if(intent.Status == "succeeded")
    {
      var spec = new OrderSpecification(intent.Id, true);

      var order = await unit.Repository<Order>().GetEntityWithSpec(spec) ??
        throw new Exception("Order not found");

      if((long)order.GetTotal() * 100 != intent.Amount)
      {
        order.Status = OrderStatus.PaymentMismatch;
      }
      else 
      {
        order.Status = OrderStatus.PaymentReceived;
      }

      await unit.Complete();

      // Notify the client via SignalR if a connection exists for the buyer's email
      var connectionId = NotificationHub.GetConnectionIdByEmail(order.BuyerEmail);

      if(!string.IsNullOrEmpty(connectionId))
      {
        await hubContext.Clients.Client(connectionId).SendAsync("OrderCompleteNotification", order.ToDto());
      }
    }
  }

  /// <summary>
  /// Constructs a Stripe event from the incoming request body and signature.
  /// </summary>
  /// <param name="json">The raw JSON data from the Stripe webhook.</param>
  /// <returns>Returns the constructed Stripe event.</returns>
  private Event ConstructStripeEvent(string json)
  {
    try
    {
      // Verifies and constructs the Stripe event using the provided signature
      return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _whSecret);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Failed to construct stripe event");
      throw new StripeException("Invalid signature");
    }
  }
}
