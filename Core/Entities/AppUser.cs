using Microsoft.AspNetCore.Identity;
namespace Core.Entities;

/// <summary>
/// Represents a user of the application, inheriting from IdentityUser.
/// </summary>
public class AppUser : IdentityUser
{
  /// <summary>
  /// Gets or sets the first name of the user. This field is optional.
  /// </summary>
  public string? FirstName { get; set; }

  /// <summary>
  /// Gets or sets the last name of the user. This field is optional.
  /// </summary>
  public string? LastName { get; set; }

  /// <summary>
  /// Gets or sets the user's address. This field is optional.
  /// </summary>
  public Address? Address { get; set; }
}