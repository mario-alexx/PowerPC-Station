/**
 * This class defines the parameters used for filtering, sorting, and paginating products in the shop.
 */
export class ShopParams
{
  
  /** List of selected brands to filter products */
  brands: string[] = [];

  /** List of selected types to filter products */
  types: string[] = [];

  /** Default sorting criterion: 'name' */
  sort = 'name';

  /** Current page number for pagination, default is 1 */
  pageNumber = 1;

  /** Page size for pagination, default is 10 */
  pageSize = 10;

  /** Search term to filter products by name */
  search = '';
}