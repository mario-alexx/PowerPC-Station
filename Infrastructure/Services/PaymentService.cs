using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services;

/// <summary>
/// Service class for handling payment processing.
/// </summary>
/// <param name="config">Configuration for accessing environment variables, including API keys.</param>
/// <param name="cartService">Service for handling shopping cart operations.</param>
/// <param name="unit">Unit of Work pattern for accessing repositories and saving changes.</param>
public class PaymentService(
  IConfiguration config, 
  ICartService cartService, 
  IUnitOfWork unit) 
  : IPaymentService
{
 
  /// <summary>
  /// Creates or updates a payment intent for the specified shopping cart.
  /// </summary>
  /// <param name="cartId">The ID of the shopping cart.</param>
  /// <returns>
  /// A task representing the asynchronous operation, containing the updated shopping cart with payment intent data or null if not found.
  /// </returns>
  public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
  {
    // Set the Stripe API key from the configuration settings
    StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
    var cart = await cartService.GetCartAsync(cartId);

    if(cart == null) return null;

    var shippingPrice = 0m;

    // Calculate shipping cost if delivery method is selected
    if(cart.DeliveryMethodId.HasValue) 
    {
      var deliveryMethod = await unit.Repository<DeliveryMethod>().GetByIdAsync((int)cart.DeliveryMethodId);

      if(deliveryMethod == null) return null; 

      shippingPrice = deliveryMethod.Price;
    }

    // Update item prices in the cart to match the latest product prices
    foreach (var item in cart.Items)
    {
      var productItem = await unit.Repository<Core.Entities.Product>().GetByIdAsync(item.ProductId);

      if(productItem == null) return null;

      if(item.Price != productItem.Price) 
      {
        item.Price = productItem.Price;
      }
    }

    var service = new PaymentIntentService();
    PaymentIntent? intent = null;

    // Create or update payment intent
    if(string.IsNullOrEmpty(cart.PaymentIntentId))
    {
      var options = new PaymentIntentCreateOptions
      {
        Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100))
           + (long)shippingPrice * 100,
        Currency = "usd",
        PaymentMethodTypes = ["card"]       
      };
      intent = await service.CreateAsync(options);
      cart.PaymentIntentId = intent.Id;
      cart.ClientSecret = intent.ClientSecret;
    }
    else 
    {
      var options = new PaymentIntentUpdateOptions 
      {
        Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100))
           + (long)shippingPrice * 100
      };
      intent = await service.UpdateAsync(cart.PaymentIntentId, options);
    }

    await cartService.SetCartAsync(cart);

    return cart;
  }
}
