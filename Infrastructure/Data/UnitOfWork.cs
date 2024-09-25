using System.Collections.Concurrent;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

/// <summary>
/// Implements the unit of work pattern, managing repositories and transaction commits.
/// </summary>
public class UnitOfWork(StoreContext context) : IUnitOfWork
{
  private readonly ConcurrentDictionary<string, object> _repositories = new();

  /// <inheritdoc>
  public async Task<bool> Complete()
  {
    return await context.SaveChangesAsync() > 0;
  }

  /// <summary>
  /// Disposes of the resources used by the unit of work.
  /// </summary>
  public void Dispose()
  {
    context.Dispose();
  }

  /// <inheritdoc>
  public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
  {
    var type = typeof(TEntity).Name;

    return (IGenericRepository<TEntity>)_repositories.GetOrAdd(type, t => 
    {
      var repositoryType = typeof(GenericRepository<>).MakeGenericType(typeof(TEntity));
      return Activator.CreateInstance(repositoryType, context) 
        ?? throw new InvalidOperationException(
          $"Could not create repository instance for {t}");
    });
  }
}
