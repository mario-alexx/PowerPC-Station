/**
 * Represents a user with personal details and an associated address.
*/
export type User = {
  /** The first name of the user. */
  firstName: string;

  /** The last name of the user. */
  lastName: string;

  /** The email address of the user. */
  email: string;

  /** The address information of the user. */
  address: Address;
};

/**
 * Represents a physical address.
 */
export type Address = {
  /** The first line of the address, typically the street name or number. */
  line1: string;

  /** The second line of the address, optional (e.g., apartment number). */
  line2?: string;

  /** The city where the address is located. */
  city: string;

  /** The state or region where the address is located. */
  state: string;

  /** The country of the address. */
  country: string;

  /** The postal or ZIP code of the address. */
  postalCode: string;
};