import { Pipe, PipeTransform } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';

/**
 * Pipe to transform payment card information into a formatted string for display.
 */
@Pipe({
  name: 'paymentCard',
  standalone: true
})
export class PaymentCardPipe implements PipeTransform {

  /**
   * Transforms the payment method object into a formatted payment card string.
   * 
   * @param value - The payment method preview object containing card details.
   * @returns A formatted string of the card brand, last 4 digits, and expiration date or 'Unknown payment method' if data is missing.
  */
  transform(value?: ConfirmationToken['payment_method_preview'], ...args: unknown[]): unknown {
    if(value?.card) 
    {
      const {brand, last4, exp_month, exp_year} = value.card;
      return `${brand.toUpperCase()} **** **** **** ${last4}, Exp: ${exp_month}/${exp_year}`;
    }
    else 
    {
      return 'Unknown payment method';
    }
  }
}
