import { inject, Injectable } from '@angular/core';
import { CartService } from './cart.service';
import { Observable, of } from 'rxjs';
import { Cart } from '../../shared/models/cart';

/**
 * Service responsible for initializing the application state, specifically for handling the shopping cart.
*/
@Injectable({
  providedIn: 'root'
})
export class InitService {

  /** Injects the CartService to interact with cart operations */
  private cartService = inject(CartService);

  /**
   * Initializes the application by retrieving the cart if it exists in localStorage.
   * @returns An observable of the Cart if a cart ID is found, or null if no cart exists.
  */
  init(): Observable<Cart> | Observable<null>
  {
    const cartId = localStorage.getItem('cart_id');
    const cart$ = cartId ? this.cartService.getCart(cartId) : of(null);
    return cart$;
  }
}
