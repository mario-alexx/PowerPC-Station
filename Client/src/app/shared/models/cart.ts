import {nanoid} from 'nanoid';

/**
 * Represents the structure of a shopping cart, containing an ID and a list of items.
*/
export type CartType = 
{
  /** The unique identifier for the cart. */
  id: string;
  
  /** The list of items in the cart. */
  items: CartItem[];

  /** The ID of the selected delivery method. */
  deliveryMethodId?: number;
    
  /** The ID of the payment intent used for payment. */
  paymentIntentId?: string;
  
  /** The secret key for confirming the payment intent. */
  clientSecret?: string;

  /** Optional coupon applied to the cart. */
  coupon?: Coupon;
}

/**
 * Represents a shopping cart item, including product details such as ID, name, price, and quantity.
 */
export type CartItem = 
{
  /** The unique identifier for the product. */
  productId: number;

  /** The name of the product. */
  productName: string;

  /** The price of the product. */
  price: number;

  /** The quantity of the product in the cart. */
  quantity: number;

  /** The URL of the product's image. */
  pictureUrl: string;

  /** The brand of the product. */
  brand: string;
  
  /** The type or category of the product. */
  type: string;
}

/**
 * A class representing a shopping cart.
 * It includes an automatically generated ID and a list of cart items.
*/
export class Cart implements CartType 
{
  /** The unique identifier for the cart, generated using `nanoid`. */
  id = nanoid();
  
  /** The list of items in the shopping cart, initialized as an empty array. */
  items: CartItem[] = [];

  /** The ID of the selected delivery method. */
  deliveryMethodId?: number;

  /** The ID of the payment intent used for payment. */
  paymentIntentId?: string;

  /** The secret key for confirming the payment intent. */
  clientSecret?: string;

  /** Optional coupon applied to the cart. */
  coupon?: Coupon;
}

/** Type representing a coupon. */
export type Coupon = {
  /** Name of the coupon. */
  name: string;
  
  /** Amount to be deducted from the total. */
  amountOff?: number;
  
  /** Percentage to be deducted from the total. */
  percentOff?: number;
  
  /** Promotion code associated with the coupon. */
  promotionCode: string;
  
  /** Unique identifier for the coupon. */
  couponId: string;
}