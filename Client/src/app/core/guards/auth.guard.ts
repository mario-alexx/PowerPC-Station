import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';
import { map, of } from 'rxjs';

/**
 * Guard to protect routes by ensuring that the user is authenticated.
 * If the user is not authenticated, they will be redirected to the login page.
 * 
 * @param route - The activated route that is being accessed.
 * @param state - The state of the router at the time of activation, containing the target URL.
 * @returns An Observable that emits `true` if the user is authenticated, or `false` if they are not.
*/
export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const router = inject(Router);

  if(accountService.currentUser())
  {
    return of(true); // User is already authenticated
  }
  else 
  {
    return accountService.getAuthState().pipe(
      map(auth => {
        if(auth.isAuthenticated) 
        {
          return true; // User is already authenticated
        }
        else 
        {
          router.navigate(['/account/login'], {queryParams: {returnUrl: state.url}});
          return false; // User is not authenticated
        }
      })
    );
  }
};
