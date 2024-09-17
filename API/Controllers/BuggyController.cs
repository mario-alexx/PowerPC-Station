using System.Security.Claims;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Controller to simulate various HTTP errors for testing purposes.
/// </summary>
public class BuggyController : BaseApiController
{
  /// <summary>
  /// Returns an Unauthorized (401) response.
  /// </summary>
  /// <returns>401 Unauthorized response</returns>
  [HttpGet("unauthorized")]
  public IActionResult GetUnauthorized() 
  {
    return Unauthorized();
  }

  /// <summary>
  /// Returns a Bad Request (400) response with a custom message.
  /// </summary>
  /// <returns>400 Bad Request response with a custom message</returns>
  [HttpGet("badrequest")]
  public IActionResult GetBadRequest() 
  {
    return BadRequest("Not a good request");
  }

  /// <summary>
  /// Returns a Not Found (404) response.
  /// </summary>
  /// <returns>404 Not Found response</returns>
  [HttpGet("notfound")]
  public IActionResult GetNotFound() 
  {
    return NotFound();
  }

  /// <summary>
  /// Simulates an Internal Server Error (500) by throwing an exception.
  /// </summary>
  /// <returns>Throws an exception to simulate a 500 error</returns>
  [HttpGet("internalerror")]
  public IActionResult GetInternalError() 
  {
    throw new Exception("This is a test exception");
  }

  /// <summary>
  /// Simulates a validation error by accepting a product and returning an OK response.
  /// </summary>
  /// <param name="product">The product data to validate</param>
  /// <returns>200 OK response (normally this would validate the product data)</returns>
  [HttpPost("validationerror")]
  public IActionResult GetValidationError(CreateProductDto product) 
  {
    return Ok();
  }

  /// <summary>
  /// Retrieves a secret resource, only accessible to authorized users.
  /// </summary>
  /// <returns>
  /// An <see cref="IActionResult"/> containing the secret resource if the user is authorized; otherwise, an unauthorized response.
  /// </returns>
  [Authorize]
  [HttpGet("secret")]
  public IActionResult GetSecret() 
  {
    var name = User.FindFirst(ClaimTypes.Name)?.Value;
    var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    return Ok("Hello " + name + " with the id of " + id);
  }
}
