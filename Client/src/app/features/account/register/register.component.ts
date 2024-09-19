import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { AccountService } from '../../../core/services/account.service';
import { Router } from '@angular/router';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { JsonPipe } from '@angular/common';
import { TextInputComponent } from "../../../shared/components/text-input/text-input.component";

/**
 * RegisterComponent handles the user registration process, including form validation and submission.
*/
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCard,
    MatFormField,
    MatLabel,
    MatInput,
    MatButton,
    JsonPipe,
    MatError,
    TextInputComponent
],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  /** Injects FormBuilder to create and manage the registration form. */
  private fb = inject(FormBuilder);

  /** Injects AccountService to handle user registration operations. */
  private accountService = inject(AccountService);

  /** Injects Router to handle navigation after a successful registration. */
  private router = inject(Router);

  /** Injects SnackbarService to display feedback messages to the user. */
  private snack = inject(SnackbarService);

  /** Stores any validation errors encountered during the registration process. */
  validationErrors?: string[];

  /**
   * The reactive form group for registration, containing first name, last name, email, and password fields.
   * - `firstName`: Required.
   * - `lastName`: Required.
   * - `email`: Required and must follow the email format.
   * - `password`: Required and must meet the specified pattern (at least one uppercase letter, one lowercase letter, one number, one special character, and a minimum length of 6 characters).
  */
  registerForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required,Validators.pattern(/^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{6,}$/)] ]
  });

  /**
   * Handles form submission by calling the registration method from AccountService.
   * Displays appropriate feedback and navigates the user upon successful registration.
  */
  onSubmit() 
  {
    this.accountService.register(this.registerForm.value).subscribe({
      next: () => {
        this.snack.success('Registration successful - you can now login');
        this.router.navigateByUrl('account/login');
      },
      error: errors => this.validationErrors = errors
    });
  }
}
