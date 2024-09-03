using System;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

/// <summary>
/// Evaluates specifications and applies them to an <see cref="IQueryable{T}"/> to construct queries.
/// </summary>
/// <typeparam name="T">The type of the entity to which the specification applies, which must inherit from <see cref="BaseEntity"/>.</typeparam>
public class SpecificationEvaluator<T> where T : BaseEntity
{
  /// <summary>
  /// Applies a specification to an <see cref="IQueryable{T}"/> and returns the modified query.
  /// </summary>
  /// <param name="query">The initial query to which the specification will be applied.</param>
  /// <param name="spec">The specification defining the criteria and ordering to apply to the query.</param>
  /// <returns>An <see cref="IQueryable{T}"/> representing the query with the specification applied.</returns>
  public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec) 
  {
    if(spec.Criteria != null)
      query = query.Where(spec.Criteria);

    if(spec.OrderBy != null)
      query = query.OrderBy(spec.OrderBy);

    if(spec.OrderByDescending != null)
      query = query.OrderByDescending(spec.OrderByDescending);
    
    if(spec.IsDistinct)
      query = query.Distinct();
    
    return query;
  }

  /// <summary>
  /// Applies a specification with a projection to an <see cref="IQueryable{T}"/> and returns the modified query projected to <typeparamref name="TResult"/>.
  /// </summary>
  /// <typeparam name="TSpec">The type of the entity to which the specification applies.</typeparam>
  /// <typeparam name="TResult">The type to which the query is projected.</typeparam>
  /// <param name="query">The initial query to which the specification will be applied.</param>
  /// <param name="spec">The specification defining the criteria, ordering, and projection to apply to the query.</param>
  /// <returns>An <see cref="IQueryable{TResult}"/> representing the query with the specification and projection applied.</returns>
  public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> query, ISpecification<T, TResult> spec) 
  {
    if(spec.Criteria != null)
      query = query.Where(spec.Criteria);

    if(spec.OrderBy != null)
      query = query.OrderBy(spec.OrderBy);

    if(spec.OrderByDescending != null)
      query = query.OrderByDescending(spec.OrderByDescending);
    
    var selectQuery = query as IQueryable<TResult>;

    if(spec != null) 
      selectQuery = query.Select(spec.Select);
    
    if(spec.IsDistinct) 
      selectQuery = selectQuery?.Distinct();
    
    return selectQuery ?? query.Cast<TResult>();;
  }
}
