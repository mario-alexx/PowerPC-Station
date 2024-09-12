/**
 * Interface representing a product in the system.
 */
export interface Product 
{
  /** The unique identifier for the product. */
  id: number;

  /** The name of the product. */
  name: string;

  /** A brief description of the product. */
  description: string;

  /** The price of the product. */
  price: number;

  /** The quantity of the product available in stock. */
  quantityInStock: number;

  /** The brand name of the product. */
  brand: string;

  /** The type or category of the product (e.g., 'hardware', 'peripheral'). */
  type: string;

  /** The URL to the image representing the product. */
  pictureUrl: string;
}