using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services;

/// <summary>
/// Service responsible for handling payment-related operations for shopping carts.
/// </summary>
 /// <summary>
/// Initializes a new instance of the <see cref="PaymentService"/> class.
/// </summary>
/// <param name="config">Configuration object for accessing app settings like Stripe API key.</param>
/// <param name="cartService">Service for handling cart-related operations.</param>
/// <param name="productRepo">Repository for handling product data.</param>
/// <param name="dmRepo">Repository for handling delivery method data.</param>
public class PaymentService(
  IConfiguration config, 
  ICartService cartService, 
  IGenericRepository<Core.Entities.Product> productRepo, 
  IGenericRepository<DeliveryMethod> dmRepo) 
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
      var deliveryMethod = await dmRepo.GetByIdAsync((int)cart.DeliveryMethodId);

      if(deliveryMethod == null) return null; 

      shippingPrice = deliveryMethod.Price;
    }

    // Update item prices in the cart to match the latest product prices
    foreach (var item in cart.Items)
    {
      var productItem = await productRepo.GetByIdAsync(item.ProductId);

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
