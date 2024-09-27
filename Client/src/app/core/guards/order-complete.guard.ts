import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { OrderService } from '../services/order.service';

/**
 * Guard that checks if the order has been completed.
 * Redirects the user to the shop if the order is not complete.
 * @param route The activated route snapshot.
 * @param state The router state snapshot.
 * @returns True if the order is complete, otherwise navigates to the shop and returns false.
*/
export const orderCompleteGuard: CanActivateFn = (route, state) => {
  const orderService = inject(OrderService);
  const router = inject(Router);

  if(orderService.orderComplete)
  {
    return true;
  }
  else 
  {
    router.navigateByUrl('/shop');
    return false;
  }
};
