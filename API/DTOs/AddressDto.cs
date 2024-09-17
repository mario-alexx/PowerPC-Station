using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

/// <summary>
/// Data Transfer Object (DTO) representing an address.
/// </summary>
public class AddressDto
{
  /// <summary>
  /// Gets or sets the first address line. This field is required.
  /// </summary>
  [Required]
  public string Line1 { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the second address line. This field is optional.
  /// </summary>
  public string? Line2 { get; set; }

  /// <summary>
  /// Gets or sets the city. This field is required.
  /// </summary>
  [Required]
  public string City { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the state. This field is required.
  /// </summary>
  [Required]
  public string State { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the postal code. This field is required.
  /// </summary>
  [Required]
  public string PostalCode { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the country. This field is required.
  /// </summary>
  [Required]
  public string Country { get; set; } = string.Empty;
}
