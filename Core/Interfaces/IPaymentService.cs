using Core.Entities;

namespace Core.Interfaces;

/// <summary>
/// Defines the contract for payment services related to shopping carts.
/// </summary>
public interface IPaymentService
{
  /// <summary>
  /// Creates or updates a payment intent for the specified shopping cart.
  /// </summary>
  /// <param name="cartId">The ID of the shopping cart.</param>
  /// <returns>A task representing the asynchronous operation, containing the updated shopping cart or null if not found.</returns>
  Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId);
}
