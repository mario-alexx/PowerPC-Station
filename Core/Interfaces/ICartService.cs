using Core.Entities;

namespace Core.Interfaces;

/// <summary>
/// Defines the contract for a service that manages shopping carts.
/// </summary>
public interface ICartService
{
  /// <summary>
  /// Retrieves the shopping cart associated with the specified key.
  /// </summary>
  /// <param name="key">The unique identifier for the shopping cart.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the shopping cart if found, or null otherwise.</returns>
  Task<ShoppingCart?> GetCartAsync(string key);

  /// <summary>
  /// Saves or updates the specified shopping cart.
  /// </summary>
  /// <param name="cart">The shopping cart to save or update.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the saved or updated shopping cart.</returns>
  Task<ShoppingCart?> SetCartAsync(ShoppingCart cart);

  /// <summary>
  /// Deletes the shopping cart associated with the specified key.
  /// </summary>
  /// <param name="key">The unique identifier for the shopping cart.</param>
  /// <returns>A task that represents the asynchronous operation. The task result indicates whether the deletion was successful.</returns>
  Task<bool> DeleteCartAsync(string key);
}
