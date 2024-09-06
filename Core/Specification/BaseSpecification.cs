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

  /// <summary>
  /// Gets the criteria expression used to filter entities.
  /// </summary>
  public Expression<Func<T, bool>>? Criteria => criteria;
  /// <summary>
  /// Gets the expression used to order entities in ascending order.
  /// </summary>
  public Expression<Func<T, object>>? OrderBy {get; private set;}

  /// <summary>
  /// Gets the expression used to order entities in descending order.
  /// </summary>
  public Expression<Func<T, object>>? OrderByDescending {get; private set;}

  /// <summary>
  /// Gets a value indicating whether the query should return distinct results.
  /// </summary>
  public bool IsDistinct {get; private set;}

  public int Take {get; private set;}

  public int Skip {get; private set;}

  public bool IsPagingEnabled {get; private set;}

  public IQueryable<T> ApplyCriteria(IQueryable<T> query)
  {
    if(Criteria != null)
      query = query.Where(Criteria);
    
    return query;
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