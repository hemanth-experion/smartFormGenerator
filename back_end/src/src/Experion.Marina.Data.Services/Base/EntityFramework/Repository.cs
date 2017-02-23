using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Experion.Marina.Data.Services
{
    /// <summary>
    /// Implementation of IRepository that uses Entity Framework for the repository operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
    public class Repository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier>
            where TEntity : class, IEntity<TIdentifier>
    {
        #region Imports

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity, TIdentifier}" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.ArgumentNullException">context is null</exception>
        public Repository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            DbContext = context;
        }

        #endregion Imports

        #region Properties

        /// <summary>
        /// Gets or sets the database context.
        /// </summary>
        /// <value>
        /// The database context.
        /// </value>
        public DbContext DbContext { get; set; }

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
        /// Gets all.
        /// </summary>
        /// <returns>
        /// return all the entities.
        /// </returns>
        public IEnumerable<TEntity> GetAll()
        {
            DbSet<TEntity> dataSet = this.DbContext.Set<TEntity>();
            return dataSet.AsEnumerable<TEntity>();
        }

        /// <summary>
        /// Gets the by specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="useCSharpNullComparisonBehavior">if set to <c>true</c> [use c sharp null comparison behavior].</param>
        /// <returns>
        /// return the entity list based on the specification
        /// </returns>
        /// <exception cref="System.Data.InvalidExpressionException">Argument Predicate is missing</exception>
        public List<TEntity> GetBySpecification(Specification<TEntity> specification, bool useCSharpNullComparisonBehavior = true)
        {
            List<TEntity> items;

            if (specification.Predicate == null)
            {
                throw new InvalidExpressionException("Argument Predicate is missing");
            }

            var adapter = (IObjectContextAdapter)DbContext;
            var objectContext = adapter.ObjectContext;
            objectContext.ContextOptions.UseCSharpNullComparisonBehavior = useCSharpNullComparisonBehavior;
            items = this.DbContext.Set<TEntity>().Where(specification.Predicate).ToList();

            // This code is added to ensure that the above query executed properly.
            // For example, if data connection is invalid, the above statement will not throw an exception.
            // So, while accessing the items, an exception shall be thrown in case of any failures.
            // var count = items.Count();
            return items;
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// return the entity based on the primary key value.
        /// </returns>
        public TEntity GetById(TIdentifier id)
        {
            DbSet<TEntity> dataSet = this.DbContext.Set<TEntity>();

            var objSet = this.Context.CreateObjectSet<TEntity>();
            var entitySet = objSet.EntitySet;
            string[] keyNames = entitySet.ElementType.KeyMembers.Select(k => k.Name).ToArray();

            if (keyNames.Length == 1)
            {
                return dataSet.Find(id);
            }

            object[] compositeIds = new object[keyNames.Length];
            object compositeKeyIdentifier = id;
            PropertyDescriptorCollection keyObjectProperties = TypeDescriptor.GetProperties(compositeKeyIdentifier);
            int counter = 0;
            foreach (var keyMember in keyNames)
            {
                PropertyDescriptor propertyDescriptor = keyObjectProperties.Find(keyMember, true);
                var val = propertyDescriptor.GetValue(compositeKeyIdentifier);
                compositeIds[counter++] = val;
            }

            return dataSet.Find(compositeIds);
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// The newly added entity with the primary key.
        /// </returns>
        /// <exception cref="System.Data.InvalidExpressionException">Argument could not be default</exception>
        public TEntity Add(TEntity entity)
        {
            if (object.Equals(entity, default(TEntity)))
            {
                throw new InvalidExpressionException("Argument could not be default");
            }

            DbSet<TEntity> dataSet = this.DbContext.Set<TEntity>();
            dataSet.Add(entity);
            return entity;
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.Data.InvalidExpressionException">Argument could not be default
        public void Update(TEntity entity)
        {
            if (object.Equals(entity, default(TEntity)))
            {
                throw new InvalidExpressionException("Argument could not be default");
            }

            var entityState = DbContext.Entry<TEntity>(entity).State;
            if (entityState == System.Data.Entity.EntityState.Deleted)
            {
                throw new InvalidExpressionException("Argument could not be updated");
            }

            var objSet = this.Context.CreateObjectSet<TEntity>();
            Context.CreateEntityKey(objSet.EntitySet.Name, entity);
            this.DbContext.Entry<TEntity>(entity).State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.Data.InvalidExpressionException">Argument could not be default</exception>
        public void Delete(TEntity entity)
        {
            if (object.Equals(entity, default(TEntity)))
            {
                throw new InvalidExpressionException("Argument could not be default");
            }

            var objSet = this.Context.CreateObjectSet<TEntity>();
            Context.CreateEntityKey(objSet.EntitySet.Name, entity);
            this.Context.DeleteObject(entity);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            this.Context.Dispose();
        }

        #endregion Public Methods
    }
}
