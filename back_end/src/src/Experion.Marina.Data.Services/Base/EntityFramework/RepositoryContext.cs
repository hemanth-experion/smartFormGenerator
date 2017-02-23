using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Z.EntityFramework.Plus;

namespace Experion.Marina.Data.Services
{
    /// <summary>
    /// Implements the <see cref="IRepositoryContext"/> interface to provide an implementation
    /// that manages the repository context.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public abstract class RepositoryContext<TContext> : IRepositoryContext where TContext : DbContext
    {
        #region Field(s)

        /// <summary>
        /// The context
        /// </summary>
        private TContext context;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool disposed;
        #endregion Field(s)

        #region Properties

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>
        /// The database context.
        /// </value>
        public TContext DbContext
        {
            get
            {
                if (this.context == null)
                {
                    this.context = this.CreateContext();
                }

                return this.context;
            }
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        private System.Data.Entity.Core.Objects.ObjectContext Context => ((IObjectContextAdapter)this.DbContext).ObjectContext;

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Commits this instance.
        /// </summary>
        /// <returns>The number of affected records.</returns>
        public int Commit(string user = "")
        {
            ////OnBeforeSaveChanges();
            var result = 0;
            if (string.IsNullOrEmpty(user))
            {
                result = this.DbContext.SaveChanges();
            }
            else
            {
                var audit = new Audit();
                audit.CreatedBy = user;
                result = this.DbContext.SaveChanges(audit);
            }

            ////OnAfterSaveChanges();

            return result;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
        /// <returns>The repository</returns>
        public IRepository<TEntity, TIdentifier> GetRepository<TEntity, TIdentifier>()
                    where TEntity : class, IEntity<TIdentifier>
        {
            var repository = new Repository<TEntity, TIdentifier>(this.DbContext);
            return repository;
        }

        /// <summary>
        /// Rollbacks this instance.
        /// </summary>
        public void Rollback()
        {
            foreach (var ent in this.DbContext.ChangeTracker
                     .Entries()
                     .Where(p => p.State == EntityState.Deleted || p.State == EntityState.Modified))
            {
                ent.State = EntityState.Unchanged;
            }

            foreach (var ent in this.DbContext.ChangeTracker
                    .Entries()
                    .Where(p => p.State == EntityState.Added))
            {
                ent.State = EntityState.Detached;
            }
        }
        /// <summary>
        /// Creates the context.
        /// </summary>
        /// <returns>The Context</returns>
        protected abstract TContext CreateContext();

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
        }

        #endregion Private Methods
    }
}