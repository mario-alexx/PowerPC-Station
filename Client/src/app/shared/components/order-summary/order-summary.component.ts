import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { RouterLink } from '@angular/router';
import { CartService } from '../../../core/services/cart.service';
import { CurrencyPipe, Location } from '@angular/common';

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
    CurrencyPipe
  ],
  templateUrl: './order-summary.component.html',
  styleUrl: './order-summary.component.scss'
})
export class OrderSummaryComponent {
  /** Injects the CartService to access cart-related operations. */
  cartService = inject(CartService);
  
  /**Injected instance of Location service to handle browser navigation and location actions. */
  location = inject(Location);
}
