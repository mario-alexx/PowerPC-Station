import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Order, OrderToCreate } from '../../shared/models/order';
import { Observable } from 'rxjs';

/**
 * Service responsible for managing orders in the application.
*/
@Injectable({
  providedIn: 'root'
})
export class OrderService {
  
  /** The base URL for the API */ 
  baseUrl = environment.apiUrl;

  /** HTTP client to interact with the API */
  private http = inject(HttpClient);

  /** Flag indicating if the order has been completed */
  orderComplete = false;

  /**
   * Creates a new order by posting the order details to the server.
   * @param orderToCreate The details of the order to be created.
   * @returns An observable of the created order.
  */
  createOrder(orderToCreate: OrderToCreate): Observable<Order>
  {
    return this.http.post<Order>(this.baseUrl + 'orders', orderToCreate);
  }

  /**
   * Retrieves all orders for the currently logged-in user.
   * @returns An observable list of orders for the user.
  */
  getOrdersForUser(): Observable<Order[]>
  {
    return this.http.get<Order[]>(this.baseUrl + 'orders');
  }

  /**
   * Retrieves detailed information about a specific order by its ID.
   * @param id The ID of the order to retrieve.
   * @returns An observable of the order details.
  */
  getOrderDetailed(id: number): Observable<Order>
  {
    return this.http.get<Order>(this.baseUrl + 'orders/' + id);
  }
}
