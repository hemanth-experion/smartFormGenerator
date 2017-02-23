using System;

namespace Experion.Marina.Core
{
    /// <summary>
    /// Keys to indicate various exceptions in the application
    /// </summary>
    public enum ErrorCode : int
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 100,

        /// <summary>
        /// The SMTP configuration unknown
        /// </summary>
        SmtpConfigurationUnknown = 101,

        /// <summary>
        /// The email template not found
        /// </summary>
        EmailTemplateNotFound = 102,

        /// <summary>
        /// The email template error
        /// </summary>
        EmailTemplateError = 103,

        /// <summary>
        /// The invalid token
        /// </summary>
        InvalidToken = 104,

        /// <summary>
        /// The invalid token
        /// </summary>
        SessionTimeout = 105,

        /// <summary>
        /// The authentication failed
        /// </summary>
        AuthenticationFailed = 106,
    }

    public class MarinaException : Exception
    {
        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MarinaException()
        {
            ErrorCode = ErrorCode.Unknown;
        }

        /// <summary>
        /// Constructor initializing ExceptionKey property.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        public MarinaException(ErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or sets exception key.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public ErrorCode ErrorCode { get; set; }

        #endregion Properties
    }
}