import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../shared/models/product';
import { ProductItemComponent } from "./product-item/product-item.component";
import { MatDialog } from '@angular/material/dialog';
import { MatButton } from '@angular/material/button';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { ShopParams } from '../../shared/models/shopParams';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../shared/models/pagination';
import { FormsModule } from '@angular/forms';

/**
 * ShopComponent is responsible for displaying and managing the product list and filters in the shop.
*/
@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    ProductItemComponent,
    MatButton,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger,
    MatPaginator, 
    FormsModule
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})

/**
 * ShopComponent is responsible for managing the product listing, filtering, sorting, 
 * and pagination logic in the shop section of the application. It interacts with 
 * ShopService to fetch and display products.
*/
export class ShopComponent implements OnInit{
  
 /** Injects ShopService to handle API calls related to products, brands, and types. 
  * @private
 */
  private shopService = inject(ShopService);
  /** 
   * Injecting the MatDialog to handle the opening of filter dialogs.
   * @private 
  */
  private dialogService = inject(MatDialog);

  /** Stores the product data along with pagination details. */
  products? : Pagination<Product>;
  
  /** Array of sorting options available in the shop. 
   * @type {Array<{name: string, value: string}>}
  */
  sortOptions = 
  [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low-High', value: 'priceAsc'},
    {name: 'Price: High-Low', value: 'priceDesc'}
  ];

  /** Shop parameters for filtering, sorting, pagination. 
   * @type {ShopParams}
  */
  shopParams = new ShopParams();
  /** Options for how many items to show per page. 
   * @type {number[]}
  */
  pageSizeOptions = [5, 10, 15, 20];

  /** Lifecycle hook that runs after the component is initialized. */
  ngOnInit(): void 
  {
    this.initializeShop();
  }

  /** 
   * Initializes the shop by loading brands, types, and fetching products. 
  */
  initializeShop(): void
  {
    this.shopService.getBrands();
    this.shopService.getTypes();
    this.getProducts();
  }

  /** 
   * Fetches products based on the current shop parameters.
  */
  getProducts(): void
  {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: response => this.products = response,
      error: error => console.log(error)
    });
  }

  /** 
   * Handles search input changes and fetches updated products.
  */
  onSearchChange(): void 
  {
    this.shopParams.pageNumber = 1;  // Reset the page number when search is changed
    this.getProducts(); // Re-fetch products with updated search term
  }

  /**
   * Handles pagination events (e.g., page change or page size change).
   * @param {PageEvent} event - The event containing the new page or page size.
  */
  handlePageEvent(event: PageEvent): void
  {
    this.shopParams.pageNumber = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.getProducts();
  }

  /**
   * Handles sort option changes and fetches sorted products.
   * @param {MatSelectionListChange} event - The event containing the selected sort option.
  */
  onSortChange(event: MatSelectionListChange) 
  {
    const selectedOptions = event.options[0];

    if(selectedOptions)
    {
      this.shopParams.sort = selectedOptions.value;
      this.shopParams.pageNumber = 1;
      this.getProducts();
    }
  }

  /**
   * Opens a dialog for selecting filters (brands and types).
  */
  openFiltersDialog(): void
  {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedBrands: this.shopParams.brands,
        selectedTypes: this.shopParams.types
      }
    });

      // After closing the dialog, update the shop parameters and fetch products if filters changed.
    dialogRef.afterClosed().subscribe({
      next: result => 
      {
        if(result) 
        {
          this.shopParams.brands = result.selectedBrands;
          this.shopParams.types = result.selectedTypes;
          this.shopParams.pageNumber = 1;
          this.getProducts();         
        }
      }
    });
  }
}
