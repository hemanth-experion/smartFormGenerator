namespace Experion.Marina.Data.Services
{
    public interface IRepositoryContext : IUnitOfWork
    {
        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
        /// <returns>return the repository context.</returns>
        IRepository<TEntity, TIdentifier> GetRepository<TEntity, TIdentifier>()
                    where TEntity : class, IEntity<TIdentifier>;
    }
}