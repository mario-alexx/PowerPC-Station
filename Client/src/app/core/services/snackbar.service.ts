import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

/**
 * Service to display snack bar notifications for success and error messages.
 */
@Injectable({
  providedIn: 'root'
})
export class SnackbarService {

  /** Injects the Angular Material `MatSnackBar` service for showing snack bar messages. */
  private snackbar = inject(MatSnackBar);

  
  /**
   * Displays an error message in a snack bar. 
   * @param {string} message - The error message to display.
  */
  error(message: string) 
  {
    this.snackbar.open(message, 'Close', {
      duration: 5000,
      panelClass: ['snack-error']
    });
  }

   /**
   * Displays a success message in a snack bar.
   * @param {string} message - The success message to display.
   */
  success(message: string) 
  {
    this.snackbar.open(message, 'Close', {
      duration: 5000,
      panelClass: ['snack-success']
    });
  }
}
