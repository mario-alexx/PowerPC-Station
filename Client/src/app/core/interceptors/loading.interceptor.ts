import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { delay, finalize } from 'rxjs';
import { BusyService } from '../services/busy.service';

/**
 * HTTP interceptor that manages the loading state during HTTP requests.
 * @param req - The outgoing HTTP request that is being intercepted.
 * @param next - The next handler in the chain that processes the HTTP request.
 * @returns An Observable representing the HTTP response, with a delay applied and the busy state finalized.
 */
export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  
  const busyService = inject(BusyService);

  busyService.busy();

  return next(req).pipe(
    delay(500),
    finalize(() => busyService.idle())
  );
};
