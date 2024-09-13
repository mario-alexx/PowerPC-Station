using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Handles API requests related to shopping carts, including retrieval, updates, and deletion.
/// </summary>
public class CartController(ICartService cartService) : BaseApiController
{ 
  /// <summary>
  /// Retrieves a shopping cart by its ID.
  /// </summary>
  /// <param name="id">The ID of the shopping cart to retrieve.</param>
  /// <returns>An ActionResult containing the shopping cart if found, or an appropriate error response.</returns>
  [HttpGet]
  public async Task<ActionResult<ShoppingCart>> GetCartById(string id) 
  {
    var cart = await cartService.GetCartAsync(id);
    
    return Ok(cart ?? new ShoppingCart{Id = id});
  }

  /// <summary>
  /// Updates or creates a shopping cart.
  /// </summary>
  /// <param name="cart">The shopping cart to update or create.</param>
  /// <returns>An ActionResult containing the updated shopping cart, or an appropriate error response.</returns>
  [HttpPost]
  public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart) 
  {
    var updatedCart = await cartService.SetCartAsync(cart);

    if(updatedCart == null) return BadRequest("Problem with cart");

    return updatedCart;
  }
  
  /// <summary>
  /// Deletes a shopping cart by its ID.
  /// </summary>
  /// <param name="id">The ID of the shopping cart to delete.</param>
  /// <returns>An ActionResult indicating the result of the delete operation.</returns>
  [HttpDelete] 
  public async Task<ActionResult> DeleteCart(string id) 
  {
    var result = await cartService.DeleteCartAsync(id);

    if(!result) return BadRequest("Problem deleting cart");

    return Ok();
  }
}
