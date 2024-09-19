import { HttpInterceptorFn } from '@angular/common/http';

/**
 * Intercepts HTTP requests to include credentials (such as cookies) in cross-origin requests.
 * This is useful when the backend expects authentication via cookies or other credentials.
 * 
 * @param req - The outgoing HTTP request.
 * @param next - The next interceptor or the HTTP handler to process the request.
 * @returns An Observable that proceeds with the modified request.
*/
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // Clone the request to include credentials
  const clonedRequest = req.clone({
    withCredentials: true
  });
  
  // Pass the cloned request to the next handler
  return next(clonedRequest);
};
