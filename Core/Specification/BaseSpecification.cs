using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specification;

/// <summary>
/// Represents the base implementation of a specification pattern for querying entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the entity to which the specification applies.</typeparam>
public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="BaseSpecification{T}"/> class with an optional criteria expression.
  /// </summary>
  /// <param name="criteria">The criteria expression used to filter entities; or null if no filtering is applied.</param>
  public BaseSpecification() : this(null) {}

  /// <inheritdoc/>
  public Expression<Func<T, bool>>? Criteria => criteria;

  /// <inheritdoc/>
  public Expression<Func<T, object>>? OrderBy {get; private set;}

  /// <inheritdoc/>
  public Expression<Func<T, object>>? OrderByDescending {get; private set;}

  /// <inheritdoc/>
  public bool IsDistinct {get; private set;}

  /// <inheritdoc/>
  public int Take {get; private set;}

  /// <inheritdoc/>
  public int Skip {get; private set;}

  /// <inheritdoc/>
  public bool IsPagingEnabled {get; private set;}

  /// <summary>
  /// Gets the list of expressions to include related entities in the query.
  /// </summary>
  public List<Expression<Func<T, object>>> Includes { get; } = [];

  /// <summary>
  /// Gets the list of related entity names as strings to include in the query.
  /// </summary>
  public List<string> IncludeStrings { get; } = [];

  /// <inheritdoc/>
   public IQueryable<T> ApplyCriteria(IQueryable<T> query)
  {
    if(Criteria != null)
      query = query.Where(Criteria);
    
    return query;
  }

  /// <summary>
  /// Adds an expression to the list of includes to specify related entities in the query.
  /// </summary>
  /// <param name="includeExpression">An expression specifying the related entity to include.</param>
  protected void AddInclude(Expression<Func<T, object>> includeExpression) 
  {
    Includes.Add(includeExpression);
  }

  /// <summary>
  /// Adds a related entity name as a string to the list of includes.
  /// </summary>
  /// <param name="includeString">The name of the related entity to include.</param>
  protected void AddInclude(string includeString) 
  {
    IncludeStrings.Add(includeString);
  }

  /// <summary>
  /// Applies an ascending order expression to the specification.
  /// </summary>
  /// <param name="orderByExpression">The expression used to order entities in ascending order.</param>     
  protected void AddOrderBy(Expression<Func<T, object>> orderByExpression) 
  {
    OrderBy = orderByExpression;
  }

  /// <summary>
  /// Applies a descending order expression to the specification.
  /// </summary>
  /// <param name="orderByDescExpression">The expression used to order entities in descending order.</param>
  protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression) 
  {
    OrderByDescending = orderByDescExpression;
  }

  /// <summary>
  /// Applies a distinct filter to the specification, ensuring that the query returns distinct results.
  /// </summary>
  protected void ApplyDistinct()
  {
    IsDistinct = true;
  }

  /// <summary>
  /// Applies pagination to the query by specifying the number of records to skip and take.
  /// </summary>
  /// <param name="skip">The number of records to skip.</param>
  /// <param name="take">The number of records to take.</param>
  protected void ApplyPaging(int skip, int take) 
  {
    Skip = skip;
    Take = take;
    IsPagingEnabled = true;
  }
}

/// <summary>
/// Represents the base implementation of a specification pattern for querying and projecting entities of type <typeparamref name="T"/> to a result type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">The type of the entity to which the specification applies.</typeparam>
/// <typeparam name="TResult">The type to which the entity is projected.</typeparam>
public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria)
  : BaseSpecification<T>(criteria), ISpecification<T, TResult>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="BaseSpecification{T, TResult}"/> class with an optional criteria expression.
  /// </summary>
  /// <param name="criteria">The criteria expression used to filter entities; or null if no filtering is applied.</param>
  public BaseSpecification() : this(null) {}

  /// <summary>
  /// Gets the expression used to select and project entities to a result type.
  /// </summary>
  public Expression<Func<T, TResult>>? Select {get; private set;}
  
  /// <summary>
  /// Applies a select expression to the specification to project entities to a result type.
  /// </summary>
  /// <param name="selectExpression">The expression used to select and project entities.</param>
  protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
  {
    Select = selectExpression;
  }
} 