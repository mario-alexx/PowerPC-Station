import { Component, inject, input } from '@angular/core';
import { CartItem } from '../../../shared/models/cart';
import { RouterLink } from '@angular/router';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { CurrencyPipe } from '@angular/common';
import { CartService } from '../../../core/services/cart.service';

/**
 * Component responsible for displaying and managing an individual item in the cart.
*/
@Component({
  selector: 'app-cart-item',
  standalone: true,
  imports: [
    RouterLink,
    MatButton,
    MatIcon,
    CurrencyPipe
  ],
  templateUrl: './cart-item.component.html',
  styleUrl: './cart-item.component.scss'
})
export class CartItemComponent {
  /** The cart item that is being displayed and managed. */ 
  item = input.required<CartItem>();
  /** Injects the CartService to handle cart-related actions. */
  cartService = inject(CartService);

  /**
   * Increments the quantity of the current cart item by adding it to the cart.
  */
  incrementQuantity(): void 
  {
    this.cartService.addItemToCart(this.item());
  }

  /**
   * Decrements the quantity of the current cart item by removing one unit.
  */
  decrementQuantity(): void 
  {
    this.cartService.removeItemFromCart(this.item().productId);
  }

  /**
   * Removes the current cart item entirely from the cart.
  */
  removeItemFromCart(): void 
  {
    this.cartService.removeItemFromCart(this.item().productId, this.item().quantity);
  }
}
