import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { DeliveryMethod } from '../../shared/models/deliveryMethod';
import { map, Observable, of } from 'rxjs';

/**
 * Service to handle checkout processes such as retrieving delivery methods.
 */
@Injectable({
  providedIn: 'root'
})
export class CheckoutService {

  /** Base URL for the API used to perform HTTP requests. */ 
  baseUrl = environment.apiUrl;

  /** Injected HttpClient service for making API requests. */
  private http = inject(HttpClient);

  /** Array to store the available delivery methods.
   * It is cached after the initial retrieval to avoid redundant API calls.
   */
  deliveryMethod: DeliveryMethod[] = [];
  
  /**
   * Fetches the available delivery methods from the server.
   * If delivery methods have already been fetched, returns the cached version.
   * @returns An observable that emits a list of sorted delivery methods.
  */
  getDeliveryMethods(): Observable<DeliveryMethod[]>
  {
    if(this.deliveryMethod.length > 0) return of(this.deliveryMethod);
    return this.http.get<DeliveryMethod[]>(this.baseUrl + 'payments/delivery-methods').pipe(
      map(methods => {
        this.deliveryMethod = methods.sort( (a, b) => b.price - a.price );
        return methods;
      })
    );
  }
}
