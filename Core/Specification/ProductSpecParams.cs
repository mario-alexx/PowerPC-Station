namespace Core.Specification;

/// <summary>
/// Encapsulates the parameters used for product specifications, including pagination, filtering, and sorting.
/// </summary>
public class ProductSpecParams
{
  private const int MaxPageSize = 50;

    /// <summary>
    /// Gets or sets the current page index for pagination.
    /// The default value is 1.
    /// </summary>
  public int PageIndex { get; set; } = 1;

  private int _pageSize = 6;
  
  /// <summary>
  /// Gets or sets the page size for pagination.
  /// The value cannot exceed <see cref="MaxPageSize"/>.
  /// </summary>
  public int PageSize
  {
    get => _pageSize;
    set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
  }
  
  private List<string> _brands = [];

  /// <summary>
  /// Gets or sets the list of brands to filter the products by.
  /// </summary>
  public List<string> Brands
  {
    get => _brands;
    set 
    { 
      _brands = value.SelectMany(x => 
        x.Split(',', StringSplitOptions.RemoveEmptyEntries))
        .ToList();
    }
  }

  /// <summary>
  /// Gets or sets the list of product types to filter by.
  /// </summary>
  private List<string> _types = [];
  public List<string> Types
  {
    get => _types;
    set 
    { 
      _types = value.SelectMany(x => 
        x.Split(',', StringSplitOptions.RemoveEmptyEntries))
        .ToList();
    }
  }

  /// <summary>
  /// Gets or sets the sort order for products. Could be "priceAsc", "priceDesc", etc.
  /// </summary>
  public string? Sort { get; set; }

  private string? _search;

  /// <summary>
  /// Gets or sets the search term for filtering products by name.
  /// </summary>
  public string Search
  {
    get => _search ?? "";
    set => _search = value.ToLower();
  }
  
}
