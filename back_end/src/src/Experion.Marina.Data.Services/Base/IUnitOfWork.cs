using System;

namespace Experion.Marina.Data.Services
{
    public interface IUnitOfWork : IDisposable
    {
        #region Methods

        /// <summary>
        /// Commits this instance.
        /// </summary>
        /// <param name="additionalInfo">Any additional information to be passed to the SaveChanges method (Optional).</param>
        /// <returns>
        /// The number of affected rows.
        /// </returns>
        int Commit(string additionalInfo = "");

        #endregion Methods
    }
}
