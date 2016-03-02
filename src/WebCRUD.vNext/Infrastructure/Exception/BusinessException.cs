using System;

namespace WebCRUD.vNext.Infrastructure.Exception
{
    /// <summary>
    /// Use this class to signal business rules violation, validation and other exceptional situation that are releated to 
    /// </summary>
    public class BusinessException : System.Exception
    {
        /// <summary>
        /// Error code. Assign each of your business exceptions a unique code 
        /// so that it can be checked and handled in your code.
        /// For humans use Message property.
        /// </summary>
        public string ErrorCode { get; protected set; }

        public BusinessException(string errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public BusinessException(string errorCode, string messageTemplate, params object[] messageParameters)
            : base(string.Format(messageTemplate,messageParameters))
        {
            ErrorCode = errorCode;
        }
    }
}