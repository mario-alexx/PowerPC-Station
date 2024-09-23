import { Pipe, PipeTransform } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';

/**
 * Pipe to transform a shipping address into a formatted string for display.
 */
@Pipe({
  name: 'address',
  standalone: true
})
export class AddressPipe implements PipeTransform {
  
  /**
   * Transforms the shipping object into a formatted address string.
   * 
   * @param value - The shipping object containing the address and name.
   * @returns A formatted string of the shipping address or 'Unknown address' if data is missing.
  */
  transform(value?: ConfirmationToken['shipping'], ...args: unknown[]): unknown {
    if(value?.address && value.name)
    {
      const {line1, line2, city, state, country, postal_code} = value.address;
      return `${value.name}, ${line1}${line2 ? ', ' + line2 : ''}, ${city},
       ${state}, ${postal_code}, ${country}`;
    }
    else 
    {
      return 'Unknown address';
    }
  }

}
