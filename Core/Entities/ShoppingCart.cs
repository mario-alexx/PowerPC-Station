namespace Core.Entities;

/// <summary>
/// Represents a shopping cart, containing a unique identifier and a list of items.
/// </summary>
public class ShoppingCart
{
  /// <summary>
  /// Gets or sets the unique identifier for the shopping cart.
  /// This field is required.
  /// </summary>
  public required string Id { get; set; }

  /// <summary>
  /// Gets or sets the list of items in the shopping cart.
  /// Each item is an instance of <see cref="CartItem"/>.
  /// </summary>
  public List<CartItem> Items { get; set; } = new List<CartItem>();
}