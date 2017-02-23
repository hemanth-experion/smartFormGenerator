using System.Resources;

namespace Experion.Marina.Core
{
    /// <summary>
    /// The Business Service base class
    /// </summary>
    public abstract class BusinessService
    {
        #region Properties

        /// <summary>
        /// Gets or sets the resource manager.
        /// </summary>
        /// <value>
        /// The resource manager.
        /// </value>
        protected ResourceManager ResourceManager { get; set; }

        #endregion Properties

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessService"/> class.
        /// </summary>
        protected BusinessService()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessService" /> class.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        protected BusinessService(ResourceManager resourceManager)
        {
            ResourceManager = resourceManager;
        }

        #endregion Constructor(s)

        #region Protected Methods

        /// <summary>
        /// Gets the localized string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>the localized string</returns>
        protected string GetLocalizedString(string key) => ResourceManager.GetString(key);

        #endregion Protected Methods
    }
}
