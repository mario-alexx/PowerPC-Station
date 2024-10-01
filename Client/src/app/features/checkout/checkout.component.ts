import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { OrderSummaryComponent } from "../../shared/components/order-summary/order-summary.component";
import { MatStepper, MatStepperModule } from '@angular/material/stepper'; 
import { MatButton } from '@angular/material/button';
import { Router, RouterLink } from '@angular/router';
import { StripeService } from '../../core/services/stripe.service';
import { ConfirmationToken, StripeAddressElement, StripeAddressElementChangeEvent, StripePaymentElement, StripePaymentElementChangeEvent } from '@stripe/stripe-js';
import { SnackbarService } from '../../core/services/snackbar.service';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox'
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { Address } from '../../shared/models/user';
import { AccountService } from '../../core/services/account.service';
import { firstValueFrom } from 'rxjs';
import { CheckoutDeliveryComponent } from "./checkout-delivery/checkout-delivery.component";
import { CheckoutReviewComponent } from "./checkout-review/checkout-review.component";
import { CartService } from '../../core/services/cart.service';
import { CurrencyPipe, JsonPipe } from '@angular/common';
import { MatProgressSpinnerModule} from '@angular/material/progress-spinner'
import { OrderToCreate, ShippingAddress } from '../../shared/models/order';
import { OrderService } from '../../core/services/order.service';

/**  Component for handling the entire checkout process, including payment, address, and delivery method steps. */
@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [
    OrderSummaryComponent,
    MatStepperModule,
    MatButton,
    RouterLink,
    MatCheckboxModule,
    CheckoutDeliveryComponent,
    CheckoutReviewComponent,
    CurrencyPipe,
    JsonPipe,
    MatProgressSpinnerModule
],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
  
})
export class CheckoutComponent implements OnInit, OnDestroy{
  
  /** Injected instance of StripeService to manage Stripe payment elements. */
  private stripeService = inject(StripeService);

  /** Injected instance of SnackbarService to display notifications. */ 
  private snackBar = inject(SnackbarService);

  /** Injected instance of AccountService to manage user account information. */
  private accountService = inject(AccountService);

  /** Injected instance of Router for navigation after the checkout process. */
  private router = inject(Router);

  /** Injected instance of CartService to handle the cart and delivery method selection. */
  cartService = inject(CartService);

  /** Injects the order service */
  orderService = inject(OrderService);

  /** Holds the Stripe address element for address input during checkout. */
  addressElement?: StripeAddressElement;

  /** Holds the Stripe payment element for handling card details. */
  paymentElement?: StripePaymentElement;

  /** Boolean flag to indicate if the user wants to save the shipping address. */
  saveAddress = false;

  /** Tracks the completion status of the address, payment, and delivery steps. */
  completionStatus = signal<{address: boolean, card: boolean, delivery: boolean}>(
    {address: false, card: false, delivery: false}
  );

  /** Stores the confirmation token that will be generated during checkout. */
  confirmationToken?: ConfirmationToken;

  /** Boolean flag to track the loading state during the checkout process. */
  loading = false;

  /** Initializes the Stripe elements and sets up the checkout flow when the component is created. */
  async ngOnInit(): Promise<void>
  {
    try {
      this.addressElement = await this.stripeService.createAddressElement();
      this.addressElement.mount('#address-element');
      this.addressElement.on('change', this.handleAddressChange);

      this.paymentElement = await this.stripeService.createPaymentElement();
      this.paymentElement.mount('#payment-element');  
      this.paymentElement.on('change', this.handlePaymentChange);
    } 
    catch (error: any) {
      this.snackBar.error(error.message);
    }
  }

  /**
   * Handles changes to the address input from the Stripe address element.
   * @param event The event containing address changes.
  */
  handleAddressChange = (event: StripeAddressElementChangeEvent) =>
  {
    this.completionStatus.update(state => {
      state.address = event.complete;
      return state;
    });
  }
  
  /**
   * Handles changes to the payment input from the Stripe payment element.
   * @param event The event containing payment element changes.
  */
  handlePaymentChange = (event: StripePaymentElementChangeEvent) =>
  {
    this.completionStatus.update(state => {
      state.card = event.complete;
      return state;
    });
  }

  /**
   * Handles changes to the delivery method selection.
   * @param event The boolean event representing delivery selection completion.
  */
  handleDeliveryChange = (event: boolean) =>
  { 
    this.completionStatus.update(state => {
      state.delivery = event;
      return state;
    });
  }

  /** Fetches the confirmation token needed to complete the payment process. */
  async getConfirmationToken(): Promise<void>
  {
    try 
    {
      if(Object.values(this.completionStatus()).every(status => status === true)) 
      {
        const result = await this.stripeService.createConfirmationToken();
        if(result.error) throw new Error(result.error.message);
        this.confirmationToken = result.confirmationToken;
        console.log(this.confirmationToken);          
      }
    } 
    catch (error: any) 
    {
      this.snackBar.error(error.message);
    }
  }

  /**
   * Handles changes between the different steps in the checkout process.
   * @param event The event triggered when a new step is selected in the stepper.
  */
  async onStepChange(event: StepperSelectionEvent): Promise<void>
  {
    if(event.selectedIndex === 1) 
    {
      if(this.saveAddress)
      {
        const address = await this.getAddressFromStripeAddress() as Address;
        address && firstValueFrom(this.accountService.updateAddress(address));
      }
    }

    if(event.selectedIndex === 2) 
    {
      await firstValueFrom(this.stripeService.createOrUpdatePaymentIntent());
    }

    if(event.selectedIndex === 3) 
    {
      await this.getConfirmationToken();
    }
  }

  /**
   * Confirms the payment and processes the order once all steps are completed.
   * @param stepper The stepper component to control the flow of the checkout process.
  */
  async confirmPayment(stepper: MatStepper): Promise<void>
  {
    this.loading = true;
    try {
      if(this.confirmationToken) 
      {
        const result = await this.stripeService.confirmPayment(this.confirmationToken);
        
        if(result.paymentIntent?.status === 'succeeded')
        {
          const order = await this.createOrderModel();
          const orderResult = await firstValueFrom(this.orderService.createOrder(order));

          if(orderResult)
          {
            this.orderService.orderComplete = true;
            this.cartService.deleteCart();
            this.cartService.selectedDelivery.set(null);
            this.router.navigateByUrl('/checkout/success');
          }
          else 
          {
            throw new Error('Order creation failed');
          }         
        }
        else if(result.error) 
        {
          throw new Error(result.error.message);
        }
        else 
        {
          throw new Error('Something went wrong');
        }
      }
    } catch (error: any) {
      this.snackBar.error(error.message || 'Something went wrong'); 
      stepper.previous();
    }
    finally {
      this.loading = false;
    }
  }

  /**
   * Creates an order model using the current cart, shipping address, and payment summary.
   * Throws an error if any required information is missing.
   * @returns A promise that resolves to the `OrderToCreate` object.
   * @throws Error if cart ID, delivery method, shipping address, or card details are missing.
  */
  private async createOrderModel(): Promise<OrderToCreate>
  {
    const cart = this.cartService.cart();
    const shippingAddress = await this.getAddressFromStripeAddress() as ShippingAddress;
    const card = this.confirmationToken?.payment_method_preview.card;

    if(!cart?.id || !cart.deliveryMethodId || !shippingAddress || !card)
    {
      throw new Error('Problem creating order');
    }

    const order: OrderToCreate =
    {
      cartId: cart.id,
      paymentSummary: {
        last4: +card.last4,
        brand: card.brand,
        expMonth: card.exp_month,
        expYear: card.exp_year
      },
      deliveryMethodId:cart.deliveryMethodId,
      shippingAddress,
      discount: this.cartService.totals()?.discount
    }
    return order;
  }

  /**
   * Retrieves the shipping address from Stripe's address element.
   * @returns A promise that resolves to the `Address` or `ShippingAddress` object, or null if not available.
  */
  private async getAddressFromStripeAddress(): Promise<Address | ShippingAddress | null>
  {
    const result = await this.addressElement?.getValue();
    const address = result?.value.address;

    if(address)
    {
      return {
        name: result.value.name,
        line1: address.line1,
        line2: address.line2 || undefined,
        city: address.city,
        country: address.country,
        state: address.state,
        postalCode: address.postal_code
      }
    }
  
    else return null;
  }

  /**
   * Handles changes to the "Save Address" checkbox.
   * @param event The checkbox change event indicating whether to save the address.
  */
  onSaveAddressCheckboxChange(event: MatCheckboxChange): void 
  {
    this.saveAddress = event.checked;
  }

  /** Cleans up resources, such as Stripe elements, when the component is destroyed. */
  ngOnDestroy(): void 
  {
    this.stripeService.disposeElements();
  }
} 
