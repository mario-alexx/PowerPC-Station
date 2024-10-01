import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cart, CartItem, Coupon } from '../../shared/models/cart';
import { Product } from '../../shared/models/product';
import { firstValueFrom, map, Observable, Subscription, tap } from 'rxjs';
import { DeliveryMethod } from '../../shared/models/deliveryMethod';

/**
 * Service responsible for managing the shopping cart operations.
*/
@Injectable({
  providedIn: 'root'
})
export class CartService {

  /** Base URL for the API endpoints */
  baseUrl = environment.apiUrl;

  /** Injects the HttpClient to handle HTTP requests */
  private http = inject(HttpClient);

  /** Holds the current shopping cart state */
  cart = signal<Cart | null>(null);

  /** Computed property that returns the total item count in the cart. */
  itemCount = computed(() => {
    return this.cart()?.items.reduce((sum, item) => sum + item.quantity, 0)
  });

  /**
   * Signal to hold the currently selected delivery method.
   * Starts as null if no delivery method is selected.
  */
  selectedDelivery = signal<DeliveryMethod | null>(null);

  /**
   * Computes the cart totals including subtotal, shipping, discount, and total amount.
   * @returns The cart totals as an object with subtotal, shipping, discount, and total.
  */
  totals = computed(() => {
    const cart = this.cart();
    const delivery = this.selectedDelivery();

    if(!cart) return null;
    const subtotal = cart.items.reduce((sum, item) => 
      sum + item.price * item.quantity, 0);

    let discountValue = 0

    if(cart.coupon)
    {
      if(cart.coupon.amountOff)
      {
        discountValue = cart.coupon.amountOff;
      }
      else if(cart.coupon.percentOff)
      {
        discountValue = subtotal * (cart.coupon.percentOff / 100);
      }
    }

    const shipping = delivery ? delivery.price : 0;
    const total  = subtotal + shipping - discountValue;
    return {
      subtotal,
      shipping,
      discount: discountValue,
      total
    };
  });

  /**
   * Retrieves the cart by its ID from the server.
   * @param id - The cart ID.
   * @returns An observable of the Cart object.
  */
  getCart(id: string): Observable<Cart>
  {
    return this.http.get<Cart>(this.baseUrl + 'cart?id=' + id).pipe(
      map(cart => {
        this.cart.set(cart);
        return cart;
      })
    )
  }

  /** Sends the cart data to the server to update it. */
  setCart(cart: Cart): Observable<Cart>
  {
    return this.http.post<Cart>(this.baseUrl + 'cart', cart).pipe(
      tap(cart => {
        this.cart.set(cart); 
      })
    )
  }

   /**
   * Adds an item to the cart or updates its quantity if it already exists.
   * @param item - The product or cart item to be added.
   * @param quantity - The quantity to be added (default is 1).
   */
  async addItemToCart(item: CartItem | Product, quantity = 1): Promise<void>
  {
    const cart = this.cart() ?? this.createCart();
    if(this.isProduct(item)) 
    {
      item = this.mapProductToCartItem(item);
    }
    cart.items = this.addOrUpdateItems(cart.items, item, quantity);
    await firstValueFrom(this.setCart(cart));
  } 

  /**
   * Removes a specific quantity of an item from the cart.
   * @param productId - The ID of the product to remove.
   * @param quantity - The quantity to be removed (default is 1).
  */
  async removeItemFromCart(productId: number,quantity = 1): Promise<void>
  {
    const cart = this.cart();
    if(!cart) return;
    const index = cart.items.findIndex(x => x.productId === productId);
    if(index !== -1) 
    {
      if(cart.items[index].quantity > quantity) 
      {
        cart.items[index].quantity -= quantity;
      }
      else 
      {
        cart.items.splice(index, 1);
      }
      if(cart.items.length === 0) 
      {
        this.deleteCart();
      }
      else 
      {
        await firstValueFrom(this.setCart(cart));
      }
    }
  }

  /**
   * Deletes the current cart.
  */
  deleteCart(): void
  {
    this.http.delete(this.baseUrl + 'cart?id=' + this.cart()?.id).subscribe({
      next: () => {
        localStorage.removeItem('cart_id');
        this.cart.set(null);
      }
    });
  }
  
  /**
   * Adds or updates items in the cart.
   * @param items - The list of current items in the cart.
   * @param item - The item to be added or updated.
   * @param quantity - The quantity to be added or updated.
   * @returns The updated array of cart items.
  */
  private addOrUpdateItems(items: CartItem[], item: CartItem, quantity: number): CartItem[] 
  {
    const index = items.findIndex(x => x.productId === item.productId);
    if(index === -1) 
    {
      item.quantity = quantity;
      items.push(item);
    }
    else 
    {
      items[index].quantity += quantity;
    }
    return items;
  }

  /**
   * Maps a product object to a cart item object.
   * @param item - The product to be mapped.
   * @returns A CartItem object derived from the product.
  */
  private mapProductToCartItem(item: Product): CartItem 
  {
    return {
      productId: item.id,
      productName: item.name,
      price: item.price,
      quantity: 0,
      pictureUrl: item.pictureUrl,
      brand: item.brand,
      type: item.type
    }
  }

  /**
   * Type guard to determine if the provided item is a Product.
   * @param item - The item to check.
   * @returns True if the item is a Product, false otherwise.
  */
  private isProduct(item: CartItem | Product): item is Product 
  {
    return (item as Product).id !== undefined;
  }

  /**
   * Creates a new cart with an empty item list.
   * @returns A new Cart object.
  */
  private createCart(): Cart
  {
    const cart = new Cart();
    localStorage.setItem('cart_id', cart.id);
    return cart;
  }

  /** 
   * Sends the cart data to the server to apply a discount using the provided coupon code.
   * @param code The coupon code to apply.
   * @returns An observable that resolves to the coupon data.
  */
  applyDiscount(code: string): Observable<Coupon>
  {
    return this.http.get<Coupon>(this.baseUrl + 'coupons/' + code);
  }
}
