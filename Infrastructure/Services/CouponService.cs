using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services;

/// <summary>
/// Service implementation for retrieving coupon information based on a promotion code.
/// </summary>
public class CouponService : ICouponService
{
  /// <summary>
  /// Initializes a new instance of the <see cref="CouponService"/> class.
  /// </summary>
  /// <param name="config">The application configuration settings.</param>
  public CouponService(IConfiguration config)
  {
    StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
  }

  /// <summary>
  /// Asynchronously retrieves the details of a coupon by its promotion code.
  /// </summary>
  /// <param name="code">The promotion code of the coupon to retrieve.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the coupon information if found, otherwise null.</returns>
  public async Task<AppCoupon?> GetCouponFromPromoCode(string code)
  {
    var promotionService = new PromotionCodeService();

    var options = new PromotionCodeListOptions 
    {
      Code = code
    };

    var promotionCodes = await promotionService.ListAsync(options);

    var promotionCode = promotionCodes.FirstOrDefault();

    if(promotionCode != null && promotionCode.Coupon != null)
    {
      return new AppCoupon 
      {
        Name = promotionCode.Coupon.Name,
        AmountOff = promotionCode.Coupon.AmountOff,
        PercentOff = promotionCode.Coupon.PercentOff,
        PromotionCode = promotionCode.Code,
        CouponId = promotionCode.Coupon.Id,
      };
    }
    return null;
  }
}
