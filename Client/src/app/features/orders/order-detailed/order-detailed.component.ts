import { Component, inject, OnInit } from '@angular/core';
import { OrderService } from '../../../core/services/order.service';
import { environment } from '../../../../environments/environment';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Order } from '../../../shared/models/order';
import { MatCard, MatCardModule } from '@angular/material/card';
import { MatButton } from '@angular/material/button';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { AddressPipe } from "../../../shared/pipes/address.pipe";
import { PaymentCardPipe } from "../../../shared/pipes/payment-card.pipe";

/**
 * Component that displays detailed information about a specific order.
*/
@Component({
  selector: 'app-order-detailed',
  standalone: true,
  imports: [
    MatCardModule,
    MatButton,
    DatePipe,
    CurrencyPipe,
    AddressPipe,
    PaymentCardPipe,
    RouterLink
],
  templateUrl: './order-detailed.component.html',
  styleUrl: './order-detailed.component.scss'
})
export class OrderDetailedComponent implements OnInit{

  /** Injects the order service */
  private orderService = inject(OrderService);

  /** Injects the activated route service to access route parameters */
  private activatedRoute = inject(ActivatedRoute);

  /** Stores the detailed information of the order */
  order?: Order;

  /**
   * Lifecycle hook that is called after component initialization.
   * Loads the detailed information of the order based on the route parameter.
   */
  ngOnInit(): void 
  {
    this.loadOrder();  
  }

  /**
   * Retrieves the order details using the ID from the route parameters.
   * Updates the `order` property with the retrieved data.
  */
  loadOrder()
  {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if(!id) return; 
    this.orderService.getOrderDetailed(+id).subscribe({
      next: order => this.order = order
    })
  }
}
