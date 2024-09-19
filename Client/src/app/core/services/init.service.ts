import { inject, Injectable } from '@angular/core';
import { CartService } from './cart.service';
import { forkJoin, Observable, of } from 'rxjs';
import { Cart } from '../../shared/models/cart';
import { AccountService } from './account.service';

/**
 * InitService handles the initialization process of the application by fetching the user and cart data.
*/
@Injectable({
  providedIn: 'root'
})
export class InitService {

  /** Injects the CartService to interact with cart operations */
  private cartService = inject(CartService);

  /** Injects the AccountService to manage user-related operations. */
  private accountService = inject(AccountService);

  /**
   * Initializes the application by fetching the cart and user information.
   * It checks for the existence of a `cart_id` in the local storage and retrieves the associated cart if found.
   * It also retrieves the current user's information if the user is authenticated.
   * @returns An observable that combines both the cart and user information using forkJoin.
  */
  init(): Observable<{}>
  {
    const cartId = localStorage.getItem('cart_id');
    const cart$ = cartId ? this.cartService.getCart(cartId) : of(null);
    return forkJoin({
      cart: cart$,
      user: this.accountService.getUserInfo()
    })
  }
}
