import { Component, inject, input, output } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { BusyService } from '../../../core/services/busy.service';

/**
 * EmptyStateComponent is a reusable UI component that displays an empty state message
 * with an icon and an action button for the user to interact with.
*/
@Component({
  selector: 'app-empty-state',
  standalone: true,
  imports: [
    MatIcon,
    MatButton,
    RouterLink
  ],
  templateUrl: './empty-state.component.html',
  styleUrl: './empty-state.component.scss'
})
export class EmptyStateComponent {

  /** Displays a message in the empty state. */
  busyService = inject(BusyService);
  message = input.required<string>();

  /** Text that appears on the action button. */
  icon = input.required<string>();

  /** Icon that is displayed alongside the message in the empty state. */
  actionText = input.required<string>();
  
  /** Event emitter for the action to be triggered when the action button is clicked. */
  action = output<void>();

  /**
   * Method that triggers the action when the user clicks the action button.
   * It emits the `action` event to notify parent components of the user interaction.
   */
  onAction() 
  {
  this.action.emit();
  }
}
