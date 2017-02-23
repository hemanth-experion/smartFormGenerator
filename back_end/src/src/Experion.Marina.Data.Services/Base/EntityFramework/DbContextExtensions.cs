using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;

namespace Experion.Marina.Data.Services
{
    /// <summary>
    /// Code First extensions.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Adds an entity (if newly created) or update (if has non-default Id).
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context">The db context.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks>
        /// Will not work for HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).
        /// Will not work for composite keys.
        /// </remarks>
        public static TEntity AddOrUpdate<TEntity>(this DbContext context, TEntity entity) where TEntity : class
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (IsTransient(context, entity))
            {
                context.Set<TEntity>().Add(entity);
            }
            else
            {
                context.Set<TEntity>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            }
            return entity;
        }

        /// <summary>
        /// Gets the key names.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static string[] GetKeyNames<TEntity>(this DbContext context) where TEntity : class
        {
            return context.GetKeyNames(typeof(TEntity));
        }

        /// <summary>
        /// Gets the key names.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns></returns>
        public static string[] GetKeyNames(this DbContext context, Type entityType)
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            // Get the mapping between CLR types and meta-data OSpace
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get meta-data for given CLR type
            var entityMetadata = metadata
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .Single(e => objectItemCollection.GetClrType(e) == entityType);

            return entityMetadata.KeyProperties.Select(p => p.Name).ToArray();
        }

        /// <summary>
        /// Determines whether the specified entity is loaded from the database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="id">The id.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity is loaded; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Will not work for composite keys.
        /// </remarks>
        public static bool IsLoaded<TEntity>(this DbContext context, object id) where TEntity : class
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var property = FindPrimaryKeyProperty<TEntity>(context);
            //check to see if it's already loaded (slow if large numbers loaded)
            var entity = context.Set<TEntity>().Local
                .FirstOrDefault(x => id.Equals(property.GetValue(x, null)));
            return entity != null;
        }

        /// <summary>
        /// Determines whether the specified entity is newly created (Id not specified).
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity is transient; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Will not work for HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).
        /// Will not work for composite keys.
        /// </remarks>
        public static bool IsTransient<TEntity>(this DbContext context, TEntity entity) where TEntity : class
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var propertyInfo = FindPrimaryKeyProperty<TEntity>(context);
            var propertyType = propertyInfo.PropertyType;
            //what's the default value for the type?
            var transientValue = propertyType.IsValueType ?
                Activator.CreateInstance(propertyType) : null;
            //is the pk the same as the default value (int == 0, string == null ...)
            return Equals(propertyInfo.GetValue(entity, null), transientValue);
        }

        /// <summary>
        /// Loads a stub entity (or actual entity if already loaded).
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks>
        /// Will not work for composite keys.
        /// </remarks>
        public static TEntity Load<TEntity>(this DbContext context, object id) where TEntity : class
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var property = FindPrimaryKeyProperty<TEntity>(context);
            //check to see if it's already loaded (slow if large numbers loaded)
            var entity = context.Set<TEntity>().Local
                .FirstOrDefault(x => id.Equals(property.GetValue(x, null)));
            if (entity == null)
            {
                //it's not loaded, just create a stub with only primary key set
                entity = CreateEntity<TEntity>(id, property);

                context.Set<TEntity>().Attach(entity);
            }
            return entity;
        }
        /// <summary>
        /// Marks the reference navigation properties unchanged.
        /// Use when adding a new entity whose references are known to be unchanged.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="entity">The entity.</param>
        public static void MarkReferencesUnchanged<TEntity>(DbContext context, TEntity entity) where TEntity : class
        {
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            var objectSet = objectContext.CreateObjectSet<TEntity>();
            var elementType = objectSet.EntitySet.ElementType;
            var navigationProperties = elementType.NavigationProperties;
            //the references
            var references = from navigationProperty in navigationProperties
                             let end = navigationProperty.ToEndMember
                             where end.RelationshipMultiplicity == RelationshipMultiplicity.ZeroOrOne ||
                             end.RelationshipMultiplicity == RelationshipMultiplicity.One
                             select navigationProperty.Name;
            //NB: We don'TEntity check Collections. EF wants to handle the object graph.

            var parentEntityState = context.Entry(entity).State;
            foreach (var navigationProperty in references)
            {
                //if it's modified but not loaded, don'TEntity need to touch it
                if (parentEntityState == EntityState.Modified &&
                    !context.Entry(entity).Reference(navigationProperty).IsLoaded)
                    continue;
                var propertyInfo = typeof(TEntity).GetProperty(navigationProperty);
                var value = propertyInfo.GetValue(entity, null);
                context.Entry(value).State = EntityState.Unchanged;
            }
        }

        /// <summary>
        /// Merges a DTO into a new or existing entity attached/added to context
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="dataTransferObject">The data transfer object. It must have a primary key property of the same name and type as the actual entity.</param>
        /// <returns></returns>
        /// <remarks>
        /// Will not work for composite keys.
        /// </remarks>
        public static TEntity Merge<TEntity>(this DbContext context, object dataTransferObject) where TEntity : class
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (dataTransferObject == null)
            {
                throw new ArgumentNullException(nameof(dataTransferObject));
            }

            var property = FindPrimaryKeyProperty<TEntity>(context);
            //find the id property of the dto
            var idProperty = dataTransferObject.GetType().GetProperty(property.Name);
            if (idProperty == null)
            {
                throw new InvalidOperationException("Cannot find an id on the dataTransferObject");
            }
            var id = idProperty.GetValue(dataTransferObject, null);
            //has the id been set (existing item) or not (transient)?
            var propertyType = property.PropertyType;
            var transientValue = propertyType.IsValueType ?
                Activator.CreateInstance(propertyType) : null;
            var isTransient = Equals(id, transientValue);
            TEntity entity;
            if (isTransient)
            {
                //it's transient, just create a dummy
                entity = CreateEntity<TEntity>(id, property);
                //if DatabaseGeneratedOption(DatabaseGeneratedOption.None) and no id, this errors
                context.Set<TEntity>().Attach(entity);
            }
            else
            {
                //try to load from identity map or database
                entity = context.Set<TEntity>().Find(id);
                if (entity == null)
                {
                    //could not find entity, assume assigned primary key
                    entity = CreateEntity<TEntity>(id, property);
                    context.Set<TEntity>().Add(entity);
                }
            }
            //copy the values from DTO onto the entry
            context.Entry(entity).CurrentValues.SetValues(dataTransferObject);
            return entity;
        }
        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        private static TEntity CreateEntity<TEntity>(object id, PropertyInfo property) where TEntity : class
        {
            // consider IoC here
            var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
            //set the value of the primary key (may error if wrong type)
            property.SetValue(entity, id, null);
            return entity;
        }

        /// <summary>
        /// Finds the primary key property.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private static PropertyInfo FindPrimaryKeyProperty<TEntity>(IObjectContextAdapter context) where TEntity : class
        {
            //find the primary key
            var objectContext = context.ObjectContext;
            //this will error if it's not a mapped entity
            var objectSet = objectContext.CreateObjectSet<TEntity>();
            var elementType = objectSet.EntitySet.ElementType;
            var pk = elementType.KeyMembers.First();
            //look it up on the entity
            var propertyInfo = typeof(TEntity).GetProperty(pk.Name);
            return propertyInfo;
        }
    }
}