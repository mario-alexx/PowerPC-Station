using API.DTOs;
using API.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Controller for managing user accounts, including registration, authentication, and user information.
/// </summary>
/// <param name="signInManager"></param>
/// <see cref="BaseApiController"/>
public class AccountController(SignInManager<AppUser> signInManager) : BaseApiController
{ 
  /// <summary>
  /// Registers a new user based on the provided registration data.
  /// </summary>
  /// <param name="registerDto">The registration data including first name, last name, email, and password.</param>
  /// <returns>An <see cref="ActionResult"/> indicating success or failure of the registration.</returns>
  [HttpPost("register")]
  public async Task<ActionResult> Register(RegisterDto registerDto)
  {
    var user = new AppUser 
    {
      FirstName = registerDto.FirstName,
      LastName = registerDto.LastName,
      Email = registerDto.Email,
      UserName = registerDto.Email
    };

    var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);
  
    if(!result.Succeeded)
    {
      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(error.Code, error.Description);
      }

      return ValidationProblem();
    }

    return Ok();    
  }

  /// <summary>
  /// Logs out the currently authenticated user.
  /// </summary>
  /// <returns>An <see cref="ActionResult"/> indicating success of the logout operation.</returns>
  [Authorize]
  [HttpPost("logout")]
  public async Task<ActionResult> Logout()
  {
    await signInManager.SignOutAsync();

    return NoContent();
  }

  /// <summary>
  /// Retrieves the authenticated user's information including first name, last name, email, and address.
  /// </summary>
  /// <returns>An <see cref="ActionResult"/> with the user's information, or <see cref="NoContent"/> if the user is not authenticated.</returns>
  [HttpGet("user-info")]
  public async Task<ActionResult> GetUserInfo()
  {
    if(User.Identity?.IsAuthenticated == false) return NoContent();

    var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

    return Ok(new 
    {
      user.FirstName,
      user.LastName,
      user.Email,
      Address = user.Address?.ToDto()
    });
  }

  /// <summary>
  /// Returns the authentication state of the current user.
  /// </summary>
  /// <returns>An <see cref="ActionResult"/> indicating whether the user is authenticated.</returns>
  [HttpGet("auth-status")]
  public ActionResult GetAuthState()
  {
    return Ok(new {IsAuthenticated = User.Identity?.IsAuthenticated ?? false});
  }


  /// <summary>
  /// Creates or updates the user's address information.
  /// </summary>
  /// <param name="addressDto">The new or updated address information.</param>
  /// <returns>An <see cref="ActionResult"/> containing the updated address.</returns>
  [Authorize]
  [HttpPost("address")]
  public async Task<ActionResult<Address>> CreateOrUpdateAddress(AddressDto addressDto) 
  {
    var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

    if (user.Address == null) 
    {
      user.Address = addressDto.ToEntity();
    }
    else 
    {
      user.Address.UpdateFromDto(addressDto);
    }

    var result = await signInManager.UserManager.UpdateAsync(user);

    if(!result.Succeeded) return BadRequest("Problem updating user address");

    return Ok(user.Address.ToDto());
  } 
}
