using System.Security.Authentication;
using System.Security.Claims;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

/// <summary>
/// Extension methods for the ClaimsPrincipal class to help retrieve user-specific information.
/// <see cref="ClaimsPrincipal"/>
/// </summary>
public static class ClaimsPrincipleExtensions
{
  /// <summary>
  /// Retrieves the <see cref="AppUser"/> object based on the email from the current user's claims.
  /// </summary>
  /// <param name="userManager">The <see cref="UserManager{AppUser}"/> instance to manage users.</param>
  /// <param name="user">The current <see cref="ClaimsPrincipal"/> representing the authenticated user.</param>
  /// <returns>The <see cref="AppUser"/> object associated with the current user's email.</returns>
  /// <exception cref="AuthenticationException">Thrown when the user is not found.</exception>
  public static async Task<AppUser> GetUserByEmail(this UserManager<AppUser> userManager, ClaimsPrincipal user) 
  {
    var userToReturn = await userManager.Users.FirstOrDefaultAsync(x => 
      x.Email == user.GetEmail());

    if(userToReturn == null) throw new AuthenticationException("User not found");

    return userToReturn;
  }

  /// <summary>
  /// Retrieves the current user along with their address by email from the user manager.
  /// </summary>
  /// <param name="userManager">The <see cref="UserManager{AppUser}"/> used to manage user-related operations.</param>
  /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the current user.</param>
  /// <returns>The user object <see cref="AppUser"/> that includes the address, or throws an <see cref="AuthenticationException"/> if the user is not found.</returns>
  /// <exception cref="AuthenticationException">Thrown when the user is not found.</exception>
  public static async Task<AppUser> GetUserByEmailWithAddress(this UserManager<AppUser> userManager, ClaimsPrincipal user) 
  {
    var userToReturn = await userManager.Users
      .Include(x => x.Address )
      .FirstOrDefaultAsync(x => x.Email == user.GetEmail());

    if(userToReturn == null) throw new AuthenticationException("User not found");

    return userToReturn;
  }

  /// <summary>
  /// Retrieves the email address of the authenticated user from the claims.
  /// </summary>
  /// <param name="user">The current <see cref="ClaimsPrincipal"/> representing the authenticated user.</param>
  /// <returns>The email address of the authenticated user.</returns>
  /// <exception cref="AuthenticationException">Thrown when the user is not found.</exception>
  public static string GetEmail(this ClaimsPrincipal user) 
  {
    var email = user.FindFirstValue(ClaimTypes.Email) ?? 
      throw new AuthenticationException("Email claim not found");
    return email;
  }
}
