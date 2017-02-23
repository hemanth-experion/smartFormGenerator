using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Experion.Marina.Data.Services
{
    /// <summary>
    /// Meta-data about a DbContext
    /// </summary>
    public static class DbContextMetadata
    {
        /// <summary>
        /// Finds the meta-data workspace.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private static MetadataWorkspace FindMetadataWorkspace(IObjectContextAdapter context)
        {
            var objectContext = context.ObjectContext;
            return objectContext.MetadataWorkspace;
        }

        /// <summary>
        /// Finds the object set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private static ObjectSet<T> FindObjectSet<T>(IObjectContextAdapter context)
            where T : class
        {
            var objectContext = context.ObjectContext;
            //this can throw an InvalidOperationException if it's not mapped
            var objectSet = objectContext.CreateObjectSet<T>();
            return objectSet;
        }

        /// <summary>
        /// Finds the navigation property collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private static IEnumerable<NavigationProperty> FindNavigationPropertyCollection<T>(
            IObjectContextAdapter context)
            where T : class
        {
            var objectSet = FindObjectSet<T>(context);
            var elementType = objectSet.EntitySet.ElementType;
            var navigationProperties = elementType.NavigationProperties;
            return navigationProperties;
        }

        /// <summary>
        /// Finds the names of the entities in a DbContext.
        /// </summary>
        public static IEnumerable<string> FindEntities(DbContext context)
        {
            var metadataWorkspace = FindMetadataWorkspace(context);
            var items = metadataWorkspace.GetItems<EntityType>(DataSpace.CSpace);
            return items.Select(t => t.FullName);
        }

        /// <summary>
        /// Finds the underlying table names.
        /// </summary>
        public static IEnumerable<string> FindTableNames(DbContext context)
        {
            var metadataWorkspace = FindMetadataWorkspace(context);
            //we don't have to force a meta-data load in Code First, apparently
            var items = metadataWorkspace.GetItems<EntityType>(DataSpace.SSpace);
            //name space name is not significant (it's not schema name)
            return items.Select(t => t.Name);
        }

        /// <summary>
        /// Finds the primary key property names for an entity of specified type.
        /// </summary>
        public static IEnumerable<string> FindPrimaryKey<T>(DbContext context)
            where T : class
        {
            var objectSet = FindObjectSet<T>(context);
            var elementType = objectSet.EntitySet.ElementType;
            return elementType.KeyMembers.Select(p => p.Name);
        }

        /// <summary>
        /// Determines whether the specified entity is transient.
        /// </summary>
        public static bool IsTransient<T>(DbContext context, T entity)
            where T : class
        {
            var pk = FindPrimaryKey<T>(context).First();
            //look it up on the entity
            var propertyInfo = typeof(T).GetProperty(pk);
            var propertyType = propertyInfo.PropertyType;
            //what's the default value for the type?
            var transientValue = propertyType.IsValueType ? Activator.CreateInstance(propertyType) : null;
            //is the pk the same as the default value (int == 0, string == null ...)
            return propertyInfo.GetValue(entity, null) == transientValue;
        }

        /// <summary>
        /// Finds the navigation properties (References and Collections)
        /// </summary>
        public static IEnumerable<string> FindNavigationProperties<T>(DbContext context)
            where T : class
        {
            var navigationProperties = FindNavigationPropertyCollection<T>(context);
            return navigationProperties.Select(p => p.Name);
        }

        /// <summary>
        /// Finds the navigation collection properties.
        /// </summary>
        public static IEnumerable<string> FindNavigationCollectionProperties<T>(DbContext context)
            where T : class
        {
            var navigationProperties = FindNavigationPropertyCollection<T>(context);

            return from navigationProperty in navigationProperties
                   where navigationProperty.ToEndMember.RelationshipMultiplicity ==
                        RelationshipMultiplicity.Many
                   select navigationProperty.Name;
        }

        /// <summary>
        /// Finds the navigation reference properties
        /// </summary>
        public static IEnumerable<string> FindNavigationReferenceProperties<T>(DbContext context)
            where T : class
        {
            var navigationProperties = FindNavigationPropertyCollection<T>(context);

            return from navigationProperty in navigationProperties
                   let end = navigationProperty.ToEndMember
                   where end.RelationshipMultiplicity == RelationshipMultiplicity.ZeroOrOne ||
                    end.RelationshipMultiplicity == RelationshipMultiplicity.One
                   select navigationProperty.Name;
        }
    }
}