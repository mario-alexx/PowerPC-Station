/**
 * Represents an order with details about items, shipping, payment, and status.
*/
export type Order = {
  /** Unique identifier of the order */
  id: number;
  /** The date the order was placed */
  orderDate: string;
  /** Email of the buyer who placed the order */
  buyerEmail: string;
  /** The shipping address for the order */
  shippingAddress: ShippingAddress;
  /** The delivery method used for the order */
  deliveryMethod: string;
  /** The price of the shipping */
  shippingPrice: number;
  /** Summary of the payment details */
  paymentSummary: PaymentSummary;
  /** The items included in the order */
  orderItems: OrderItem[];
  /** The subtotal cost of the order (excluding shipping) */
  subtotal: number;
  /** The current status of the order */
  status: string;
  /** The total cost of the order (including shipping) */
  total: number;
  /** Identifier for the payment intent associated with the order */
  paymentIntentId: string;
}

/**
 * Represents the shipping address for an order.
*/
export type ShippingAddress = {
  /** The name of the recipient */
  name: string;
  /** The first line of the address (e.g., street, house number) */
  line1: string;
  /** The second line of the address (e.g., apartment, suite number) */
  line2?: string;
  /** The city of the address */
  city: string;
  /** The state or region of the address */
  state: string;
  /** The postal or ZIP code */
  postalCode: string;
  /** The country of the address */
  country: string;
}

/**
 * Summary of payment information, including card details.
*/
export type PaymentSummary = {
  /** The last 4 digits of the payment card */
  last4: number;
  /** The brand of the payment card (e.g., Visa, MasterCard) */
  brand: string;
  /** The expiration month of the payment card */
  expMonth: number;
  /** The expiration year of the payment card */
  expYear: number;
}

/**
 * Represents an item in the order.
*/
export type OrderItem = {
  /** The ID of the product ordered */
  productId: number;
  /** The name of the product ordered */
  productName: string;
  /** The URL to the product's picture */
  pictureUrl: string;
  /** The price of a single unit of the product */
  price: number;
  /** The quantity of the product ordered */
  quantity: number;
}

/**
 * DTO used to create a new order, containing the necessary details.
*/
export type OrderToCreate = {
  /** The cart ID associated with the order */
  cartId: string;
  /** The ID of the delivery method selected */
  deliveryMethodId: number;
  /** The shipping address for the order */
  shippingAddress: ShippingAddress;
  /** Summary of payment information */
  paymentSummary: PaymentSummary;
}