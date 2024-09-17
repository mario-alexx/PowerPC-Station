using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

/// <summary>
/// Data Transfer Object (DTO) used for registering a new user.
/// </summary>
public class RegisterDto
{ 
  /// <summary>
  /// Gets or sets the first name of the user. This field is required.
  /// </summary>
  [Required]
  public string FirstName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the last name of the user. This field is required.
  /// </summary>
  [Required]
  public string LastName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the email of the user. This field is required.
  /// </summary>
  [Required]
  public string Email { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the password of the user. This field is required.
  /// </summary>
  [Required]
  public string Password { get; set; } = string.Empty;
}
