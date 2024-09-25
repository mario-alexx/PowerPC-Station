using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

/// <summary>
/// A generic repository for performing CRUD operations and applying specifications to entities that inherit from <see cref="BaseEntity"/>.
/// </summary>
/// <typeparam name="T">The type of the entity for which this repository is responsible, which must inherit from <see cref="BaseEntity"/>.</typeparam>
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
  private readonly StoreContext _context;

  /// <summary>
  /// Initializes a new instance of the <see cref="GenericRepository{T}"/> class with a specified database context.
  /// </summary>
  /// <param name="context">The database context to be used by this repository.</param>
  public GenericRepository(StoreContext context)
  {
    _context = context;
  }

  /// <inheritdoc />
  public void Add(T entity)
  {
    _context.Set<T>().Add(entity);
  }

  /// <inheritdoc/>
  public async Task<int> CountAsync(ISpecification<T> spec)
  {
    var query = _context.Set<T>().AsQueryable();

    query = spec.ApplyCriteria(query);

    return await query.CountAsync();
  }

  /// <inheritdoc />
  public bool Exists(int id)
  {
    return _context.Set<T>().Any(x => x.Id == id);
  }

  /// <inheritdoc />
  public async Task<T?> GetByIdAsync(int id)
  {
    return await _context.Set<T>().FindAsync(id);
  }

  /// <inheritdoc />
  public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
  {
    return await ApplySpecification(spec).FirstOrDefaultAsync();
  }

  /// <inheritdoc />
  public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec)
  {
    return await ApplySpecification(spec).FirstOrDefaultAsync();
  }

  /// <inheritdoc />
  public async Task<IReadOnlyList<T>> ListAllAsync()
  {
    return await _context.Set<T>().ToListAsync();
  }

  /// <inheritdoc />
  public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
  {
    return await ApplySpecification(spec).ToListAsync();
  }
  
  /// <inheritdoc />
  public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec)
  {
    return await ApplySpecification(spec).ToListAsync();
  }

  /// <inheritdoc />
  public void Remove(T entity)
  {
    _context.Set<T>().Remove(entity);
  }

  /// <inheritdoc />
  public void Update(T entity)
  {
    _context.Set<T>().Attach(entity);
    _context.Entry(entity).State = EntityState.Modified;
  }

  /// <summary>
  /// Applies a specification to the query for filtering and ordering.
  /// </summary>
  /// <param name="spec">The specification to apply.</param>
  /// <returns>An <see cref="IQueryable{T}"/> representing the query with the specification applied.</returns>
  private IQueryable<T> ApplySpecification(ISpecification<T> spec) 
  {
    return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
  }

  /// <summary>
  /// Applies a specification with projection to the query for filtering, ordering, and selecting specific fields.
  /// </summary>
  /// <typeparam name="TResult">The type to which the query is projected.</typeparam>
  /// <param name="spec">The specification to apply.</param>
  /// <returns>An <see cref="IQueryable{TResult}"/> representing the query with the specification and projection applied.</returns>
  private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> spec) 
  {
    return SpecificationEvaluator<T>.GetQuery<T, TResult>(_context.Set<T>().AsQueryable(), spec);
  }
}
