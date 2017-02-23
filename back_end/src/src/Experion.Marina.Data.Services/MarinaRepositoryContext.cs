namespace Experion.Marina.Data.Services
{
    public class MarinaRepositoryContext : RepositoryContext<MarinaContext>
    {
        /// <summary>
        /// The connection string
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarinaRepositoryContext" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MarinaRepositoryContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Creates the context.
        /// </summary>
        /// <returns>Marina Container</returns>
        protected override MarinaContext CreateContext() => new MarinaContext(_connectionString);
    }
}
