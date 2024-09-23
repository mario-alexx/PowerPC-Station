/**
 * Represents a delivery method with details such as time and price.
*/
export type DeliveryMethod = {
  /** A short name to identify the delivery method. */
  shortName: string;

  /** The expected time for delivery. */
  deliveryTime: string;

  /** A description of the delivery method. */
  description: string;

  /** The price of the delivery method. */
  price: number;

  /** The unique identifier for the delivery method. */
  id: number;
}