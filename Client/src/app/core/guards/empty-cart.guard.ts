import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CartService } from '../services/cart.service';
import { SnackbarService } from '../services/snackbar.service';

/**
 * Guard to ensure that the user's cart is not empty before allowing access to a route.
 * If the cart is empty, the user will be redirected to the cart page with an error message.
 * 
 * @param route - The activated route that is being accessed.
 * @param state - The state of the router at the time of activation, containing the target URL.
 * @returns A boolean or Observable that emits `true` if the cart is not empty, or `false` if it is empty.
*/
export const emptyCartGuard: CanActivateFn = (route, state) => {
  const cartService = inject(CartService);
  const snack = inject(SnackbarService);
  const router = inject(Router);

  // Check if the cart exists and if it contains items
  if(!cartService.cart() || cartService.cart()?.items.length === 0)
  {
    snack.error('Your cart is empty');
    router.navigateByUrl('/cart');
    return false;
  }
  return true;
};
