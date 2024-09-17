namespace Core.Entities;

/// <summary>
/// Represents a postal address with line details, city, state, postal code, and country.
/// </summary>
public class Address : BaseEntity
{
  /// <summary>
  /// Gets or sets the first line of the address (e.g., street and number).
  /// </summary>
  public required string Line1 { get; set; }

  /// <summary>
  /// Gets or sets the second line of the address, which is optional (e.g., apartment or suite number).
  /// </summary>
  public string? Line2 { get; set; }

  /// <summary>
  /// Gets or sets the city of the address.
  /// </summary>
  public required string City { get; set; }

  /// <summary>
  /// Gets or sets the state or province of the address.
  /// </summary>
  public required string State { get; set; }

  /// <summary>
  /// Gets or sets the postal or ZIP code of the address.
  /// </summary>
  public required string PostalCode { get; set; }

  /// <summary>
  /// Gets or sets the country of the address.
  /// </summary>
  public required string Country { get; set; }
}
