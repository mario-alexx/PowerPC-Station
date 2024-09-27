import { Pipe, PipeTransform } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';
import { PaymentSummary } from '../models/order';

/**
 * Pipe to transform payment card information into a formatted string for display.
 */
@Pipe({
  name: 'paymentCard',
  standalone: true
})
export class PaymentCardPipe implements PipeTransform {

  /**
   * Transforms the provided payment card data into a string format for display.
   * @param value - The payment method preview or payment summary to be transformed.
   * @param args - Additional arguments (not used in this case).
   * @returns The transformed payment card information or `null` if no valid data is provided.
   */
  transform(value?: ConfirmationToken['payment_method_preview'] | PaymentSummary, ...args: unknown[]): unknown {
    if(value && 'card' in value) 
    {
      const {brand, last4, exp_month, exp_year} = (value as ConfirmationToken['payment_method_preview']).card!;
      return `${brand.toUpperCase()} **** **** **** ${last4}, Exp: ${exp_month}/${exp_year}`;
    }
    else if(value && 'last4' in value)
    {
      const {brand, last4, expMonth, expYear} = value as PaymentSummary;
      return `${brand.toUpperCase()} **** **** **** ${last4}, Exp: ${expMonth}/${expYear}`
    }
    else 
    {
      return 'Unknown payment method';
    }
  }
}
