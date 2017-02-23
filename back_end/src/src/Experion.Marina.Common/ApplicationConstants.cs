namespace Experion.Marina.Common
{
    /// <summary>
    /// This class will define the various application constants
    /// </summary>
    public static class ApplicationConstants
    {
        /// <summary>
        /// The constant used to refer the service request coming from
        /// </summary>
        public const string RequestedBy = "Requested-By";

        /// <summary>
        /// The name of the token that will be kept in the session of a user
        /// to store the authentication token
        /// The same will be mentioned as the key in the request headers
        /// </summary>
        public const string AuthToken = "Authorization";

        /// <summary>
        /// The key that will be used in the request headers to identify the language type
        /// </summary>
        public const string LanguageHeaderKey = "Language";

        /// <summary>
        /// The key that will be used in the request header to track the login session
        /// </summary>
        public const string SessionId = "SessionId";

        /// <summary>
        /// The key that will be used in the header to track the user information
        /// </summary>
        public const string RequestInfo = "Invoke";

        /// <summary>
        /// The key that sets the Authentication Token Lifetime in days
        /// </summary>
        public const int AuthTokenLifeTimeInDays = 14;
    }
}
