import { inject, Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Address, User } from '../../shared/models/user';
import { environment } from '../../../environments/environment';
import { map, Observable } from 'rxjs';

/**
 * Service responsible for handling user account-related operations such as login, registration, user info retrieval, and logout.
 * Provides methods to interact with the backend API for authentication and user management.
 * @providedIn 'root'
*/
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  /**
   * Base URL for the API endpoints, configured via environment variables. @type {string}
  */
  baseUrl = environment.apiUrl;

  /**
   * Injects the HttpClient service to perform HTTP requests.
   * @type {HttpClient}
  */
  private http = inject(HttpClient);

  /**
   * Signal that holds the current user information or null if no user is authenticated.
   * @type {signal<User | null>}
  */
  currentUser = signal<User | null>(null);

  /**
   * Logs in a user by sending their credentials to the API and retrieving user information.
   * @param {any} values - The login credentials (email and password).
   * @returns {Observable<User>} - An observable that emits the logged-in user data.
  */
  login(values: any): Observable<User>
  {
    let params = new HttpParams();
    params = params.append('useCookies', true);
    return this.http.post<User>(this.baseUrl + 'login', values, {params} );
  }

  /**
   * Registers a new user by sending their details to the API.
   * @param {any} values - The registration details (first name, last name, email, password).
   * @returns {Observable<Object>} - An observable that emits the registration result.
   */
  register(values: any): Observable<Object>
  {
    return this.http.post(this.baseUrl + 'account/register', values);
  }

  /**
   * Retrieves the current user's information from the API and updates the `currentUser` signal.
   * @returns {Observable<User>} - An observable that emits the user's information.
   */
  getUserInfo(): Observable<User>
  {
    return this.http.get<User>(this.baseUrl + 'account/user-info').pipe(
      map(user => {
        this.currentUser.set(user);
        return user;
      })
    );
  }

  /**
   * Logs out the user by sending a request to the API.
   * @returns {Observable<Object>} - An observable that emits the logout result.
  */
  logout(): Observable<Object>
  {
    return this.http.post(this.baseUrl + 'account/logout', {});
  }

  /**
   * Updates the user's address by sending a `POST` request to the API.
   * @param {Address} address - The new address data to update.
   * @returns {Observable<Object>} - An observable that emits the result of the address update.
  */
  updateAddress(address: Address): Observable<Object>
  {
    return this.http.post(this.baseUrl + 'account/address', address);
  }

  /**
   * Retrieves the user's authentication state from the API.
   * @returns {Observable<{isAuthenticated: boolean}>} - An observable that emits the authentication state.
  */
  getAuthState(): Observable<{isAuthenticated: boolean}>
  {
    return this.http.get<{isAuthenticated: boolean}>(this.baseUrl + 'account/auth-status');
  }
}
