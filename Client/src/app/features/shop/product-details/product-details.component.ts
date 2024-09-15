import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../../../shared/models/product';
import { Observable } from 'rxjs';
import { CurrencyPipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatDivider } from '@angular/material/divider';
import { CartService } from '../../../core/services/cart.service';
import { FormsModule } from '@angular/forms';

/**
 * Component responsible for displaying product details and managing cart actions for a specific product.
 */
@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [
    CurrencyPipe,
    MatButton,
    MatIcon,
    MatFormField,
    MatInput,
    MatLabel,
    MatDivider,
    FormsModule
  ],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit{
  /** Injects the ShopService to access product-related operations. */
  private shopService = inject(ShopService);

  /** Injects the ActivatedRoute to access route parameters. */
  private activatedRoute = inject(ActivatedRoute);

  /** Injects the CartService to handle cart-related actions. */
  private cartService = inject(CartService);
  
  /**
   * The product to display in the details view. @type {Product | undefined}
  */
  product?: Product;

  /** Stores the quantity of the product currently in the cart. */
  quantityInCart = 0;

  /** The quantity of the product to add to the cart. */
  quantity = 1;


  /**
   * Angular lifecycle hook that is called after component initialization.
  */
  ngOnInit(): void 
  {
    this.loadProduct();
  }

  /**
   * Loads the product details by fetching the product from the API using the product ID from the route.
  */
  loadProduct(): void
  {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if(!id) return;

    this.shopService.getProduct(+id).subscribe({
      next: product => {
        this.product = product;
        this.updateQuantityInCart();
      },
      error: error => console.log(error)
    });
  }

  /**
   * Updates the cart by adding or removing the product based on the current quantity.
  */
  updateCart(): void 
  {
    if(!this.product) return;
    if(this.quantity > this.quantityInCart) 
    {
      const itemsToAdd = this.quantity - this.quantityInCart;
      this.quantityInCart += itemsToAdd;
      this.cartService.addItemToCart(this.product, itemsToAdd);
    }
    else 
    {
      const itemsToRemove = this.quantityInCart - this.quantity;
      this.quantityInCart -= itemsToRemove;
      this.cartService.removeItemFromCart(this.product.id, itemsToRemove);
    }
  }

  /**
   * Updates the quantity of the product in the cart based on current cart status.
  */
  updateQuantityInCart(): void
  {
    this.quantityInCart = this.cartService.cart()?.
      items.find(x => x.productId === this.product?.id)?.quantity || 0;

    this.quantity = this.quantityInCart || 1;
  }

  /**
   * Gets the appropriate text for the cart button based on the product's presence in the cart.
   * 
   * @returns The text to be displayed on the button.
  */
  getButtonText(): string
  {
    return this.quantityInCart > 0 ? 'Update cart' : 'Add to cart'
  }
}
