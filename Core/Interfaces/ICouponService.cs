using Core.Entities;

namespace Core.Interfaces;

/// <summary>
/// Interface for a service that handles retrieving coupon details based on a promotion code.
/// </summary>
public interface ICouponService
{
  /// <summary>
  /// Retrieves a coupon by its promotion code.
  /// </summary>
  /// <param name="code">The promotion code for the coupon.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the coupon information if found, otherwise null.</returns>
  Task<AppCoupon?> GetCouponFromPromoCode(string code);
}

