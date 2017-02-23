namespace Experion.Marina.Data.Services
{
    /// <summary>
    /// Base contract for storing and retrieving the unique identifier of an entity.
    /// </summary>
    /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
    public interface IEntity<TIdentifier>
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        TIdentifier Id { get; set; }
    }
}
