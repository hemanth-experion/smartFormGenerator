using Experion.Marina.Core;
using System.Collections.Generic;

namespace Experion.Marina.Business.Services
{
    public class MasterDataHandler
    {
        #region Private Members

        private readonly IApplicationContext _appContext = null;

        #endregion Private Members

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterDataHandler"/> class.
        /// </summary>
        /// <param name="appContext">The application context.</param>
        public MasterDataHandler(IApplicationContext appContext)
        {
            _appContext = appContext;
        }

        /// <summary>
        /// Gets the application setting value.
        /// </summary>
        /// <param name="settingKey">The setting key.</param>
        /// <returns></returns>
        public string GetApplicationSettingValue(string settingKey)
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets the resource values by language.
        /// </summary>
        /// <param name="stringKey">The string key.</param>
        /// <returns></returns>
        public Dictionary<string, string> GetResourceValuesByLanguage(string stringKey)
        {
            return null;
        }

        /// <summary>
        /// Gets the resource value.
        /// </summary>
        /// <param name="stringKey">The string key.</param>
        /// <returns></returns>
        public string GetResourceValue(string stringKey)
        {
            var currentSelectedLanguage = _appContext.GetUserObject(Keys.UserLanguage.ToString());
            return GetResourceValue((currentSelectedLanguage != null ? (string)currentSelectedLanguage : "en"), stringKey);
        }

        /// <summary>
        /// Gets the resource value.
        /// </summary>
        /// <param name="languageName">Name of the language.</param>
        /// <param name="stringKey">The string key.</param>
        /// <returns></returns>
        public string GetResourceValue(string languageName, string stringKey)
        {
            return string.Empty;
        }

        #endregion Public Methods
    }
}
