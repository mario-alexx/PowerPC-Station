using System.Linq.Expressions;

namespace Core.Interfaces;

/// <summary>
/// Defines a specification pattern for querying entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the entity to which the specification applies.</typeparam>
public interface ISpecification<T>
{
  /// <summary>
  /// Gets the criteria expression used to filter entities.
  /// </summary>
  /// <value>An expression representing the criteria for filtering entities; or null if no filtering is applied.</value>
  Expression<Func<T, bool>>? Criteria{ get; }

  /// <summary>
  /// Gets the expression used to order entities in ascending order.
  /// </summary>
  /// <value>An expression representing the ordering criteria; or null if no ordering is applied.</value>
  Expression<Func<T, object>>? OrderBy { get; }

  /// <summary>
  /// Gets the expression used to order entities in descending order.
  /// </summary>
  /// <value>An expression representing the ordering criteria for descending order; or null if no ordering is applied.</value>
  Expression<Func<T, object>>? OrderByDescending { get; }

  /// <summary>
  /// Gets a list of expressions to include related entities in the query.
  /// </summary>
  List<Expression<Func<T, object>>> Includes { get; }

  /// <summary>
  /// Gets a list of related entity names as strings to include in the query.
  /// </summary>
  List<string> IncludeStrings { get; }

  /// <summary>
  /// Gets a value indicating whether the query should return distinct results.
  /// </summary>
  /// <value>True if the query should return distinct results; otherwise, false.</value>
  bool IsDistinct {get;}

  /// <summary>
  /// Gets the number of entities to take (for pagination purposes).
  /// </summary>
  int Take{get;}

  /// <summary>
  /// Gets the number of entities to skip (for pagination purposes).
  /// </summary>
  int Skip{get;}

  /// <summary>
  /// Indicates whether pagination is enabled.
  /// </summary>
  bool IsPagingEnabled {get;}

  /// <summary>
  /// Applies the specified filtering criteria, ordering, and pagination to the given query.
  /// </summary>
  /// <param name="query">The query to which the criteria will be applied.</param>
  /// <returns>The query with the criteria applied.</returns>
  IQueryable<T> ApplyCriteria(IQueryable<T> query);
}

/// <summary>
/// Defines a specification pattern for querying and projecting entities of type <typeparamref name="T"/> to a result type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">The type of the entity to which the specification applies.</typeparam>
/// <typeparam name="TResult">The type to which the entity is projected.</typeparam>
public interface ISpecification<T, TResult> : ISpecification<T>
{
  /// <summary>
  /// Gets the expression used to select and project entities to a result type.
  /// </summary>
  /// <value>An expression representing the projection of entities to a result type; or null if no projection is applied.</value>
  Expression<Func<T, TResult>>? Select { get; }
}