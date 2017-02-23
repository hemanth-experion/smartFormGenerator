using Autofac;
using Experion.Marina.Core;
using System;
using System.Data.Entity;

namespace Experion.Marina.Data.Services
{
    /// <summary>
    /// The Data Layer Service base class
    /// </summary>
    public abstract class DataService<TContext> where TContext : DbContext, IDisposable
    {
        private readonly IComponentContext _iocContext;

        #region Properties

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        /// <value>
        /// The data context.
        /// </value>
        protected IRepositoryContext RepositoryContext { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        /// <value>
        /// The data context.
        /// </value>
        protected IRepositoryContext DataContext { get; set; }

        #endregion Properties

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="DataService" /> class.
        /// </summary>
        /// <param name="iocContext">The ioc context.</param>
        /// <param name="context">The context.</param>
        protected DataService(IComponentContext iocContext, IRepositoryContext context)
        {
            _iocContext = iocContext;
            this.RepositoryContext = context;
            this.InitilizeContext();
        }

        #endregion Constructor(s)

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            this.DataContext.Dispose();
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        protected int Save()
        {
            var userId = string.Empty;
            //var appContext = _iocContext.Resolve<IApplicationContext>();
            //if (appContext != null)
            //{
            //    try
            //    {
            //        userId = appContext.GetUserId();
            //    }
            //    catch (Exception)
            //    {
            //    }
            //}
            return this.DataContext.Commit(userId);
        }

        /// <summary>
        /// Initializes the context.
        /// </summary>
        private void InitilizeContext()
        {
            this.DataContext = this.RepositoryContext;
        }
    }
}