import { Component, inject, OnInit, output } from '@angular/core';
import { CheckoutService } from '../../../core/services/checkout.service';
import { MatRadioModule } from '@angular/material/radio';
import { CurrencyPipe } from '@angular/common';
import { CartService } from '../../../core/services/cart.service';
import { DeliveryMethod } from '../../../shared/models/deliveryMethod';

/**
 * Component for selecting the delivery method during checkout
 */
@Component({
  selector: 'app-checkout-delivery',
  standalone: true,
  imports: [
    MatRadioModule,
    CurrencyPipe
  ],
  templateUrl: './checkout-delivery.component.html',
  styleUrl: './checkout-delivery.component.scss'
})
export class CheckoutDeliveryComponent implements OnInit{
  /** Injected instance of CheckoutService to handle delivery method data. */
  checkoutService = inject(CheckoutService);

  /** Injected instance of CartService to manage cart and delivery method selection. */
  cartService = inject(CartService);
  
  /** Event emitter that signals when the delivery method selection is complete. */
  deliveryComplete = output<Boolean>();

  /**
   * Initializes the component and fetches available delivery methods.
   * If a delivery method is already selected in the cart, it is set as the selected delivery method.
  */
  ngOnInit(): void {
    this.checkoutService.getDeliveryMethods().subscribe({
      next: methods => {
        if(this.cartService.cart()?.deliveryMethodId) 
        {
          const method = methods.find(x => x.id === this.cartService.cart()?.deliveryMethodId);
          if(method) 
          {
            this.cartService.selectedDelivery.set(method);
            this.deliveryComplete.emit(true);
          }
        }
      }
    });
  }

  /**
   * Updates the selected delivery method based on the user's choice.
   * 
   * @param method The delivery method selected by the user.
  */
  updateDeliveryMethod(method: DeliveryMethod): void
  {
    this.cartService.selectedDelivery.set(method);
    const cart = this.cartService.cart();
    if(cart)
    {
      cart.deliveryMethodId = method.id;
      this.cartService.setCart(cart);
      this.deliveryComplete.emit(true);
    }
  }
}
