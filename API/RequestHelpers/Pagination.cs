namespace API.RequestHelpers;

/// <summary>
/// Represents a paginated response containing a subset of data and pagination details.
/// </summary>
/// <typeparam name="T">The type of the items in the data collection.</typeparam>
/// <param name="pageIndex">The current page index.</param>
/// <param name="pageSize">The number of items per page.</param>
/// <param name="count">The total number of items.</param>
/// <param name="data">The data for the current page.</param>
public class Pagination<T>(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
{
  /// <summary>
  /// Gets or sets the current page index.
  /// </summary>
  public int PageIndex { get; set; } = pageIndex;

  /// <summary>
  /// Gets or sets the number of items per page.
  /// </summary>
  public int PageSize { get; set; } = pageSize;

  /// <summary>
  /// Gets or sets the total number of items.
  /// </summary>
  public int Count { get; set; } = count;

  /// <summary>
  /// Gets or sets the data for the current page.
  /// </summary>
  public IReadOnlyList<T> Data { get; set; } = data; 
}
