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
    
    var cart = await cartService.GetCartAsync(cartId) 
      ?? throw new Exception("Cart unavailable");

    var shippingPrice = await GetShippingPriceAsync(cart) ?? 0;

    await ValidateCartItemsInCartAsync(cart);

    var subtotal = CalculateSubtotal(cart);

    if(cart.Coupon != null) 
    {
      subtotal = await ApplyDiscountAsync(cart.Coupon, subtotal);
    }

    var total = subtotal + shippingPrice;

    await CreateUpdatePaymentIntentAsync(cart, total);

    await cartService.SetCartAsync(cart);

    return cart;
  }

  /// <summary>
  /// Asynchronously creates or updates the payment intent.
  /// </summary>
  /// <param name="cart">The shopping cart to update.</param>
  /// <param name="total">The total amount to be charged.</param>
  private async Task CreateUpdatePaymentIntentAsync(ShoppingCart cart, long total)
  {
    var service = new PaymentIntentService();

    if(string.IsNullOrEmpty(cart.PaymentIntentId))
    {
      var options = new PaymentIntentCreateOptions 
      {
        Amount = total,
        Currency = "usd",
        PaymentMethodTypes = ["card"]
      };

      var intent = await service.CreateAsync(options);
      cart.PaymentIntentId = intent.Id;
      cart.ClientSecret = intent.ClientSecret;
    }
    else 
    {
      var options = new PaymentIntentUpdateOptions
      {
        Amount = total,
      };
      await service.UpdateAsync(cart.PaymentIntentId, options);
    }
  }

  /// <summary>
  /// Applies a discount to the specified amount based on the provided coupon.
  /// </summary>
  /// <param name="appCoupon">The coupon to apply.</param>
  /// <param name="amount">The original amount.</param>
  /// <returns>The discounted amount.</returns>
  private async Task<long> ApplyDiscountAsync(AppCoupon appCoupon, long amount)
  {
    var couponService = new Stripe.CouponService();

    var coupon = await couponService.GetAsync(appCoupon.CouponId);

    if(coupon.AmountOff.HasValue)
    {
      amount -= (long)coupon.AmountOff * 100;
    }

    if(coupon.PercentOff.HasValue)
    {
      var discount =  amount * (coupon.PercentOff.Value / 100);
      amount -= (long)discount;
    }
    
    return amount;
  }

  /// <summary>
  /// Calculates the subtotal of the items in the shopping cart.
  /// </summary>
  /// <param name="cart">The shopping cart containing the items.</param>
  /// <returns>The calculated subtotal.</returns>
  private long CalculateSubtotal(ShoppingCart cart)
  {
    var itemTotal = cart.Items.Sum(x => x.Quantity * x.Price * 100);
    return (long)itemTotal;
  }

  /// <summary>
  /// Validates that the items in the shopping cart are available and valid.
  /// </summary>
  /// <param name="cart">The shopping cart to validate.</param>
  private async Task ValidateCartItemsInCartAsync(ShoppingCart cart)
  {
    foreach(var item in cart.Items)
    {
      var productItem = await unit.Repository<Core.Entities.Product>()
        .GetByIdAsync(item.ProductId) 
          ?? throw new Exception("Problem getting product in cart");

      if(item.Price != productItem.Price)
      {
        item.Price = productItem.Price;
      }
    }
  }

  /// <summary>
  /// Retrieves the shipping price for the items in the shopping cart.
  /// </summary>
  /// <param name="cart">The shopping cart to retrieve the shipping price for.</param>
  /// <returns>The shipping price or null if not applicable.</returns>
  private async Task<long?> GetShippingPriceAsync(ShoppingCart cart)
  {
    // hint: throw exception if cannot find delivery method
    // return null if cart does not have it set
    if(cart.DeliveryMethodId.HasValue)
    {
      var deliveryMethod = await unit.Repository<DeliveryMethod>().
        GetByIdAsync((int)cart.DeliveryMethodId)
          ?? throw new Exception("Problem with delivery method");

      return (long)deliveryMethod.Price * 100;
    }
    return null;    
  }
}
