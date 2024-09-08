using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

/// <summary>
/// DTO for creating a new product.
/// </summary>
public class CreateProductDto
{
  /// <summary>
  /// The name of the product.
  /// </summary>
  [Required]
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// The description of the product.
  /// </summary>
  [Required]
  public string Description { get; set; } = string.Empty;

  /// <summary>
  /// The price of the product. Must be greater than 0.
  /// </summary>
  [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
  public decimal Price { get; set; }

  /// <summary>
  /// The URL of the product image.
  /// </summary>
  [Required]
  public string PictureUrl { get; set; } = string.Empty;

  /// <summary>
  /// The type/category of the product.
  /// </summary>
  [Required]
  public string Type { get; set; } = string.Empty;

  /// <summary>
  /// The brand of the product.
  /// </summary>
  [Required]
  public string Brand { get; set; } = string.Empty;

  /// <summary>
  /// The quantity of the product available in stock. Must be at least 1.
  /// </summary>
  [Range(1, int.MaxValue, ErrorMessage = "Quantity in stock must be at least 1")]
  public int QuantityInStock { get; set; }
}
