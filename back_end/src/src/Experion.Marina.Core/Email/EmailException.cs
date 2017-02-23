using System;

namespace Experion.Marina.Core
{
    public class EmailException : Exception
    {
        public const string EMAIL_AUTHENTICATION_ERROR = "EML002";
        public const string EMAIL_PARAMETER_ERROR = "EML001";

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailException"/> class.
        /// </summary>
        public EmailException()
        {
            ErrorCode = EMAIL_AUTHENTICATION_ERROR;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EmailException(string message) : base(message)
        {
            ErrorCode = EMAIL_AUTHENTICATION_ERROR;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="errorCode">The error code.</param>
        public EmailException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public EmailException(string message, Exception inner) : base(message, inner)
        {
            ErrorCode = EMAIL_AUTHENTICATION_ERROR;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="inner">The inner.</param>
        public EmailException(string message, string errorCode, Exception inner) : base(message, inner)
        {
            ErrorCode = errorCode;
        }

        public string ErrorCode { get; set; }
    }
}