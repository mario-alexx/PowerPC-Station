using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Controller for managing coupon-related operations.
/// </summary>
public class CouponsController(ICouponService couponService) : BaseApiController
{ 
  /// <summary>
  /// Validates the specified coupon code.
  /// </summary>
  /// <param name="code">The coupon code to validate.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the coupon information if valid, otherwise an error message.</returns>
 
  [HttpGet("{code}")]
  public async Task<ActionResult<AppCoupon>> ValidateCoupon(string code)
  {
    var coupon = await couponService.GetCouponFromPromoCode(code);

    if(coupon == null) return BadRequest("Invalid voucher code");

    return coupon;
  }
}
