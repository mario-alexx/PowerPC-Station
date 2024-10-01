namespace Core.Entities;

/// <summary>
/// Represents a coupon that can be applied to an order or shopping cart.
/// </summary>
public class AppCoupon
{
  /// <summary>
  /// Gets or sets the name of the coupon.
  /// </summary>
  public required string Name { get; set; } 

  /// <summary>
  /// Gets or sets the amount off in currency (if applicable).
  /// </summary>
  public decimal? AmountOff { get; set; } 

  /// <summary>
  /// Gets or sets the percentage off for the coupon (if applicable).
  /// </summary>
  public decimal? PercentOff { get; set; } 

  /// <summary>
  /// Gets or sets the promotion code associated with the coupon.
  /// </summary>
  public required string PromotionCode { get; set; }

  /// <summary>
  /// Gets or sets the unique identifier for the coupon.
  /// </summary>
  public required string CouponId { get; set; } 
}

