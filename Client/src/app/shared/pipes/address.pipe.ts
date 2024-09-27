import { Pipe, PipeTransform } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';
import { ShippingAddress } from '../models/order';

/**
 * Pipe to transform a shipping address into a formatted string for display.
 */
@Pipe({
  name: 'address',
  standalone: true
})
export class AddressPipe implements PipeTransform {
  
  /**
   * Transforms the provided address data into a string format for display.
   * @param value - The shipping address data to be transformed.
   * @param args - Additional arguments (not used in this case).
   * @returns The transformed address string or `null` if no valid data is provided.
   */
  transform(value?: ConfirmationToken['shipping'] | ShippingAddress, ...args: unknown[]): unknown {
    if(value && 'address' in value && value.name)
    {
      const {line1, line2, city, state, country, postal_code} = (value as ConfirmationToken['shipping'])?.address!;
      return `${value.name}, ${line1}${line2 ? ', ' + line2 : ''}, ${city},
       ${state}, ${postal_code}, ${country}`;
    }
    else if(value && 'line1' in value)
    {
      const {line1, line2, city, state, country, postalCode} = value as ShippingAddress
      return `${value.name}, ${line1}${line2 ? ', ' + line2 : ''}, ${city},
       ${state}, ${postalCode}, ${country}`;
    }
    else 
    {
      return 'Unknown address';
    }
  }
}
