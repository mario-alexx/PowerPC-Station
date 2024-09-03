using Core.Entities;

namespace Core.Interfaces;

/// <summary>
/// A generic repository interface for managing entities in the data store.
/// </summary>
/// <typeparam name="T">The type of the entity, which must inherit from <see cref="BaseEntity"/>.</typeparam>
public interface IGenericRepository<T> where T : BaseEntity
{
  /// <summary>
  /// Asynchronously retrieves an entity by its unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier of the entity.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
  Task<T?> GetByIdAsync(int id);

  /// <summary>
  /// Asynchronously retrieves a read-only list of all entities.
  /// </summary>
  /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of all entities.</returns>
  Task<IReadOnlyList<T>> ListAllAsync();
  
  ///<summary>
  /// Asynchronously retrieves a single entity that matches the specified criteria.
  /// </summary>
  /// <param name="spec">The specification criteria to filter entities.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the entity that matches the criteria if found; otherwise, null.</returns>
  Task<T?> GetEntityWithSpec(ISpecification<T> spec);

  /// <summary>
  /// Asynchronously retrieves a read-only list of entities that match the specified criteria.
  /// </summary>
  /// <param name="spec">The specification criteria to filter entities.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of entities that match the criteria.</returns>
  Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
 
  /// <summary>
  /// Asynchronously retrieves a single entity that matches the specified criteria and projects it to the specified result type.
  /// </summary>
  /// <typeparam name="TResult">The result type to project the entity to.</typeparam>
  /// <param name="spec">The specification criteria to filter entities and define the projection.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the projected entity that matches the criteria if found; otherwise, null.</returns>
  Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec);
  
  /// <summary>
  /// Asynchronously retrieves a read-only list of entities that match the specified criteria and projects them to the specified result type.
  /// </summary>
  /// <typeparam name="TResult">The result type to project the entities to.</typeparam>
  /// <param name="spec">The specification criteria to filter entities and define the projection.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of projected entities that match the criteria.</returns>
  Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec);

  /// <summary>
  /// Adds a new entity to the data store.
  /// </summary>
  /// <param name="entity">The entity to add.</param>
  void Add(T entity);

  /// <summary>
  /// Updates an existing entity in the data store.
  /// </summary>
  /// <param name="entity">The entity to update.</param>
  void Update(T entity);

  /// <summary>
  /// Removes an entity from the data store.
  /// </summary>
  /// <param name="entity">The entity to remove.</param>
  void Remove(T entity);

  /// <summary>
  /// Asynchronously saves all changes made in the context to the data store.
  /// </summary>
  /// <returns>A task that represents the asynchronous save operation. The task result contains a boolean indicating whether the changes were successfully saved.</returns>
  Task<bool> SaveAllAsync();

  /// <summary>
  /// Checks if an entity exists in the data store by its unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier of the entity.</param>
  bool Exists(int id);
}
