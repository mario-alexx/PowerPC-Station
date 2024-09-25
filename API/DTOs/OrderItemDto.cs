namespace API.DTOs;

/// <summary>
/// DTO representing an item in the order.
/// </summary>
public class OrderItemDto
{
  /// <summary>
  /// The ID of the product.
  /// </summary>
  public int ProductId { get; set; }

  /// <summary>
  /// The name of the product.
  /// </summary>
  public required string ProductName { get; set; }

  /// <summary>
  /// The URL of the product's picture.
  /// </summary>
  public required string PictureUrl { get; set; }

  /// <summary>
  /// The price of the product.
  /// </summary>
  public decimal Price { get; set; }

  /// <summary>
  /// The quantity of the product ordered.
  /// </summary>
  public int Quantity { get; set; }
}