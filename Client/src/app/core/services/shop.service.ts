import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/product';
import { Observable } from 'rxjs';
import { ShopParams } from '../../shared/models/shopParams';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})

/**
 * Service responsible for interacting with the product-related API endpoints.
*/
export class ShopService {

  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

   /**
   * List of available product types.
   * @type {string[]}
   */
  types: string[] = [];
  /**
   * List of available product brands.
   * @type {string[]}
   */
  brands: string[] = [];

  /**
   * Fetches a paginated list of products based on the provided shop parameters.
   * 
   * @param {ShopParams} shopParams - The parameters used to filter and paginate the products.
   * @returns {Observable<Pagination<Product>>} - An observable that emits the paginated product data.
  */
  getProducts(shopParams: ShopParams): Observable<Pagination<Product>>
  {
    let params = new HttpParams();

    if(shopParams.brands.length > 0) 
      params = params.append('brands', shopParams.brands.join(','));

    if(shopParams.types.length > 0) 
      params = params.append('types', shopParams.types.join(','));
    
    if(shopParams.sort)
      params = params.append('sort', shopParams.sort);
    
    if(shopParams.search)
      params = params.append('search', shopParams.search);

    params = params.append('pageSize', shopParams.pageSize);
    params = params.append('pageIndex', shopParams.pageNumber);

    return this.http.get<Pagination<Product>>(this.baseUrl + 'products', {params});
  }

  /**
   * Retrieves a product by its ID from the API.
   * 
   * @param {number} id - The ID of the product to retrieve.
   * @returns {Observable<Product>} An observable that emits the product details.
  */
  getProduct(id: number): Observable<Product> 
  {
    return this.http.get<Product>(this.baseUrl + 'products/' + id);
  }

  /**
   * Fetches the list of available brands and updates the internal brands list.
   * @returns {void}
  */
  getBrands()
  {
    if(this.brands.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'products/brands').subscribe({
      next: response => this.brands = response
    });
  }

  /**
   * Fetches the list of available types and updates the internal types list. 
   * @returns {void}
  */
  getTypes()
  {
    if(this.types.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'products/types').subscribe({
      next: response => this.types = response
    });
  }
}
