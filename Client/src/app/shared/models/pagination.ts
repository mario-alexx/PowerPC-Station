/**
 * Generic interface for pagination, used to structure paginated data responses.
 * @template T The type of data being paginated.
 */
export interface Pagination<T> 
{
  /** The current page index, starting from 1. */
  pageIndex: number;

  /** The number of items per page. */
  pageSize: number;

  /** The total count of items available for pagination. */
  count: number;

  /** The array of data items for the current page. */
  data: T[];
}