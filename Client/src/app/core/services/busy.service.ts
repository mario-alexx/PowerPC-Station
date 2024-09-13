import { Injectable } from '@angular/core';

/**
 * Service to manage the loading state of the application.
 *
 * The `BusyService` keeps track of the number of ongoing HTTP requests and toggles the loading state.
 * It is used to show or hide a loading indicator (e.g., a spinner) during the application's HTTP activity.
 */
@Injectable({
  providedIn: 'root'
})
export class BusyService {
  /** Indicates if the application is in a loading state. */
  loading = false;
  /** Keeps count of ongoing requests to manage when to show or hide the loading indicator. */
  busyRequestCount = 0;

  /**
   * Marks the start of a new request by increasing the request count and setting the loading state to `true`.
  */
  busy(): void
  {
    this.busyRequestCount++;
    this.loading = true;
  }

  /**
   * Marks the completion of a request by decreasing the request count.
   * When all requests are completed, the loading state is set to `false`.
  */
  idle(): void
  {
    this.busyRequestCount--;
    if(this.busyRequestCount <= 0)
    {
      this.busyRequestCount = 0;
      this.loading = false;
    }
  }
}
