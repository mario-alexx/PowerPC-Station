using Core.Entities;

namespace Core.Interfaces;

/// <summary>
/// Interface representing a unit of work pattern to handle multiple repositories.
/// </summary>
public interface IUnitOfWork : IDisposable
{
  /// <summary>
  /// Retrieves the generic repository for a given entity type.
  /// </summary>
  /// <typeparam name="TEntity">The type of the entity.</typeparam>
  /// <returns>The generic repository for the entity.</returns>
  IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

  /// <summary>
  /// Saves all changes made within the unit of work to the database.
  /// </summary>
  /// <returns>A task representing the asynchronous operation, with a boolean result indicating success or failure.</returns>
  Task<bool> Complete();
}
