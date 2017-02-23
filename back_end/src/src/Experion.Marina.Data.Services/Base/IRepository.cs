using System.Collections.Generic;

namespace Experion.Marina.Data.Services
{
    /// <summary>
    ///  The repository interface defines a standard contract that repository components should implement.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
    public interface IRepository<TEntity, TIdentifier> where TEntity : class, IEntity<TIdentifier>
    {
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>Return all the entities</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Gets the by specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="useCSharpNullComparisonBehavior">if set to <c>true</c> [use c sharp null comparison behavior].</param>
        /// <returns>
        /// Return the entities based on the specification.
        /// </returns>
        List<TEntity> GetBySpecification(Specification<TEntity> specification, bool useCSharpNullComparisonBehavior = false);

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Return the Entity based on the primary key.</returns>
        TEntity GetById(TIdentifier id);

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Return the newly added entity with the primary key.</returns>
        TEntity Add(TEntity entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(TEntity entity);
    }
}