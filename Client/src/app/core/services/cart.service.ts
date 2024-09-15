import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cart, CartItem } from '../../shared/models/cart';
import { Product } from '../../shared/models/product';
import { map, Observable, Subscription } from 'rxjs';

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

  /**
   * Computes the total number of items in the cart.
   * @returns The total item count in the cart.
  */
  itemCount = computed(() => {
    return this.cart()?.items.reduce( (sum, item) => sum + item.quantity, 0);
  });

  /**
   * Computes the cart totals including subtotal, shipping, discount, and total amount.
   * @returns The cart totals as an object with subtotal, shipping, discount, and total.
  */
  totals = computed(() => {
    const cart = this.cart();
    if(!cart) return null;
    const subtotal = cart.items.reduce( (sum, item) => sum + item.price * item.quantity, 0);
    const shipping = 0;
    const discount = 0;
    return {
      subtotal,
      shipping,
      discount,
      total: subtotal + shipping - discount
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

  /**
   * Sends the cart data to the server to update it.
   * @param cart - The Cart object to be saved.
   * @returns A subscription to the HTTP request.
  */
  setCart(cart: Cart): Subscription
  {
    return this.http.post<Cart>(this.baseUrl + 'cart', cart).subscribe({
      next: cart => this.cart.set(cart)
    });
  }

   /**
   * Adds an item to the cart or updates its quantity if it already exists.
   * @param item - The product or cart item to be added.
   * @param quantity - The quantity to be added (default is 1).
   */
  addItemToCart(item: CartItem | Product, quantity = 1): void
  {
    const cart = this.cart() ?? this.createCart();
    if(this.isProduct(item)) 
    {
      item = this.mapProductToCartItem(item);
    }
    cart.items = this.addOrUpdateItems(cart.items, item, quantity);
    this.setCart(cart);
  } 

  /**
   * Removes a specific quantity of an item from the cart.
   * @param productId - The ID of the product to remove.
   * @param quantity - The quantity to be removed (default is 1).
  */
  removeItemFromCart(productId: number,quantity = 1): void
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
        this.setCart(cart);
      }
    }
  }

  /**
   * Deletes the current cart.
  */
  private deleteCart(): void
  {
    this.http.delete(this.baseUrl + 'cart' + this.cart()?.id).subscribe({
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
}
