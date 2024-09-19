import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { AccountService } from '../../../core/services/account.service';
import { ActivatedRoute, Router } from '@angular/router';

/**
 * LoginComponent handles the user login process, including form submission and redirection.
*/
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCard,
    MatFormField,
    MatInput,
    MatLabel,
    MatButton
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  /** Injects FormBuilder to create and manage the login form. */ 
  private fb = inject(FormBuilder);
  
  /** Injects AccountService to handle authentication operations. */
  private accountService = inject(AccountService);
  
  /** Injects Router to handle navigation after a successful login. */
  private router = inject(Router);
  
  /** Injects ActivatedRoute to get the return URL for redirection. */
  private activatedRoute = inject(ActivatedRoute);
  
  /** Default URL to return to after login, or from query parameters if provided. */
  returnUrl = '/shop';

  /**
   * Initializes the component and checks if a return URL is provided in the query parameters.
  */
  constructor() 
  {
    const url = this.activatedRoute.snapshot.queryParams['returnUrl'];
    if(url) this.returnUrl = url;
  }

  /**
   * The reactive form group for login, containing email and password fields.
  */
  loginForm = this.fb.group({
    email: [''],
    password: ['']
  });
  
  /**
   * Handles form submission by calling the login method from AccountService.
   * If login is successful, fetches user info and navigates to the return URL.
  */
  onSubmit() 
  {
    this.accountService.login(this.loginForm.value).subscribe({
      next: () => {
        this.accountService.getUserInfo().subscribe();
        this.router.navigateByUrl('/shop');
      }
    });
  }
}
