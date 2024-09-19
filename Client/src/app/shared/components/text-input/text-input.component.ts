import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from '@angular/forms';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';

/**
 * Component for a text input field with label and type customization.
 * Implements the ControlValueAccessor interface to allow Angular forms to control the value of the input.
*/
@Component({
  selector: 'app-text-input',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormField,
    MatInput,
    MatError,
    MatLabel
  ],
  templateUrl: './text-input.component.html',
  styleUrl: './text-input.component.scss'
})
export class TextInputComponent implements ControlValueAccessor{
  
  /** Label for the text input field. @default '' */
  @Input() label = '';

  /**Type of the input field (e.g., 'text', 'password'). 
   * @default 'text'
  */
  @Input() type = 'text';

  /**
   * Constructor that injects NgControl to bind the form control to the input component.
   * @param controlDir - The NgControl instance associated with this component.
  */
  constructor(@Self() public controlDir: NgControl) 
  {
    this.controlDir.valueAccessor = this;
  }
  
  writeValue(obj: any): void 
  {
    
  }
  registerOnChange(fn: any): void 
  {
    
  }
  registerOnTouched(fn: any): void 
  {
    
  }

  /**
   * Getter for the FormControl associated with this component.
   * @returns The FormControl instance bound to this input.
  */
  get control(): FormControl
  {
    return this.controlDir.control as FormControl;
  }
}
