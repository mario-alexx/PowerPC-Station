import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { MatCard } from '@angular/material/card';
import { Router } from '@angular/router';

/**
 * Component responsible for displaying server error details.
*/
@Component({
  selector: 'app-server-error',
  standalone: true,
  imports: [
    MatCard
  ],
  templateUrl: './server-error.component.html',
  styleUrl: './server-error.component.scss'
})
export class ServerErrorComponent {
  /**
   * The error object that holds the server error details.
   * @type {any | undefined}
  */
  error?: any;

  /**
   * Constructor that retrieves the error from the router state.
   * @param {Router} router - The Angular Router used to get the current navigation state.
  */
  constructor(private router: Router) 
  {
    const navigation = this.router.getCurrentNavigation();
    // Retrieves the 'error' object from the router's navigation state (if available)
    this.error = navigation?.extras.state?.['error'];
  }
}
