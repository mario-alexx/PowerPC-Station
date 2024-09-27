import { Component, inject, OnInit } from '@angular/core';
import { OrderService } from '../../core/services/order.service';
import { Order } from '../../shared/models/order';
import { RouterLink } from '@angular/router';
import { CurrencyPipe, DatePipe } from '@angular/common';

/**
 * Component that displays a list of orders for the logged-in user.
*/
@Component({
  selector: 'app-order',
  standalone: true,
  imports: [
    RouterLink,
    DatePipe,
    CurrencyPipe
  ],
  templateUrl: './order.component.html',
  styleUrl: './order.component.scss'
})
export class OrderComponent implements OnInit{
  
  /** Injects the order service */ 
  private orderService = inject(OrderService);

  /** Stores the list of orders */
  orders: Order[] = [];

  /**
   * Lifecycle hook that is called after component initialization.
   * Retrieves the user's orders and stores them in the `orders` array.
  */
  ngOnInit(): void 
  {
    this.orderService.getOrdersForUser().subscribe({
      next: orders => this.orders = orders
    })
  }

}
