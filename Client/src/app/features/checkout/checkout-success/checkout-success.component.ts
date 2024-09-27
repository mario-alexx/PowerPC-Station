import { Component, inject, OnDestroy } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { SignalrService } from '../../../core/services/signalr.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CurrencyPipe, DatePipe, NgIf } from '@angular/common';
import { AddressPipe } from '../../../shared/pipes/address.pipe';
import { PaymentCardPipe } from '../../../shared/pipes/payment-card.pipe';
import { OrderService } from '../../../core/services/order.service';

/**
 * Component that handles the success scenario after completing the checkout process.
 * Resets the order completion state and SignalR order signal when destroyed.
*/
@Component({
  selector: 'app-checkout-success',
  standalone: true,
  imports: [
    MatButton,
    RouterLink,
    MatProgressSpinnerModule,
    DatePipe,
    AddressPipe,
    CurrencyPipe,
    PaymentCardPipe,
    NgIf
  ],
  templateUrl: './checkout-success.component.html',
  styleUrl: './checkout-success.component.scss'
})
export class CheckoutSuccessComponent implements OnDestroy {

  /** Injects the SignalR service to handle real-time signals */
  signalrService = inject(SignalrService);

  /** Injects the order service */
  private orderService = inject(OrderService);
  
  /**
   * Lifecycle hook that is called when the component is destroyed.
   * Resets the `orderComplete` state to false and clears the SignalR order signal.
  */
  ngOnDestroy(): void 
  { 
    this.orderService.orderComplete = false;
    this.signalrService.orderSignal.set(null);
  }
}
