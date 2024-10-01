import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { RouterLink } from '@angular/router';
import { CartService } from '../../../core/services/cart.service';
import { CurrencyPipe, Location, NgIf } from '@angular/common';
import { StripeService } from '../../../core/services/stripe.service';
import { firstValueFrom } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { MatIcon } from '@angular/material/icon';

/**
 * This component is responsible for displaying the summary of the order.
 * It retrieves the shopping cart data from the `CartService`.
*/
@Component({
  selector: 'app-order-summary',
  standalone: true,
  imports: [
    MatButton,
    RouterLink,
    MatFormField,
    MatLabel,
    MatInput,
    CurrencyPipe,
    FormsModule,
    NgIf,
    MatIcon
  ],
  templateUrl: './order-summary.component.html',
  styleUrl: './order-summary.component.scss'
})
export class OrderSummaryComponent {
  /** Injects the CartService to access cart-related operations. */
  cartService = inject(CartService);
  
  /**Injected instance of Location service to handle browser navigation and location actions. */
  location = inject(Location);

  /** Injects the StripeService */
  private stripeService = inject(StripeService);

  /** Optional coupon code entered by the user. */
  code?: string;
  
  /** Applies the coupon code provided by the user. */
  applyCouponCode(): void
  {
    if(!this.code) return;
    this.cartService.applyDiscount(this.code).subscribe({
      next: async coupon => {
        const cart = this.cartService.cart();
        if(cart)
        {
          cart.coupon = coupon;
          this.cartService.setCart(cart);
          this.code = undefined;
        }
        if(this.location.path() === '/checkout')
        {
          await firstValueFrom(this.stripeService.createOrUpdatePaymentIntent());
        }
      }
    });
  }

  /**
   * Removes the currently applied coupon code.
   * @returns A promise that resolves when the coupon code is removed.
  */
  async removeCouponCode(): Promise<void>
  {
    const cart = this.cartService.cart();
    if(!cart) return;

    if(cart.coupon) cart.coupon = undefined;
    this.cartService.setCart(cart);
    
    if(this.location.path() === '/checkout')
    {
      await firstValueFrom(this.stripeService.createOrUpdatePaymentIntent());
    }
  }
}
