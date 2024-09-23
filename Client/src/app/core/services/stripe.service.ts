import { inject, Injectable } from '@angular/core';
import { ConfirmationToken, ConfirmationTokenResult, loadStripe, PaymentIntentResult, Stripe, StripeAddressElement, StripeAddressElementOptions, StripeElements, StripePaymentElement } from '@stripe/stripe-js';
import { environment } from '../../../environments/environment';
import { CartService } from './cart.service';
import { HttpClient } from '@angular/common/http';
import { Cart } from '../../shared/models/cart';
import { firstValueFrom, map, Observable } from 'rxjs';
import { AccountService } from './account.service';

/**
 * Service to manage Stripe payment and address elements, create tokens, and confirm payments.
*/
@Injectable({
  providedIn: 'root'
})
export class StripeService {

  /** Base URL for API requests related to payments. */
  baseUrl = environment.apiUrl;

  /** Injected instance of the CartService to interact with the shopping cart. */
  private cartService = inject(CartService);
  
  /** Injected instance of the AccountService to manage user-related functionality. */ 
  private accountService = inject(AccountService);

  /** Injected instance of HttpClient to perform HTTP requests. */
  private http = inject(HttpClient);
  
  /** Promise to load the Stripe instance asynchronously using the public key from the environment. */ 
  private stripePromise: Promise<Stripe | null>;
  
  /** StripeElements object to manage the Stripe elements like payment and address forms. */
  private elements?: StripeElements;

  /** StripeAddressElement to handle address input within the Stripe form. */
  private addressElement?: StripeAddressElement;

  /** StripePaymentElement to handle payment method inputs (credit card, etc.). */
  private paymentElement?: StripePaymentElement;

  /** Constructor initializes the Stripe instance with the public key. */
  constructor() 
  {
    this.stripePromise = loadStripe(environment.stripePublicKey)
  }

  /**
   * Retrieves the Stripe instance after it has been loaded.
   * @returns A promise that resolves to the Stripe instance or null.
  */
  getStripeInstance(): Promise<Stripe | null>
  {
    return this.stripePromise;
  }

  /**
   * Initializes the Stripe elements, including payment and address fields.
   * @returns A promise that resolves to the initialized StripeElements object.
  */
  async initializeElements(): Promise<StripeElements>
  {
    if(!this.elements) 
    {
      const stripe = await this.getStripeInstance();
      if(stripe) 
      {
        const cart = await firstValueFrom(this.createOrUpdatePaymentIntent());
        this.elements = stripe.elements({
          clientSecret: cart.clientSecret, appearance: {labels: 'floating'}});
      }
      else 
      {
        throw new Error('Stripe has not been loaded');
      }
    }
    return this.elements;
  }

   /**
   * Creates and returns a StripePaymentElement to handle payment input.
   * @returns A promise that resolves to the created StripePaymentElement.
  */
  async createPaymentElement(): Promise<StripePaymentElement>
  {
    if(!this.paymentElement)
    {
      const elements = await this.initializeElements();
      if(elements)
      {
        this.paymentElement = elements.create('payment');
      }
      else 
      {
        throw new Error('Elements instance has not been initialized');
      }
    }
    return this.paymentElement;
  }

  /**
   * Creates and returns a StripeAddressElement to handle address input.
   * @returns A promise that resolves to the created StripeAddressElement.
  */
  async createAddressElement(): Promise<StripeAddressElement>
  {
    if(!this.addressElement)
    {
      const elements = await this.initializeElements();
      if(elements)
      {
        const user = this.accountService.currentUser();
        let defaultValues: StripeAddressElementOptions['defaultValues'] =  {};

        if(user) 
        {
          defaultValues.name = user.firstName + ' ' + user.lastName;
        }

        if(user?.address)
        {
          defaultValues.address = {
            line1: user.address.line1,
            line2: user.address.line2,
            city: user.address.city,
            state: user.address.state,
            country: user.address.country,
            postal_code: user.address.postalCode
          }
        }

        const options: StripeAddressElementOptions = {
          mode: 'shipping',
          defaultValues: defaultValues
        };
        this.addressElement = elements.create('address', options);
      }
      else 
      {
        throw new Error('Elements instance has not been loaded');
      }
    }
    return this.addressElement;
  }

  /**
   * Creates a confirmation token for the payment, required to confirm the payment with Stripe.
   * @returns A promise that resolves to the ConfirmationTokenResult.
  */
  async createConfirmationToken(): Promise<ConfirmationTokenResult>
  {
    const stripe = await this.getStripeInstance();
    const elements = await this.initializeElements();
    const result = await elements.submit();

    if(result.error) throw new Error(result.error.message);
    if(stripe)
    {
      return await stripe.createConfirmationToken({elements});
    }
    else 
    {
      throw new Error('Stripe not available');
    }
  }

  /**
   * Confirms the payment using the confirmation token.
   * @param confirmationToken The confirmation token generated during the payment process.
   * @returns A promise that resolves to the PaymentIntentResult from Stripe.
  */
  async confirmPayment(confirmationToken: ConfirmationToken): Promise<PaymentIntentResult>
  {
    const stripe = await this.getStripeInstance();
    const elements = await this.initializeElements();
    const result = await elements.submit();
    if(result.error) throw new Error(result.error.message);

    const clientSecret = this.cartService.cart()?.clientSecret;

    if(stripe && clientSecret)
    { 
      return await stripe.confirmPayment({
        clientSecret: clientSecret,
        confirmParams: {
          confirmation_token: confirmationToken.id
        },
        redirect: 'if_required'
      });
    }
    else 
    {
      throw new Error('Unable to load stripe');
    }
  }

  /**
   * Creates or updates a payment intent in the system and returns the updated cart with the payment intent details.
   * @returns An observable that emits the updated cart.
  */
  createOrUpdatePaymentIntent(): Observable<Cart>
  {
    const cart = this.cartService.cart();
  
    if(!cart) throw new Error('Problem with cart');

    return this.http.post<Cart>(this.baseUrl + 'payments/' + cart.id, {}).pipe(
      map(cart => {
        this.cartService.setCart(cart);
        return cart;
      })
    );
  }

  /**
   * Disposes of the Stripe elements when they are no longer needed, for cleanup purposes.
  */
  disposeElements(): void
  {
    this.elements = undefined;
    this.addressElement = undefined;
    this.paymentElement = undefined;
  }
}
